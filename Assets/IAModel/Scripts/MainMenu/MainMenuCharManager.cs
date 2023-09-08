using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCharManager : MonoBehaviour {
    public List<string> anims = new List<string>();
    public List<GameObject> materials = new List<GameObject>();
    List<Material> backupMaterials = new List<Material>();
    public float changeTime = 5f;
    public float fadeTime = 0.5f;

    public int currentIndex = 1;
    public Animator animator;
    public float deltaTime = 0;

    
    public FadeState fadeState = FadeState.FadedIn;

    public float secondsToCrazy = 5;
    float crazyCounter = 0;
    bool crazyMode = false;
    public string cheatCode = "DANCE";
    public string endCheatCode = "STOP";

    public float timeoutDuration = 1.0f;

    private string userInput = "";
    private float timeoutTime = 0.0f;

    public CallbackEventHandler CallBackFade;

    public bool inForceFade = false;
    private IEnumerator coroutine;

    public Transform weapon;
    public List<Transform> WeaponPositions = new List<Transform>();

    // Use this for initialization
    void Start () {
        if (animator==null)
            animator = GetComponent<Animator>();
        deltaTime = 0f;
        foreach (GameObject mt in materials)
            backupMaterials.Add(materials[0].GetComponent<Renderer>().material);
    }

    public void LockCharacter(bool locked, Material mat)
    {
        if(locked)
            foreach (GameObject go in materials)
                go.GetComponent<Renderer>().material=mat;

    }



    bool antRoot = false;
    // Update is called once per frame
    void Update () {

        if (inForceFade)
            return;

        if (Input.inputString.Length > 0)
        {
            timeoutTime = Time.time + timeoutDuration;
            userInput += Input.inputString;
            if (userInput.Contains(cheatCode))
            {
                crazyCounter = secondsToCrazy;
            }
            if (userInput.Contains(endCheatCode))
            {
                crazyCounter = 0;
                crazyMode = false;
                animator.applyRootMotion = antRoot;
                if (anims.Count>0)
                    PlayAnimation(currentIndex);
                else
                    animator.Play("anim01", 0, 0);
            }
        }
        else if ((Time.time > timeoutTime) && (userInput.Length > 0))
        {
            userInput = "";
        }

        if (crazyMode)
            return;

        crazyCounter += Time.deltaTime;
        if (crazyCounter > secondsToCrazy && fadeState == FadeState.FadedIn)
        {
            crazyMode = true;
            antRoot= animator.applyRootMotion;
            animator.applyRootMotion=false;
            animator.Play("dance9", 0, 0);
        }

        /*
        if (fadeState == FadeState.FadedOut)
        {
            fadeState = FadeState.ToFadeIn;
            PlayAnimation(currentIndex);
            coroutine = FadeTo(0f, 1f, fadeTime);
            StartCoroutine(coroutine);
        }
        else if (fadeState == FadeState.FadedIn)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > changeTime && anims.Count>1)
            {
                deltaTime = 0f;
                fadeState = FadeState.ToFadeOut;
                coroutine = FadeTo(1f, 0f, fadeTime);
                StartCoroutine(coroutine);
            }
        }
        */
    }

    public void ForceFadeIn(float from, CallbackEventHandler cbFade=null)
    {
        inForceFade = true;
        if(fadeState!=FadeState.ToFadeIn)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            float currentAlpha = from;
            if (from < 0)
                currentAlpha = materials[0].GetComponent<Renderer>().material.color.a;
            coroutine = FadeTo(currentAlpha, 1f, fadeTime*(1-currentAlpha));
            StartCoroutine(coroutine);
            
        }
        CallBackFade = cbFade;
    }

    public void ForceFadeOut(float from, CallbackEventHandler cbFade=null)
    {
        inForceFade = true;
        if (fadeState != FadeState.ToFadeOut)
        {
            if(coroutine!=null)
                StopCoroutine(coroutine);
            float currentAlpha = from;
            if(from<0)
                currentAlpha = materials[0].GetComponent<Renderer>().material.color.a;
            coroutine = FadeTo(currentAlpha, 0f, fadeTime * currentAlpha);
            StartCoroutine(coroutine);
        }
        CallBackFade = cbFade;
    }

    public void ForceFadeInmediate(float alpha)
    {
        //Debug.Log("ForceFadeInmediate");
        if (coroutine != null)
            StopCoroutine(coroutine);
        foreach (GameObject go in materials)
        {
            Material mat = go.GetComponent<Renderer>().material;
            Color newColor = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
            mat.color = newColor;
        }
    }

    IEnumerator FadeTo(float from, float to, float aTime)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {            
            foreach (GameObject go in materials)
            {                
                Material mat = go.GetComponent<Renderer>().material;
                Color newColor = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(from, to, t));
                mat.color = newColor;
            }
            yield return null;
        }

        //Ensures target is reached
        foreach (GameObject go in materials)
        {            
            Material mat = go.GetComponent<Renderer>().material;
            Color newColor = new Color(mat.color.r, mat.color.g, mat.color.b, to);
            mat.color = newColor;
            //Debug.Log("FadeTo :"+to);
        }
        //inForceFade = false;
        if (to > 0.9)
        {
            fadeState = FadeState.FadedIn;
            if (CallBackFade != null)
                CallBackFade(FadeState.FadedIn, gameObject);
        }
        else
        {
            fadeState = FadeState.FadedOut;
            if (CallBackFade != null)
                CallBackFade(FadeState.FadedOut, gameObject);
        }
    }

    void PlayAnimation(int anim)
    {     
         if(weapon!=null)
        {
            weapon.position = WeaponPositions[anim].position;
            weapon.rotation = WeaponPositions[anim].rotation;
        }

        deltaTime = 0f;
        animator.transform.position = Vector3.zero;
        animator.transform.rotation = Quaternion.identity;
        animator.Play(anims[anim], 0, 0);
        currentIndex++;
        currentIndex %= anims.Count;
    }
}
