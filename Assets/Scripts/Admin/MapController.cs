using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public NetworkController network;
    public AdminMapGenerator generator;
    public List<MapPlayer> players;
    public MapPlayer playerBase;

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
    }

    public void OnPlayerEntered(string data)
    {
        NewPlayerMessage msg = JsonUtility.FromJson<NewPlayerMessage>(data);
        if(msg != null)
        {
            MapPlayer newPlayer = Instantiate(playerBase, transform);
            newPlayer.playerId = msg.playerId;
            newPlayer.gameObject.SetActive(true);
            players.Add(newPlayer);
        }
    }

    public void OnPlayerExit(string data)
    {
        PlayerDeadMessage msg = JsonUtility.FromJson<PlayerDeadMessage>(data);
        if (msg != null)
        {
            MapPlayer player = players.Find(c => c.playerId == msg.playerId);
            if(player != null)
            {
                players.Remove(player);
                GameObject.Destroy(player);
            }
        }
    }

    public void OnPlayerMoved(string data)
    {
        PlayerPosMessage msg = JsonUtility.FromJson<PlayerPosMessage>(data);
        if(msg != null)
        {
            var player = players.Find(c => c.playerId == msg.playerId);
            player.MovePlayer(JsonUtility.FromJson<PlayerPositionModel>(msg.payload));
        }
    }

    public void OnTileModified(string data)
    {
        BoardChangeMessage msg = JsonUtility.FromJson<BoardChangeMessage>(data);
        if(msg != null)
        {
            CellModel mod = JsonUtility.FromJson<CellModel>(msg.payload);
            var cell = generator.currentCells.Find(c => c.model.cellId == mod.cellId);
            if(cell != null)
            {
                cell.model.type = mod.type;
                cell.OverrideDefinition(generator.fieldDefinitions.Find(c => c.type == mod.type));
            }
        }
    }
}
