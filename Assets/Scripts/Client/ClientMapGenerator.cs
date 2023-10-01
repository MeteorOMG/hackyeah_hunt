using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMapGenerator : MonoBehaviour
{
    public ClientCell cellBase;
    public List<ClientCell> currentCells;

    public MapModel exampleMap;

    [ContextMenu("gen")]
    public void Generate()
    {
        GenerateMap(exampleMap);
    }

    public void GenerateMap(MapModel model)
    {
        foreach (var ceil in model.cells)
        {
            ClientCell adminCell = Instantiate(cellBase, transform);
            adminCell.transform.localPosition = new Vector3(ceil.cellPosition.x * model.cellSize, 0f, ceil.cellPosition.y * model.cellSize);
            adminCell.gameObject.SetActive(true);
            adminCell.Init(ceil);
            currentCells.Add(adminCell);
        }
    }
}
