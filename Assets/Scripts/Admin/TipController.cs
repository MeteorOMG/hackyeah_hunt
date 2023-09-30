using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipController : MonoBehaviour
{
    public NetworkController sender;
    public Camera cam;

    public MapPlayer selectedPlayer;
    public Vector2 currentDirection;
    public string currentSteps;

    [Header("UI")]
    public Button moveUp;
    public Button moveDown;
    public Button moveLeft;
    public Button moveRight;
    public TMP_InputField steps;

    public TextMeshPro current;
    public Transform arrow;
    public float arrowDist;

    private void Start()
    {
        moveUp.onClick.AddListener(() => { currentDirection = Vector2.up; SendHintUpdate(); });
        moveDown.onClick.AddListener(() => { currentDirection = Vector2.down; SendHintUpdate(); });
        moveLeft.onClick.AddListener(() => { currentDirection = Vector2.left; SendHintUpdate(); });
        moveRight.onClick.AddListener(() => { currentDirection = Vector2.right; SendHintUpdate(); });
        steps.onSubmit.AddListener((newSteps) => { currentSteps = newSteps; SendHintUpdate(); });
    }

    private void Update()
    {
        //Player selection
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                MapPlayer player = hit.collider.GetComponent<MapPlayer>();
                selectedPlayer = player;
            }
        }
        
        UpdateVis();
    }

    public void UpdateVis()
    {
        if(selectedPlayer == null)
        {
            current.transform.position = Vector3.down * 100;
            arrow.transform.position = Vector3.down * 100;
            return;
        }

        current.transform.position = selectedPlayer.transform.position + Vector3.up;
        current.text = selectedPlayer.currentHint.steps;
        var dir = selectedPlayer.currentHint.direction;
        arrow.transform.position = selectedPlayer.transform.position + (new Vector3(dir.x, 0f, dir.y) * arrowDist);
    }

    public void SendHintUpdate()
    {
        if (selectedPlayer == null) 
            return;

        selectedPlayer.currentHint = GetModel();
        sender.SendData(JsonUtility.ToJson(new HintMessage(selectedPlayer.playerId, GetModel())));
    }

    private HintModel GetModel()
    {
        return new HintModel(currentDirection, currentSteps, string.Empty);
    }
}
