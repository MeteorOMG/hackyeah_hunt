using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
    public Button digBtn;
    public ClientRaycaster raycaster;
    public NetworkController network;
    public ClientMapGenerator clientMap;
    public ClientMockup mock;

    private void Start()
    {
        digBtn.onClick.AddListener(TryDig);
    }

    public void TryDig()
    {
        if(raycaster.currentCell != null)
        {
            Debug.Log("Cell exists");
            Vector3 height = raycaster.currentCell.transform.position;
            Vector3 raycasterPoss = raycaster.transform.position;
            raycasterPoss.y = height.y;

            float distance = Vector3.Distance(raycasterPoss, raycaster.currentCell.transform.position);
            Debug.Log(distance);
            if (distance < clientMap.exampleMap.cellSize)
            {
                raycaster.TryDig(OnBone, OnDied, OnDied);
            }
        }
    }

    public void OnBone()
    {
        Debug.Log("Bone");
        var cell = raycaster.currentCell;
        cell.model.type = 0;
        ModifyCellMessage msg = new ModifyCellMessage(mock.playerId, cell.model);
        network.SendData(JsonUtility.ToJson(msg));
    }

    public void OnDied()
    {
        Debug.Log("Died");
        PlayerDeadMessage msg = new PlayerDeadMessage(mock.playerId);
        network.SendData(JsonUtility.ToJson(msg));
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
