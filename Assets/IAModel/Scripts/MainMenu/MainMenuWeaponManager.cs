using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWeaponManager : MonoBehaviour
{
    public List<GameObject> materials = new List<GameObject>();
    List<Material> backupMaterials = new List<Material>();

    public float fadeTime = 0.5f;

    public FadeState fadeState = FadeState.FadedIn;

    public CallbackEventHandler CallBackFade;

    public bool inForceFade = false;
    private IEnumerator coroutine;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject mt in materials)
            backupMaterials.Add(materials[0].GetComponent<Renderer>().material);
    }

    public void LockCharacter(bool locked, Material mat)
    {
        if (locked)
            foreach (GameObject go in materials)
                go.GetComponent<Renderer>().material = mat;
    }


    void Update()
    {
        if (inForceFade)
            return;
    }

    public void ForceFadeIn(float from, CallbackEventHandler cbFade = null)
    {
        inForceFade = true;
        if (fadeState != FadeState.ToFadeIn)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            float currentAlpha = from;
            if (from < 0)
                currentAlpha = materials[0].GetComponent<Renderer>().material.color.a;
            coroutine = FadeTo(currentAlpha, 1f, fadeTime * (1 - currentAlpha));
            StartCoroutine(coroutine);

        }
        CallBackFade = cbFade;
    }

    public void ForceFadeOut(float from, CallbackEventHandler cbFade = null)
    {
        inForceFade = true;
        if (fadeState != FadeState.ToFadeOut)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            float currentAlpha = from;
            if (from < 0)
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

}
