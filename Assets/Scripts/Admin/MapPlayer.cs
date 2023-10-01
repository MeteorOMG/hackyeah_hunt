using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class MapPlayer : MonoBehaviour
{
    public string playerId;
    public HintModel currentHint;

    public List<SpriteRenderer> rrends;

    public void Init(Color col)
    {
        foreach (var rend in rrends)
            rend.color = col;
    }

    public void MovePlayer(PlayerPositionModel model)
    {
        transform.localPosition = model.position;
        transform.localRotation = Quaternion.Euler(model.rotation);
    }
}
