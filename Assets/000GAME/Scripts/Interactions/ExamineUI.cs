using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ExamineUI : MonoBehaviour {    

    public List<ExamineClueUI> clues = new List<ExamineClueUI>();
    public Text clueText;
    public GameObject clueCanvas, backButton;
    public GameConditionList[] conditionList;
    public ReactionList reactionList;

    public ReactionList exitInterface;

    public string okText, pendingText;

    
    public void OnSolve(ExamineClueUI clue)
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
        if (clue.isSolution)
            DrawClueText();
        else
        {
            TextManager textManager = FindObjectOfType<TextManager>();
            if (textManager)
                textManager.DisplayMessage(LocalizableData.instance.languageText[clue.localizedText], Color.white, 0);
        }

        bool solved = true;
        for (int i = 0; i < clues.Count; i++)
            if (!clues[i].solved && clues[i].isSolution)
                solved = false;
        if (solved)
        {
            for (int i = 0; i < conditionList.Length; i++)
            {
                // ... then check and potentially react to each.  If the reaction happens, exit the function.
                if (conditionList[i].CheckAndReact())
                    return;
            }
            reactionList.React();
        }
    }

    void Start()
    {
        DrawClueText();
        for (int i = 0; i < clues.Count; i++)
            clues[i].OnSolve += OnSolve;

    }

    public void DrawClueText()
    {
        clueText.text = "";
        for (int i = 0; i < clues.Count; i++)
            if (clues[i].solved && clues[i].isSolution)
                clueText.text += okText + " " + LocalizableData.instance.languageText[clues[i].localizedText] + "\r\n\r\n";
            else if (clues[i].isSolution)
                clueText.text += pendingText + " " + LocalizableData.instance.languageText[29] + "\r\n\r\n";

    }

    void OnEnable()
    {
        DrawClueText();
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm)
        {
            pm.StopAll();
        }
        backButton.SetActive(true);
        backButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        backButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { DisableMe(); });
        clueCanvas.SetActive(true);

        clueCanvas.transform.Find("ClueCanvas").GetComponent<Image>().fillAmount = 0;

        /*
        UnityStandardAssets.ImageEffects.BlurOptimized mainBlur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        if (mainBlur != null)
            mainBlur.enabled = true;
        */
        HighlightingSystem.Demo.RaycastController rc = Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>();
        if (rc != null)
            rc.enabled = false;
    }

    public void DisableMe()
    {
        //Debug.Log("DisableMe");
        if (exitInterface!=null)
        {
            //Debug.Log("DisableMe.exitInterface");
            exitInterface.React();
        }
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
}
