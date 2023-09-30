using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminCell : MonoBehaviour
{
    public CellModel model;
    public FieldDefinition definition;
    public Renderer rend;

    public void Init(CellModel model, FieldDefinition definition)
    {
        this.model = model;
        rend.material = Instantiate(rend.material);
        OverrideDefinition(definition);
    }

    public void OverrideDefinition(FieldDefinition definition)
    {
        this.definition = definition;
        rend.sharedMaterial.color = definition.color;
    }
}
