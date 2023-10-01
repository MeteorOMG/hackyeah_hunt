using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMapGenerator : MonoBehaviour
{
    public ClientCell cellBase;
    public List<ClientCell> currentCells;

    public MapModel exampleMap;
    private MapModel mapToGenerate;

    [ContextMenu("gen")]
    public void Generate()
    {
        //GenerateMap(exampleMap);
    }

    private void Update()
    {
        if(mapToGenerate != null)
        {
            MakeMap();
            mapToGenerate = null;
        }
    }

    public void GenerateMap(MapModel model)
    {
        this.exampleMap = model;
        this.mapToGenerate = model;
    }

    private void MakeMap()
    {
        foreach (var ceil in mapToGenerate.cells)
        {
            ClientCell adminCell = Instantiate(cellBase, transform);
            adminCell.transform.localPosition = new Vector3(ceil.cellPosition.x * mapToGenerate.cellSize, 0f, ceil.cellPosition.y * mapToGenerate.cellSize);
            adminCell.gameObject.SetActive(true);
            adminCell.Init(ceil);
            currentCells.Add(adminCell);
        }

        mapToGenerate = null;
    }
}
