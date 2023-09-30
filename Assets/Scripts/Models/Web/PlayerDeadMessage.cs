using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDeadMessage
{
    public string key = "playerLeave";
    public string playerId;

    public PlayerDeadMessage(string playerId)
    {
        this.playerId = playerId;
    }
}
