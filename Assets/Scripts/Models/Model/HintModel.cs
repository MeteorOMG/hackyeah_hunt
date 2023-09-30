using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintModel
{
    public Vector2 direction;
    public string steps;
    public string extra;

    public HintModel(Vector2 direction, string steps, string extra)
    {
        this.direction = direction;
        this.steps = steps;
        this.extra = extra;
    }
}
