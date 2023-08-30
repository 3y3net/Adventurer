using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour {

    //Nested vCams opened
    public Camera currentCam;
    public List<GameObject> zoomPositionList = new List<GameObject>();
    public float zoomSpeed = 10;
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
        backClicked = true;
    }

    public void AddvCam(GameObject vCam, ExitCam callBack, CanExit check)
    {
        zoomPositionList.Add(vCam);
        vCam.SetActive(true);
        CamZoomed = true;
        CallBack.Add(callBack);
        ExitCheck.Add(check);
    }

    // Update is called once per frame
    private void Update()
    {
        if (zoomPositionList.Count > 0)
        {
            SceneManager.instance.SetGameMode(SceneManager.GameMode.ZoomArea);
            int index = zoomPositionList.Count - 1;
            currentCam.transform.rotation = Quaternion.Slerp(currentCam.transform.rotation, zoomPositionList[index].transform.rotation, Time.deltaTime * zoomSpeed);
            currentCam.transform.position = Vector3.Slerp(currentCam.transform.position, zoomPositionList[index].transform.position, Time.deltaTime * zoomSpeed);

            if (gameState != null && gameState.gameStates[(int)GameStates.HasTablet])
                DD_GameManager.GameUIManager.instance.HideTabletIcon();
    
            if (BackButton != null && !BackButton.activeSelf)
                BackButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || backClicked)
            {
                backClicked = false;                

                if (ExitCheck[index] != null)
                    if (!ExitCheck[index]())
                        return;

                GameObject go = zoomPositionList[index];
                zoomPositionList.RemoveAt(index);
                go.SetActive(false);
                if (CallBack[index] != null)
                    CallBack[index]();
                if (zoomPositionList.Count == 0)
                    CamZoomed = false;
                CallBack.RemoveAt(index);
                ExitCheck.RemoveAt(index);
            }
        }      

        if (zoomPositionList.Count == 0 && BackButton != null && BackButton.activeSelf)
        {
            BackButton.SetActive(false);
            SceneManager.instance.SetGameMode(SceneManager.GameMode.Locomotion);
        }

        if (gameState != null && gameState.gameStates[(int)GameStates.HasTablet] && !gameState.gameStates[(int)GameStates.TableVisible])
            DD_GameManager.GameUIManager.instance.HideTabletIcon();
        if (zoomPositionList.Count == 0 && gameState != null && gameState.gameStates[(int)GameStates.HasTablet] && gameState.gameStates[(int)GameStates.TableVisible])
            DD_GameManager.GameUIManager.instance.ShowTabletIcon();
    }
}
