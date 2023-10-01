using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
    public RectTransform winInformation;

    public NetworkController network;
    public AdminMapGenerator generator;
    public List<MapPlayer> players;
    public MapPlayer playerBase;
    public string playerId;

    public List<Color> playerColors;

    private NewPlayerMessage newPlayer;
    private Queue<PlayerPosMessage> newPos = new Queue<PlayerPosMessage>();
    private Queue<string> newBoard = new Queue<string>();

    private Dictionary<string, UnityAction<string>> responses = new Dictionary<string, UnityAction<string>>();

    private void Start()
    {
        responses.Add("assigned", OnIDAssigned);
        responses.Add("board_change", OnTileModified);
        responses.Add("new_player_enter", OnPlayerEntered);
        responses.Add("playerLeave", OnPlayerExit);
        responses.Add("move", OnPlayerMoved);
        responses.Add("game_ends", OnGameEneded);
    }

    private void Update()
    {
        if (newPlayer != null)
            CreatePlayer();

        if(newPos.Count > 0)
        {
            while(newPos.Count > 0)
            {
                MovePlayer(newPos.Peek());
                newPos.Dequeue();
            }
        }

        if (newBoard.Count > 0)
        {
            while (newBoard.Count > 0)
            {
                ModifyTile(newBoard.Peek());
                newBoard.Dequeue();
            }
        }

    }

    [ContextMenu("Connect")]
    public void ConnectToServer()
    {
        network.Connect(JoinServer);
    }

    public void JoinServer()
    {
        generator.GenerateModel();
        generator.Generate();

        network.SendData(JsonUtility.ToJson(new AdminMessage()));
        network.SendData(JsonUtility.ToJson(new StartMessage(generator.mapModel)));

        network.ws.OnMessage += Ws_OnMessage;

        Debug.Log(JsonUtility.ToJson(generator.mapModel));
    }

    #region Receivers
    private void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
    {
        JObject msg = JObject.Parse(e.Data);
        JProperty prop = msg.Properties().ToList().Find(c => c.Name == "key");
        responses[prop.Value.ToString()]?.Invoke(e.Data);
    }

    private void OnIDAssigned(string data)
    {
        IdAssignMessage msg = JsonUtility.FromJson<IdAssignMessage>(data);
        if (msg != null)
        {
            playerId = msg.playerId;
        }
    }

    public void OnTileModified(string data)
    {
        newBoard.Enqueue(data);
    }

    private void ModifyTile(string data)
    {
        var msg = JsonUtility.FromJson<BoardChangeMessage>(data);
        JObject jo = JObject.Parse(data);
        var prop = jo.Properties().ToList().Find(c => c.Name == "payload");
        string truVa = JsonConvert.SerializeObject(prop.Value);
        msg.payload = truVa;

        CellModel mod = JsonUtility.FromJson<CellModel>(msg.payload);
        var cell = generator.currentCells.Find(c => c.model.cellId == mod.cellId);
        if (cell != null)
        {
            cell.model.type = mod.type;
            cell.OverrideDefinition(generator.fieldDefinitions.Find(c => c.type == mod.type));
        }
    }

    public void OnPlayerEntered(string data)
    {
        newPlayer = JsonUtility.FromJson<NewPlayerMessage>(data);
    }

    private void CreatePlayer()
    {
        MapPlayer newPlayer = Instantiate(playerBase, transform);
        newPlayer.playerId = this.newPlayer.playerId;
        newPlayer.gameObject.SetActive(true);
        newPlayer.Init(playerColors[players.Count % playerColors.Count]);
        players.Add(newPlayer);
        this.newPlayer = null;
    }

    public void OnPlayerExit(string data)
    {
        Debug.Log(data);
        PlayerDeadMessage msg = JsonUtility.FromJson<PlayerDeadMessage>(data);
        Debug.Log(msg);
        if (msg != null)
        {
            MapPlayer player = players.Find(c => c.playerId == msg.playerId);
            if (player != null)
            {
                players.Remove(player);
                GameObject.Destroy(player);
            }
        }
    }

    public void OnPlayerMoved(string data)
    {
        PlayerPosMessage msg = JsonUtility.FromJson<PlayerPosMessage>(data);
        if (msg != null)
        {
            newPos.Enqueue(msg);
        }
    }

    private void MovePlayer(PlayerPosMessage msg)
    {
        if (players.Count == 0 || msg.playerId == null || msg.playerId == string.Empty)
            return;

        var player = players.Find(c => c.playerId == msg.playerId);
        player?.MovePlayer(JsonUtility.FromJson<PlayerPositionModel>(msg.payload));

        foreach (var cell in generator.currentCells)
            cell.PlayerExit();

        foreach(var play in players)
        {
            Vector2 pos = new Vector2(Mathf.Round(play.transform.position.x), Mathf.Round(play.transform.position.z));
            var cell = generator.currentCells.Find(c => c.model.cellPosition == pos);
            if (cell != null)
                cell.PlayerEnter();
        }
    }

    public void OnGameEneded(string msg)
    {
        winInformation.gameObject.SetActive(true);
    }
    #endregion
}
