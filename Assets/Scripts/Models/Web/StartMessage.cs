using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartMessage
{
    public string key = "start";
    public string payload;

    public StartMessage(MapModel map)
    {
        payload = JsonUtility.ToJson(map);
    }
}
