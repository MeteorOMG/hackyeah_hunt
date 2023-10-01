using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientTip : MonoBehaviour
{
    public Transform player;
    public Transform mapHeight;
    public Transform arrow;
    public TextMeshPro tip;
    public Transform tipTransform;
    public float distance;

    public HintModel currentHint;

    public void SetTip()
    {

    }

    public void Update()
    {
        transform.position = new Vector3(player.position.x, mapHeight.position.y, player.position.z);
        UpdateTip();
        tip.text = currentHint.steps;
        tipTransform.transform.rotation = Quaternion.Euler(new Vector3(0f, player.transform.rotation.eulerAngles.y, 0f));
    }

    private void UpdateTip()
    {
        Vector3 translated = new Vector3(currentHint.direction.x, 0f, currentHint.direction.y);
        arrow.transform.localPosition = distance * translated;
        arrow.transform.localRotation = Quaternion.LookRotation(arrow.transform.localPosition);
    }
}
