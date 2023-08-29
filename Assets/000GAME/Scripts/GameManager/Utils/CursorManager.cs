using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameCursor
{
    ModeNormal,
    ModeInfo,
    ModeAction,
    ModeGrab,
    ModeGlass,
    ModeFork
}

public class CursorManager : MonoBehaviour {

    public GameCursor currentGameCursor = GameCursor.ModeNormal;
    public GameObject GameCursors;

    private GameObject ModeGear, ModeInfo, ModeGrab, ModeGlass;

    public static CursorManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (currentGameCursor != GameCursor.ModeNormal)
            GameCursors.transform.position = Input.mousePosition + new Vector3(-32, -32, 0); //mouseCursor.transform.position+ new Vector3(-32, -32, 0); 
    }

    void Start()
    {
        if (ModeInfo == null)
            ModeInfo = GameCursors.transform.Find("Help").gameObject;
        if (ModeGear == null)
            ModeGear = GameCursors.transform.Find("Gear").gameObject;
        if (ModeGrab == null)
            ModeGrab = GameCursors.transform.Find("Grab").gameObject;
        if (ModeGlass == null)
            ModeGlass = GameCursors.transform.Find("Glass").gameObject;
    }

    public void SetCursor(GameCursor gameCursor)
    {

        if (currentGameCursor == gameCursor)
            return;

        currentGameCursor = gameCursor;        

        switch (gameCursor)
        {
            case GameCursor.ModeNormal:
                ModeGear.SetActive(false);
                ModeInfo.SetActive(false);
                ModeGrab.SetActive(false);
                ModeGlass.SetActive(false);
                break;
            case GameCursor.ModeInfo:
                ModeGear.SetActive(false);
                ModeGrab.SetActive(false);
                ModeInfo.SetActive(true);
                ModeGlass.SetActive(false);
                break;
            case GameCursor.ModeAction:
                ModeInfo.SetActive(false);
                ModeGrab.SetActive(false);
                ModeGear.SetActive(true);
                ModeGlass.SetActive(false);
                break;
            case GameCursor.ModeGrab:
                ModeGear.SetActive(false);
                ModeInfo.SetActive(false);
                ModeGrab.SetActive(true);
                ModeGlass.SetActive(false);
                break;
            case GameCursor.ModeGlass:
                ModeGear.SetActive(false);
                ModeInfo.SetActive(false);
                ModeGrab.SetActive(false);
                ModeGlass.SetActive(true);
                break;
        }
    }
}
