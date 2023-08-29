using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ExamineClue : MonoBehaviour {

    public Sprite ImageClue;
    public float overTime = 3f;
    public int localizedText;
    public Image clueCanvas;
    public AudioClip spelling, positive;
    public AudioSource aus;
    public event Action OnSolve;

    private float currentTime=0;
    public GameStates asocState;    

    public GameCursor gameCursor;

    GameState gameState;

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        if (!gameState)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
    }

    void OnMouseEnter()
    {
        currentTime = Time.time;
        if (!gameState.gameStates[(int)asocState])
        {
            clueCanvas.fillAmount = 0;
            clueCanvas.sprite = ImageClue;
            aus.clip = spelling;
            aus.Play();
        }
    }

    private void OnMouseOver()
    {
        if(!gameState.gameStates[(int)asocState])
        {
            CursorManager.instance.SetCursor(gameCursor);
            clueCanvas.fillAmount = (Time.time - currentTime) / overTime;        
            if(Time.time - currentTime>=overTime)
            {
                gameState.gameStates[(int)asocState] = true;
                aus.Stop();
                aus.clip = positive;
                aus.Play();
                if (OnSolve != null)
                    OnSolve();
                DD_GameManager.GameGoalsManager gameStateManager = GameObject.FindObjectOfType<DD_GameManager.GameGoalsManager>();
                if(gameStateManager)
                    gameStateManager.RepaintItemAllToDo();
            }
        }
    }

    private void OnMouseExit()
    {
        currentTime = 0;
        if (!gameState.gameStates[(int)asocState])
        {
            CursorManager.instance.SetCursor(GameCursor.ModeNormal);
            clueCanvas.fillAmount = 0;
            aus.Stop();
        }
    }

    void OnDisable()
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
    }
}
