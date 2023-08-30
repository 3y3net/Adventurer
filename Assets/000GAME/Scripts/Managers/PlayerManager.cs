using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    void Awake()
    {
        Instance = this;
    }

    public void StopHighlight()
    {
        if (Camera.main.GetComponent<HighlightingSystem.HighlighterRenderer>() != null)
            Camera.main.GetComponent<HighlightingSystem.HighlighterRenderer>().enabled = false;
    }

    public void ResumeHighlight()
    {
        if (Camera.main.GetComponent<HighlightingSystem.HighlighterRenderer>() != null)
            Camera.main.GetComponent<HighlightingSystem.HighlighterRenderer>().enabled = true;
    }
    
}