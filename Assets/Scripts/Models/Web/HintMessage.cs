using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HintMessage
{
    public string key = "hint";
    public string playerId;
    public string payload;

    public HintMessage(string playerId, HintModel model)
    {
        this.playerId = playerId;
        this.payload = JsonUtility.ToJson(model);
    }
}
