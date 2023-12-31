using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance = null;
    public static SceneManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SceneManager)FindObjectOfType(typeof(SceneManager));
                if (_instance == null)
                {
                    GameObject managers = GameObject.Find("/Managers");
                    if (managers == null)
                        managers = new GameObject("GSceneManager");
                    _instance = (new GameObject("GSceneManager")).AddComponent<SceneManager>();
                    _instance.transform.parent = managers.transform;
                }
            }
            return _instance;
        }
    }

    public enum GameMode
	{
        Locomotion,
        ZoomArea,
        Inspect,
        Menu,
        Tablet
	}

    public enum GameCursor
    {
        ModeNormal,
        ModeInfo,
        ModeAction,
        ModeGrab,
        ModeFork
    }

    public GameMode gameMode;
    public GameCursor currentGameCursor = GameCursor.ModeNormal;
    public GameObject player;

	private void Start()
	{
        if (Application.isPlaying)
            HideCursor();
	}

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.T)) {
            if (gameMode == GameMode.Locomotion)
            {
                gameMode = GameMode.Tablet;
                ShowCursor();
                DD_GameManager.GameUIManager.instance.ShowInterface();
            }
            else
            {
                gameMode = GameMode.Locomotion;
                HideCursor();
                DD_GameManager.GameUIManager.instance.HideInterface();
            }
		}

    }

    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;
        switch (gameMode)
        {
            case GameMode.Locomotion:
                HideCursor();
                break;
            case GameMode.ZoomArea:
                ShowCursor();
                break;
        }
    }

    public void ShowCursor()
    {
        Cursor.visible = true;        
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
