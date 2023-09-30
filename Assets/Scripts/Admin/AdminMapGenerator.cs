using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminMapGenerator : MonoBehaviour
{
    [Header("Definitions")]
    public List<FieldDefinition> fieldDefinitions;

    [Header("Cells")]
    public AdminCell cellBase;
    public List<AdminCell> currentCells;

    [Header("Generation")]
    public int bones;

    [Header("Targe")]
    public MapModel mapModel;

    [ContextMenu("Gen Map")]
    public void Generate()
    {
        Clear();
        GenerateMap(mapModel);
    }

    #region PhysicalMap
    public void GenerateMap(MapModel model)
    {
        foreach(var ceil in model.cells)
        {
            AdminCell adminCell = Instantiate(cellBase, transform);
            adminCell.transform.localPosition = new Vector3(ceil.cellPosition.x * model.cellSize, 0f, ceil.cellPosition.y * model.cellSize);
            adminCell.gameObject.SetActive(true);
            adminCell.Init(ceil, fieldDefinitions.Find(c => c.type == ceil.type));
            currentCells.Add(adminCell);
        }
    }

    private void Clear()
    {
        for (int i = currentCells.Count - 1; i >= 0; i--)
            DestroyImmediate(currentCells[i].gameObject);

        currentCells.Clear();
    }
    #endregion

    #region Randomize
    [ContextMenu("Generate Model")]
    public void GenerateModel()
    {
        mapModel.cells.Clear();

        for(int i = 0; i < mapModel.mapWidth; i++)
        {
            for(int j = 0; j < mapModel.mapHeight; j++)
            {
                CellModel model = new CellModel($"{i}{j}", new Vector2(i, j), 0);
                mapModel.cells.Add(model);
            }
        }

        AddBones();
    }

    public void AddBones()
    {
        if(bones >= mapModel.cells.Count)
        {
            foreach (var cell in currentCells)
            {
                cell.OverrideDefinition(fieldDefinitions[0]);
                cell.model.type = fieldDefinitions[0].type;
            }

            return;
        }

        for(int i = 0; i < bones; i++)
        { 
            var availableCells = mapModel.cells.FindAll(c => c.type != fieldDefinitions[0].type);
            int randomCell = UnityEngine.Random.Range(0, availableCells.Count - 1);
            var cell = availableCells[randomCell];
            cell.type = fieldDefinitions[0].type;
        }
    }
    #endregion
}

[Serializable]
public class FieldDefinition
{
    public int type;
    public Color color;
}
