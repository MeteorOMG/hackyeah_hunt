using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public MapModel model;

    public MessageModel mainMesasge;

    [ContextMenu("show Map model")]
    public void Show()
    {
        Debug.Log(JsonUtility.ToJson(model));
    }

    [ContextMenu("show main")]
    public void ShowMain()
    {
        Debug.Log(JsonUtility.ToJson(mainMesasge));
    }
}
