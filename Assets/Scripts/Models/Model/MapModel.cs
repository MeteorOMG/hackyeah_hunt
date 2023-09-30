using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapModel
{
    public int cellSize;
    public int mapWidth;
    public int mapHeight;
    public List<CellModel> cells;
}

[System.Serializable]
public class CellModel
{
    public string cellId;
    public Vector2 cellPosition;
    public int type;

    public CellModel(string id, Vector2 pos, int type)
    {
        this.cellId = id;
        this.cellPosition = pos;
        this.type = type;
    }
}