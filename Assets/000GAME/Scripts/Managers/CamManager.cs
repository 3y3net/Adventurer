using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour {

    //Nested vCams opened
    public List<GameObject> currentVCam = new List<GameObject>();
    public bool LockWhenZoom = true;
    public bool CamZoomed = false;
    public GameObject BackButton;

    private bool backClicked = false;
    public delegate void ExitCam();
    private List<ExitCam> CallBack = new List<ExitCam>();

    public delegate bool CanExit();
    private List<CanExit> ExitCheck = new List<CanExit>();
    private bool restoreTablet = false;

    public GameObject dummyVcam;

    GameState gameState;

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        if(gameState!=null) //Siempre epezamos con la table visible (si existe)
            gameState.gameStates[(int)GameStates.TableVisible] = true;
    }

    public void AddDummyvCam()
    {
        AddvCam(dummyVcam, ExitCamDel, ExitCondition);
    }

    public void ExitCamDel()
    {

    }

    public bool ExitCondition()
    {
        return true;
    }

    public void BackClicked()
    {
        Debug.Log("BackClicked");
        backClicked = true;
    }

    public void AddvCam(GameObject vCam, ExitCam callBack, CanExit check)
    {
        currentVCam.Add(vCam);
        vCam.SetActive(true);
        CamZoomed = true;
        CallBack.Add(callBack);
        ExitCheck.Add(check);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentVCam.Count > 0)
        {
            if (gameState != null && gameState.gameStates[(int)GameStates.HasTablet])
                DD_GameManager.GameUIManager.instance.HideTabletIcon();
    
            if (BackButton != null && !BackButton.activeSelf)
                BackButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || backClicked)
            {
                backClicked = false;
                int index = currentVCam.Count - 1;

                if (ExitCheck[index] != null)
                    if (!ExitCheck[index]())
                        return;

                GameObject go = currentVCam[index];
                currentVCam.RemoveAt(index);
                go.SetActive(false);
                if (CallBack[index] != null)
                    CallBack[index]();
                if (currentVCam.Count == 0)
                    CamZoomed = false;
                CallBack.RemoveAt(index);
                ExitCheck.RemoveAt(index);
            }
        }
        if (currentVCam.Count == 0 && BackButton != null && BackButton.activeSelf)
        {
            BackButton.SetActive(false);
        }

        if (gameState != null && gameState.gameStates[(int)GameStates.HasTablet] && !gameState.gameStates[(int)GameStates.TableVisible])
            DD_GameManager.GameUIManager.instance.HideTabletIcon();
        if (currentVCam.Count == 0 && gameState != null && gameState.gameStates[(int)GameStates.HasTablet] && gameState.gameStates[(int)GameStates.TableVisible])
            DD_GameManager.GameUIManager.instance.ShowTabletIcon();
    }
}
