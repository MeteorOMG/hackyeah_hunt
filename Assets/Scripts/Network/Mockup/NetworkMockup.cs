using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMockup : MonoBehaviour
{
    [Header("Map")]
    public MapController map;

    [Header("PlayerPos")]
    public PlayerPositionModel position;
    public PlayerPosMessage moveMsg;

    [Header("NewPlayer")]
    public NewPlayerMessage newMsg;

    [Header("Cell Mod")]
    public ModifyCellMessage cellMsg;
    public CellModel cellMod;

    [ContextMenu("FakeMove")]
    public void FakePlayerMove()
    {
        moveMsg.payload = JsonUtility.ToJson(position);
        map.OnPlayerMoved(JsonUtility.ToJson(moveMsg));
    }

    [ContextMenu("NewPlayer")]
    public void FakeNewPlayer()
    {
        map.OnPlayerEntered(JsonUtility.ToJson(newMsg));
    }

    [ContextMenu("Cell changed")]
    public void ChangeCell()
    {
        cellMsg.payload = JsonUtility.ToJson(cellMod);
        map.OnTileModified(JsonUtility.ToJson(cellMsg));
    }
}
