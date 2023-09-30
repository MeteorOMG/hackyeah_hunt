using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPosMessage
{
    public string key = "move";
    public string playerId;
    public string payload;

    public PlayerPosMessage(string playerId, PlayerPositionModel model)
    {
        this.playerId = playerId;
        this.payload = JsonUtility.ToJson(model);
    }
}
