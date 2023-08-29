using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIApp : MonoBehaviour {
    
    public AudioClip OpenAppClip, CloseAppClip;
    public GameObject thisApp, badgeApp;    
    public Text badgeText;
    public AudioSource audioSource;

    public bool isAppOpen = false;
    public bool autoCleadBadge = true;
    public bool keepLocalBadge = true;

    public int badgeAmount = 0;
    public int localAmount = 0;

    public void ShowHideApp()
    {
        if (!isAppOpen)
            OpenApp();
        else
            CloseApp();
    }

    public void OpenApp()
    {
        if(autoCleadBadge)
            badgeAmount = 0;
        if (!keepLocalBadge)
            localAmount = 0;
        isAppOpen = true;
        thisApp.GetComponent<Animator>().Play("Open", 0, 0);
        if (audioSource != null && OpenAppClip != null)
            audioSource.PlayOneShot(OpenAppClip);
    }

    public void CloseApp()
    {
        isAppOpen = false;
        thisApp.GetComponent<Animator>().Play("Close", 0, 0);
        if (audioSource != null && CloseAppClip != null)
            audioSource.PlayOneShot(CloseAppClip);
    }

    public void FastSilentCloseApp()
    {
        thisApp.transform.localScale = Vector3.zero;
        isAppOpen = false;
    }

    public void FastOpenApp()
    {
        thisApp.transform.localScale = new Vector3(1,1,1);
        isAppOpen = true;
    }

    void Update()
    {
        badgeText.text = "" + localAmount;
        if (localAmount > 0 && !badgeApp.activeSelf)
        {            
            badgeApp.SetActive(true);
        }
        if (localAmount == 0 && badgeApp.activeSelf)
            badgeApp.SetActive(false);
    }
}
