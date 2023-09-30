using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ClientMockup : MonoBehaviour
{
    public NetworkController network;
    public bool ready;

    [Header("To Send")]
    public PlayerPosMessage posMsg;
    public PlayerPositionModel posMod;
    public CellModel cellToModify;

    [Header("Current dat")]
    public string playerId;
    public HintModel currentHint;
    public MapModel currentMap;

    private Dictionary<string, UnityAction<string>> responses = new Dictionary<string, UnityAction<string>>();

    private void Start()
    {
        responses.Add("hint", OnHintReceived);
        responses.Add("assigned", OnIDAssigned);
        responses.Add("board_change", OnTileModified);
        Connect();
    }

    private void Update()
    {
        if (ready)
            UpdatePlayerPosition();
    }

    public void Connect()
    {
        network.Connect(OnConnected);
    }

    private void OnConnected()
    {
        ready = true;
        network.ws.OnMessage += Ws_OnMessage;
    }

    [ContextMenu("Connect Player")]
    public void ConnectAsPlayer()
    {
        network.SendData(JsonUtility.ToJson(new ClientMessage()));
    }

    [ContextMenu("Update position")]
    public void UpdatePlayerPosition()
    {
        posMsg.playerId = playerId;
        posMsg.payload = JsonUtility.ToJson(posMod);
        network.SendData(JsonUtility.ToJson(posMsg));
    }

    [ContextMenu("Change tile")]
    public void ChangeTile()
    {
        ModifyCellMessage msg = new ModifyCellMessage(playerId, cellToModify);
        network.SendData(JsonUtility.ToJson(msg));

        Debug.Log(JsonUtility.ToJson(msg));
    }

    [ContextMenu("BoneFound")]
    public void BoneFound()
    {

    }

    [ContextMenu("Died")]
    public void Died()
    {
        PlayerDeadMessage deadMsg = new PlayerDeadMessage(playerId);
        network.SendData(JsonUtility.ToJson(deadMsg));
    }

    private void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
    {
        JObject msg = JObject.Parse(e.Data);
        JProperty prop = msg.Properties().ToList().Find(c => c.Name == "key");
        responses[prop.Value.ToString()]?.Invoke(e.Data);
    }

    private void OnHintReceived(string data)
    {
        HintMessage msg = JsonUtility.FromJson<HintMessage>(data);
        if(msg != null)
        {
            currentHint = JsonUtility.FromJson<HintModel>(msg.payload);
        }
    }

    private void OnIDAssigned(string data)
    {
        IdAssignMessage msg = JsonUtility.FromJson<IdAssignMessage>(data);
        if(msg != null)
        {
            playerId = msg.playerId;
        }
    }

    public void OnTileModified(string data)
    {
        BoardChangeMessage msg = JsonUtility.FromJson<BoardChangeMessage>(data);
        if (msg != null)
        {
            CellModel mod = JsonUtility.FromJson<CellModel>(msg.payload);
            var cell = currentMap.cells.Find(c => c.cellId == mod.cellId);
            cell.type = mod.type;
        }
    }
}
