using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineManager : MonoBehaviour {

    public PlayableDirector director;
    public List<TimelineAsset> TimelineClips = new List<TimelineAsset>();
    //public static TimeLineManager Instance;
    public HighlightingSystem.Demo.RaycastController highlightCamera;

    public PlayerMovement Player;
    public bool escFinish = false;
    public bool useFade = false;

    public delegate void FinishPLay(int index);
    protected FinishPLay callbackFct;

    public bool playing;
    private int idx;
    private SceneController sceneController;

    void Awake()
    {
        idx = -1;

        sceneController = FindObjectOfType<SceneController>();

        // If the SceneController couldn't be found throw an exception so it can be added.
        if (!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
    }

    public void PlayClip(int index, FinishPLay callBack=null)
    {
        if (playing)
            return;

        playing = true;
        callbackFct = callBack;
        director.playableAsset = TimelineClips[0];
        director.Play();
        idx = index;
        Player.handleInput = false;
        highlightCamera.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!playing)
            return;
        //Hay fade y llegamos al tiempo limite para aplicarlo
        if(director.duration-director.time<= sceneController.fadeDuration && useFade)
        {
            playing = false;
            StartCoroutine(sceneController.FadeAndCallBack(DelayedEnd));
        }
        //No hay fade y acaba el director
        else if(director.state!=PlayState.Playing)
        {
            playing = false;                  
            if (callbackFct != null)
                callbackFct(idx);
            idx = -1;
            Player.handleInput = true;            
        }
        else if(director.state == PlayState.Playing && director.duration - director.time > sceneController.fadeDuration && director.time>1f)
        {
            //Da esc en medio de la reproducción
            if(Input.GetKeyDown(KeyCode.Escape) && escFinish)
            {
                Debug.Log("ESCAPED!!");
                if (useFade)
                {
                    Debug.Log("Go fade");
                    playing = false;
                    StartCoroutine(sceneController.FadeAndCallBack(DelayedEnd));
                }
                else
                {
                    director.Stop();
                }
                
            }
        }
	}

    void DelayedEnd()
    {
        Debug.Log("Stopping director");
        director.Stop();
        if (callbackFct != null)
            callbackFct(idx);
        idx = -1;
        Player.handleInput = true;
        highlightCamera.enabled = true;
    }
}
