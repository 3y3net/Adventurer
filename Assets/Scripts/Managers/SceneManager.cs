using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
                    GameObject managers = GameObject.Find("/GameManagers");
                    if (managers == null)
                        managers = new GameObject("SceneManager");
                    _instance = (new GameObject("SceneManager")).AddComponent<SceneManager>();
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
        Menu
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
                gameMode = GameMode.ZoomArea;
                ShowCursor();
            }
            else
            {
                gameMode = GameMode.Locomotion;
                HideCursor();
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
