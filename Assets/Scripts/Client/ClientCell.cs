using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCell : MonoBehaviour
{
    public CellModel model;
    public SpriteRenderer spriteRend;

    public Color highColor;
    public Color defCol;
    public float speed;

    public bool taken;

    public void Init(CellModel model)
    {
        this.model = model;
    }

    private void Update()
    {
        spriteRend.color = Color.Lerp(spriteRend.color, taken ? highColor : defCol, Time.deltaTime * speed);
    }

    public void OnHighlight()
    {
        taken = true;
    }

    public void OnHighlightEnded()
    {
        taken = false;
    }
}
