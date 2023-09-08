using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalConfiguration : MonoBehaviour {

    public enum EventType
    {
        EndFadeIn, EndFadeOut
    }

    public class EventInfo
    {
        public EventType messageInfo;
        public GameObject origin = null;
    }

    public float fadeTime = 3.0f;
    public Color fadeColor = Color.white;
    public Image FadeImage;

    float alphaFadeValue = 0f;
    bool fadeIn = false;
    bool fadeOut = false;
    bool fadeAudio = false;
    Texture2D fadeTexture;

    public delegate void CallbackEventHandler(EventInfo eventInfo, GameObject destination = null);
    public CallbackEventHandler CallBackFade;

    public static GlobalConfiguration instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TestForFade();
    }

    public void FadeOut()
    {
        alphaFadeValue = 0f; //From clear to opaque
        fadeOut = true;
        FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alphaFadeValue);
        FadeImage.gameObject.SetActive(true);
    }

    public void FadeIn()
    {
        alphaFadeValue = 1f; //From opaque to clear
        fadeIn = true;
        FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alphaFadeValue);
        FadeImage.gameObject.SetActive(true);
    }

    void TestForFade()
    {
        if (fadeOut)
        {
            alphaFadeValue += Mathf.Clamp01(Time.deltaTime / fadeTime);

            FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alphaFadeValue);
            if (fadeAudio)
            {
                //fadeAudio.volume = initialVol * (1 - alphaFadeValue);
            }
        }

        if (fadeIn)
        {
            alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / fadeTime);
            FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alphaFadeValue);
            if (fadeAudio)
            {
                //fadeAudio.volume = (1 - alphaFadeValue);
            }
        }

        if (alphaFadeValue < 0.001 && fadeIn)
        {
            fadeIn = false;            
            FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
            FadeImage.gameObject.SetActive(false);
            if (CallBackFade != null)
            {
                EventInfo myEvent = new EventInfo();
                myEvent.messageInfo = EventType.EndFadeIn;
                CallBackFade(myEvent);
            }
        }

        if (alphaFadeValue > 0.999 && fadeOut)
        {
            fadeOut = false;
            FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
            if (CallBackFade != null)
            {
                EventInfo myEvent = new EventInfo();
                myEvent.messageInfo = EventType.EndFadeOut;
                CallBackFade(myEvent);
            }            
        }
    }
}
