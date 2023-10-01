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
    public RectTransform uiRect;
    public Button moveUp;
    public Button moveDown;
    public Button moveLeft;
    public Button moveRight;
    public TMP_InputField steps;
    private Dictionary<Vector2, Button> buttonsForDict = new Dictionary<Vector2, Button>();
    public Color selectedColor;
    public Color notSelectedColor;

    private void Awake()
    {
        buttonsForDict.Add(Vector2.up, moveUp);
        buttonsForDict.Add(Vector2.down, moveDown);
        buttonsForDict.Add(Vector2.left, moveLeft);
        buttonsForDict.Add(Vector2.right, moveRight);
    }

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
            } else
            {
                selectedPlayer = null;
            }
        }
        
        UpdateVis();
    }

    public void UpdateVis()
    {
        if (selectedPlayer == null)
        {
            uiRect.position = new Vector3(1000, 1000f, 0f);
            return;
        }
            
        
        uiRect.position = cam.WorldToScreenPoint(selectedPlayer.transform.position);
        foreach(var btn in buttonsForDict)
        {
            btn.Value.image.color = currentDirection == btn.Key ? selectedColor : notSelectedColor;
        }
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
