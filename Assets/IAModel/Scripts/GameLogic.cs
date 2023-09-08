using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public bool paired;
    public Transform objectPaired;
    public AudioSource FX, Music;
    public Transform estrellitas;
    public Transform player, Enemy;
    public List<Transform> respawnPoints;

    public List<AI_3y3net.PatrolPoint> patrolPoints;

    public Text hits, deads;

    public AudioClip incomingAudio;
    public GameObject MissionBox, MissionText;

    //Change damage, hitforce, drop probabilitie etc....
    public float LevelMultiplicator = 1f;

    public static GameLogic instance;
    public bool miniGame1 = false;
    public float minigameSpawnTime = 1.5f;
    public float minigameSpawnRand = 0.5f;
    public float enemyLimit = 100;
    float elapsed = 0f;
    float when = 5;
    int counter = 0;

    int vhits = 0;
    int vdeads = 0;

    public GameObject pausePanel, confirmPanel, deadPanel;
    bool gamePaused = false;

    public bool doMissionStart = false;
    public float maxTimePanel=10;
    float timeCounter;

    public enum MenuSelection {None, MainMenu, Quit}
    MenuSelection menuSelection;

    public Image InventorySlot;
    public Text interactText;

    public List<DropInstance.DropItem> inventoryObject=new List<DropInstance.DropItem>();
    public LevelPuzzle levelPuzzle;

    private void Awake()
    {
        instance = this;
        when = minigameSpawnTime + Random.Range(-minigameSpawnTime, minigameSpawnTime);
    }

    // Use this for initialization
    void Start() {
        timeCounter = maxTimePanel;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (doMissionStart)
            PlayMissionIntro();
    }

    public void SetInventorySlot(Sprite spr, DropInstance.DropItem item)
    {
        inventoryObject.Add(item);
        InventorySlot.gameObject.SetActive(true);
        InventorySlot.sprite = spr;
    }

    public void UnSetInventorySlot()
    {
        InventorySlot.gameObject.SetActive(false);
        InventorySlot.sprite = null;
    }

    public void ShowInteract(string text=null)
    {
        if (text == null)
            interactText.text = "'E' to Interact";
        else
            interactText.text = text;
        interactText.gameObject.SetActive(true);
    }

    public void HideInteract()
    {
        interactText.gameObject.SetActive(false);        
    }

    public void Interact(int interactionKey)
    {
        levelPuzzle.Interact(interactionKey);
    }

    public void RestartCurrentScene()
    {
        //int scene = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void PlayerIsDead()
    {
        deadPanel.SetActive(true);
        pausePanel.SetActive(false);
        MissionBox.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PauseGame()
    {
        if (deadPanel.activeSelf)
            return;

        gamePaused = true;
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void ContinueGame()
    {
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        confirmPanel.SetActive(false);
        Cursor.visible = false;
        //enable the scripts again
    }

    public void MainMenu(bool force)
    {
        if (force)
        {
            //SceneManager.LoadScene(0, LoadSceneMode.Single);
            return;
        }

        menuSelection = MenuSelection.MainMenu;
        pausePanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void QuitApp(bool force)
    {
        if(force)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
            return;
        }

        menuSelection = MenuSelection.Quit;
        pausePanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void ConfirmSelection()
    {
        switch(menuSelection)
        {
            case MenuSelection.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
                break;
            case MenuSelection.MainMenu:
                //SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;
        }
    }

    public AI_3y3net.PatrolPoint GetPatrolPoint()
    {
        return patrolPoints[Random.Range(0, patrolPoints.Count)];
    }

    public void PlayMissionIntro()
    {
        MissionBox.SetActive(true);
        FX.clip = incomingAudio;
        FX.Play();
    }

    // Update is called once per frame
    void Update() {
        if (timeCounter > 0)
            timeCounter -= Time.deltaTime;

        else if (MissionBox.activeSelf)
            MissionBox.SetActive(false);

        if (paired && objectPaired != null)
        {
            transform.position = objectPaired.transform.position;
            transform.rotation = objectPaired.transform.rotation;
        }

        /*
        if(Input.anyKeyDown && gamePaused)
        {
            ContinueGame();
        }
        */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (doMissionStart)
            {
                MissionBox.SetActive(false);
                doMissionStart = false;
                MissionText.SetActive(true);
                timeCounter = maxTimePanel;
            }
            else if (pausePanel.activeSelf)
                ContinueGame();
            else
                PauseGame();            
        }

        
        if (miniGame1)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= when && counter < enemyLimit)
            {
                elapsed = elapsed % 1f;
                Transform enemy = GameObject.Instantiate(Enemy, respawnPoints[Random.Range(0, respawnPoints.Count)].position, respawnPoints[Random.Range(0, respawnPoints.Count)].rotation);
                enemy.gameObject.SetActive(true);
                enemy.parent = null;
                when = minigameSpawnTime + Random.Range(-minigameSpawnTime, minigameSpawnTime);
                enemy.GetComponent<AI_3y3net.StateController>().CallBackFunction = MyCallbackEventHandler;
                counter++;
            }
            if (!Music.isPlaying)
            {
                //player.GetComponent<weaponselector>().EndMinigame();
            }
            
        }

    }

    public void MyCallbackEventHandler(AI_3y3net.StateController.EventInfo eventInfo)
    {
        switch (eventInfo.messageInfo)
        {
            case AI_3y3net.StateController.EventType.Hit:
                vhits++;
                hits.text = "Hits: " + vhits;
                break;

            case AI_3y3net.StateController.EventType.Dead:
                vdeads++;
                deads.text = "Deads: " + vdeads;
                break;

        }
    }

}