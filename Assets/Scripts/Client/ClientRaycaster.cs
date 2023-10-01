using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClientRaycaster : MonoBehaviour
{
    public float rayDist;
    public GameObject rayVis;
    public ClientMapGenerator map;

    public ClientCell currentCell;

    private void Update()
    {
        foreach (var cell in map.currentCells)
        {
            cell.OnHighlightEnded();
        }

        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, rayDist))
        {
            rayVis.transform.position = hit.point;
            currentCell = hit.transform.GetComponent<ClientCell>();
            if (currentCell != null)
                currentCell.OnHighlight();
        }
    }

    public void TryDig(UnityAction OnBone, UnityAction OnDeath, UnityAction OnNothing)
    {
        switch (currentCell.model.type)
        {
            case 1:
                OnBone?.Invoke();
                break;

            case 0:
                OnDeath?.Invoke();
                break;
        }

        OnNothing?.Invoke();
    }
}
