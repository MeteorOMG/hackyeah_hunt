using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class MapPlayer : MonoBehaviour
{
    public string playerId;
    public HintModel currentHint;

    public List<SpriteRenderer> rrends;

    private Vector3 targetPos;
    private Quaternion targetRot;
    public float speed;

    public void Init(Color col)
    {
        foreach (var rend in rrends)
            rend.color = col;
    }

    public void MovePlayer(PlayerPositionModel model)
    {
        //transform.localPosition = model.position;
        //transform.localRotation = Quaternion.Euler(model.rotation);

        targetPos = model.position;
        targetPos.y = 0f;
        targetRot = Quaternion.Euler(model.rotation);
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * speed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * speed);
    }
}
