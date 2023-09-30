using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class AdminCell : MonoBehaviour
{
    public CellModel model;
    public FieldDefinition definition;
    public SpriteRenderer spriteRend;

    public GameObject boneIcon;
    public Sprite takenSprite;
    public Color takenColor;
    public Sprite freeSprite;
    public Color freeColor;

    public bool taken;

    public void Init(CellModel model, FieldDefinition definition)
    {
        this.model = model;
        OverrideDefinition(definition);
    }

    public void OverrideDefinition(FieldDefinition definition)
    {
        this.definition = definition;
        boneIcon.SetActive(definition.type == 1);
    }

    public void PlayerEnter()
    {
        taken = true;
        spriteRend.sprite = takenSprite;
        spriteRend.color = takenColor;
    }

    public void PlayerExit()
    {
        taken = false;
        spriteRend.sprite = freeSprite;
        spriteRend.color = freeColor;
    }
}
