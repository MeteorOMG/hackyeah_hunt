using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifyCellMessage
{
    public string key = "board_change";
    public string playerId;
    public string payload;

    public ModifyCellMessage(string playerId, CellModel model)
    {
        this.playerId = playerId;
        this.payload = JsonUtility.ToJson(model);
    }
}
