using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameClue
{
    public string description;
    public GameStates asocState;
    public int textClueIdx;
    public int textDoneIdx;    
}

[System.Serializable]
public class GameGoal {
    public string description;
    public GameStates asocState;
    public bool active;
    public int titleIdx, descIniIdx, descEndIdx;
    public Sprite iconToDo;
    public Sprite iconDone;
    public int conclusionIdx;
    public GameClue[] clues = new GameClue[0];
}
