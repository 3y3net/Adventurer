using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ExamineClueUI : MonoBehaviour {

    public Sprite ImageClue;
    public string TextClue;
    public int localizedText;
    public float overTime = 3f;

    public Image clueCanvas;
    public AudioClip spelling, positive;
    public AudioSource aus;
    public event Action<ExamineClueUI> OnSolve;
    public bool isSolution = false;

    private float currentTime = 0;
    public bool solved = false;

    bool entered = false;

    public GameCursor gameCursor;

    void OnEnable()
    {
        if(solved)
            clueCanvas.fillAmount = 1;
        else
            clueCanvas.fillAmount = 0;
    }

    public void OnMouseEnter()
    {
        currentTime = Time.time;
        if (!solved)
        {
            clueCanvas.fillAmount = 0;
            clueCanvas.sprite = ImageClue;
            aus.clip = spelling;
            aus.Play();
            entered = true;
        }
    }

    void Update()
    {
        if (!solved && entered)
        {
            CursorManager.instance.SetCursor(gameCursor);
            clueCanvas.fillAmount = (Time.time - currentTime) / overTime;
            if (Time.time - currentTime >= overTime)
            {
                solved = true;
                aus.Stop();
                aus.clip = positive;
                aus.Play();
                if (OnSolve != null)
                    OnSolve(this);
                entered = false;
                DD_GameManager.GameGoalsManager gameStateManager = GameObject.FindObjectOfType<DD_GameManager.GameGoalsManager>();
                if (gameStateManager)
                    gameStateManager.RepaintItemAllToDo();
            }
        }
    }

    public void OnMouseExit()
    {
        currentTime = 0;
        if (!solved)
        {
            CursorManager.instance.SetCursor(GameCursor.ModeNormal);
            clueCanvas.fillAmount = 0;
            aus.Stop();
            entered = false;
        }
    }

    void OnDisable()
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
    }
}
