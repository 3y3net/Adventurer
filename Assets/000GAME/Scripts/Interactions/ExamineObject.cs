using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineObject : MonoBehaviour {

    [Tooltip("Speed of Horizontal movement when rotating an object")]
    [SerializeField] private float horizontalSpeed = 5.0F;

    [Tooltip("Speed of Vertical movement when rotating an object")]
    [SerializeField] private float verticalSpeed = 5.0F;

    public List<ExamineClue> clues = new List<ExamineClue>();
    public Text clueText;
    public GameObject clueCanvas, backButton;
    public ReactionList reactionList;

    public string okText, pendingText;

    private Vector3 startPos;
    private bool doOnce;

    GameState gameState;

    public void OnSolve()
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
        DrawClueText();
        bool solved = true;
        for (int i = 0; i < clues.Count; i++)
            if (!gameState.gameStates[(int)clues[i].asocState])
                solved = false;
        if (solved)
            reactionList.React();
    }

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        if (!gameState)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");

        startPos = this.gameObject.transform.localEulerAngles;
        doOnce = false;
        DrawClueText();
        for (int i = 0; i < clues.Count; i++)
            clues[i].OnSolve+=OnSolve;

    }

    public void DrawClueText()
    {
        gameState = FindObjectOfType<GameState>();
        if (!gameState)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
        clueText.text = "";
        for (int i = 0; i < clues.Count; i++)
            if(gameState.gameStates[(int)clues[i].asocState])
                clueText.text += okText + " " + LocalizableData.instance.languageText[clues[i].localizedText] + "\r\n\r\n";
            else
                clueText.text += pendingText + " " + LocalizableData.instance.languageText[29] + "\r\n\r\n";

    }

    void OnEnable()
    {
        DrawClueText();
        PlayerMovement pm= FindObjectOfType<PlayerMovement>();
        if (pm)
        {
            pm.StopAll();
        }
        backButton.SetActive(true);
        backButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        backButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { DisableMe(); });
        clueCanvas.SetActive(true);
        
        clueCanvas.transform.Find("ClueCanvas").GetComponent<Image>().fillAmount=0;

        /*
         * UnityStandardAssets.ImageEffects.BlurOptimized mainBlur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        if (mainBlur != null)
            mainBlur.enabled = true;
        */

        HighlightingSystem.Demo.RaycastController rc= Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>();
        if (rc != null)
            rc.enabled = false;
    }

    public void DisableMe()
    {
        InventoryInspect inventoryInspect = FindObjectOfType<InventoryInspect>();
        if (inventoryInspect != null)
            inventoryInspect.CloseCam();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm)
        {
            pm.ResumeAll();
        }
        backButton.SetActive(false);
        clueCanvas.SetActive(false);
        /*
        UnityStandardAssets.ImageEffects.BlurOptimized mainBlur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        if (mainBlur != null)
            mainBlur.enabled = false;
        */
        HighlightingSystem.Demo.RaycastController rc = Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>();
        if (rc != null)
            rc.enabled = true;
    }

    void Update()
    {
        if (this.gameObject.activeSelf && !doOnce)
        {
            doOnce = true;

            if (doOnce)
            {
                this.gameObject.transform.localEulerAngles = startPos;
            }
        }

        float h = horizontalSpeed * Input.GetAxis("Mouse X"); //This is the rotation control for mouse input, you can change this if you need another input style
        float v = verticalSpeed * Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(0))
        {
            this.gameObject.transform.Rotate(v, -h, 0, Space.World); //On left click held down we perform the rotation of the object                        
            //this.gameObject.transform.Rotate(v, 0, 0); //On left click held down we perform the rotation of the object                        
            //horizontal.transform.Rotate(new Vector3(0,1,0), -h); 
        }

        else if (Input.GetMouseButtonDown(1)) //When we right click we enable all player functionality once again
        {
            doOnce = false;
            this.gameObject.SetActive(false);
        }
    }
}
