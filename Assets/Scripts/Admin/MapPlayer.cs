using UnityEngine;
using System;

[System.Serializable]
public class MapPlayer : MonoBehaviour
{
    public string playerId;
    public HintModel currentHint;

    public void MovePlayer(PlayerPositionModel model)
    {
        transform.localPosition = model.position;
        transform.localRotation = Quaternion.Euler(model.rotation);
    }
}
