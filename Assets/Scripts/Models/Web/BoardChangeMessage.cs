using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardChangeMessage
{
    public string key = "board_change";
    public string playerId;
    public string payload;

    public BoardChangeMessage(string playerId, CellModel model)
    {
        this.playerId = playerId;
        this.payload = JsonUtility.ToJson(model);
    }
}
