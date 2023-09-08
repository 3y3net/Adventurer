using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadLevel : MonoBehaviour {

    private float mProgress;
    public Image imageComponent, cargaImg;
    public Sprite GoToInicio;
    public GameObject Bar, Button, LoadUI, ToHide;
    public List<GameObject> toHide = new List<GameObject>();
    public string nivel;
    public bool keepFaded = false;
    private AsyncOperation async;
    private float mSmoothFactor = 1.5f;
    private float mSmoothedProgress = 0.0f;
    private bool mIsFirstSmoothProgressEnd = true, isFullLoaded=false;
    private bool waitingKey = false;

    string[] niveles = { "Prototype" };

    public void ResetLevels()
    {
        cargaImg.sprite = GoToInicio;        
    }

    public void GotoLevel(int level)
    {
        SetLevel(level);
        DoLoadLevel();
    }

    public void SetLevel(int lvl)
    {
        nivel = niveles[lvl];
    }

    public void SetLevelInicial()
    {
        nivel = "Prototype";
    }

    public void DoLoadLevel()
    {
        GlobalConfiguration.instance.CallBackFade = FadeHandler;
        GlobalConfiguration.instance.FadeOut();        

    }

    void FadeHandler(GlobalConfiguration.EventInfo eventInfo, GameObject destination = null)
    {
        if(eventInfo.messageInfo==GlobalConfiguration.EventType.EndFadeOut)
        {
            foreach (GameObject go in toHide)
                go.SetActive(false);

            LoadUI.SetActive(true);
            if (!keepFaded)
                GlobalConfiguration.instance.FadeIn();
            else
                EndFade();
        }
        if (eventInfo.messageInfo == GlobalConfiguration.EventType.EndFadeIn)
        {
            EndFade();
        }
    }

    void EndFade()
    {
        StartCoroutine("load");
    }

    IEnumerator load()
    {
        #if LEGACY_LEVEL_LOADER
        async = Application.LoadLevelAsync(nivel);
        #else
        //async = SceneManager.LoadSceneAsync(nivel);
        #endif
        async.allowSceneActivation = false;
        yield return async;
    }

    public  void GoToLevel()
    {
        //SingletonLoadLevelAsync.GetInstance().ShowLoadedLevel();
        LoadUI.SetActive(false);
        StartCoroutine("ShowOpenedLevel");
    }

    IEnumerator ShowOpenedLevel()
    {
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            yield return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingKey)
        {
            if (Input.anyKey)
            {
                waitingKey = false;
                GoToLevel();
            }
        }

        if (isFullLoaded)
        {
            ToHide.SetActive(false);
            Button.SetActive(true);
            waitingKey = true;
        }
        else
        {

            if (async != null)
            {
                //Smooth the progress
                mSmoothedProgress += mSmoothFactor * Time.deltaTime;
                if (mSmoothedProgress > async.progress)
                    mSmoothedProgress = async.progress;
            }
            //Don't load next level until "mSmoothedProgress" to 0.9
            if (Mathf.Approximately(mSmoothedProgress, 0.9f))
            {
                //Waits a frame to update UI
                if (mIsFirstSmoothProgressEnd == true)
                    mIsFirstSmoothProgressEnd = false;
                else
                    isFullLoaded = true;
            }

            //mProgress = SingletonLoadLevelAsync.GetInstance().RawProgress;
            //mProgress = SingletonLoadLevelAsync.GetInstance().FullRawProgress;
            //mProgress = SingletonLoadLevelAsync.GetInstance().SmoothProgress;
            mProgress = mSmoothedProgress / 0.9f;

            if (imageComponent == null)
                return;

            imageComponent.fillAmount = mProgress;

        }
    }
}
