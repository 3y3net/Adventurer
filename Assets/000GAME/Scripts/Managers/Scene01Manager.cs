using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Scene01Manager : MonoBehaviour {

    /*
    public GameObject vCam, cardBoxClosed, cardBoxOpened, exitCollider;
    public InteractionObject AssadBox;
    public Transform finalPos;
    public PlayerMovement Player;
    public AudioSource audioSource;
    public AudioClip addItem;
    */

    public ReactionList FreshStartReactionList;
    public ReactionList LoadedStartReactionList;

    [TextArea]
    public string defaultInitialText;

    private GameState gameState;
    private SceneController sceneController;
    private TimeLineManager timelineManager;
    private DD_GameManager.GameGoalsManager goalsManager;

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        if (!gameState)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
        
        sceneController = FindObjectOfType<SceneController>();
        if (!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");

        timelineManager = FindObjectOfType<TimeLineManager>();
        if (!timelineManager)
            throw new UnityException("TimeLineManager could not be found, ensure that it exists in the Persistent scene.");

        goalsManager = FindObjectOfType<DD_GameManager.GameGoalsManager>();
        if (!goalsManager)
            throw new UnityException("GameGoalsManager could not be found, ensure that it exists in the Persistent scene.");

        if (!gameState.gameStates[(int)GameStates.Intro1Played])
        {
            gameState.gameStates[(int)GameStates.Intro1Played] = true;
            timelineManager.PlayClip(0, FinishPlay);
            sceneController.fadeText.text = "";
            gameState.gameValues[(int)GameValues.StartingTextIndex] = 2;            
        }
        else
        {
            LoadedStartReactionList.React();
            //vCam.SetActive(true);
            //cardBoxClosed.SetActive(true);
            //Player.ForcePosition(finalPos);
        }
        /*
        if(gameState.gameStates[(int)GameStates.AssadBoxOpened])
        {
            AssadBox.HighLightObject = cardBoxOpened;
        }
        */
    }

    void FinishPlay(int index)
    {
        switch(index)
        {
            case 0:
                FreshStartReactionList.React();
                //exitCollider.SetActive(true);
                //vCam.SetActive(true);
                //cardBoxClosed.SetActive(true);                
                //goalsManager.AddGoalToDo(0);
                //Player.ForcePosition(finalPos);
                break;
        }
    }
}
