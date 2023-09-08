using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharValues
{
    public GameObject character;
    public string charName;
    [TextArea(3, 10)]
    public string description;
    public bool Locked = true;
}

[System.Serializable]
public class WeaponValues
{
    public GameObject weapon;
    public string weaponName;
    [TextArea(3, 10)]
    public string description;
    public bool Locked = true;
}

public enum FadeState { ToFadeIn, ToFadeOut, FadedIn, FadedOut }
public delegate void CallbackEventHandler(FadeState state, GameObject sender);

public class MenuSceneManager : MonoBehaviour {

    public List<CharValues> character;
    public List<WeaponValues> weapons;

    public GameObject platform;    
    public Text nameText;

    public GameObject frameDescription;
    public Text textDescription;    

    private bool _isLerping;
    private float _timeStartedLerping;
    private Vector3 _startPosition;
    private Vector3 _endPosition;


    public float timeChangeChar = 600;
    float curTime = 0;

    int curChar = 0, oldChar=0;
    int curWeapon = 0, oldWeapon = 0;

    bool isFading=false;
    public Material lockMaterial;

    public List<AudioClip> music = new List<AudioClip>();
    AudioSource audioSource;
    int currentClip = 0;


    public Button chars, weaps;
    public Text textChar, textWeaps;
    public Color active, pasive, textPasive;

    List<int> playList = new List<int>();
    bool modeChars = true;

    public Canvas canvasMainMenu;
    public Canvas canvasMenuInfo;

    public void ClickChars()
    {
        chars.image.color = active;
        weaps.image.color = pasive;
        textChar.color = active;
        textWeaps.color = textPasive;
        modeChars = true;
        weapons[curWeapon].weapon.GetComponent<MainMenuWeaponManager>().ForceFadeOut(-1, FadeHandler);

        chars.enabled = false;
        weaps.enabled = true;

    }

    public void Clickweaps()
    {
        chars.image.color = pasive;
        weaps.image.color = active;
        textWeaps.color = active;
        textChar.color = textPasive;
        modeChars = false;
        character[curChar].character.GetComponent<MainMenuCharManager>().ForceFadeOut(-1, FadeHandler);

        chars.enabled = true;
        weaps.enabled = false;
    }

    // Use this for initialization
    void Start () {
        platform.SetActive(false);
        foreach (CharValues cv in character)
            cv.character.SetActive(false);
        character[0].character.SetActive(true);
        nameText.text = character[0].charName;
        textDescription.text = character[0].description;

        foreach (WeaponValues cv in weapons)
            cv.weapon.SetActive(false);

        for (int i = 0; i < music.Count; i++)
            playList.Add(i);
        RandomizeList();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music[playList[currentClip++]];
        audioSource.Play();
        currentClip %= music.Count;
    }

    void RandomizeList()
    {
        for(int i=0; i<100; i++)
        {
            int a = Random.Range(0, playList.Count);
            int b = Random.Range(0, playList.Count);
            int aux = playList[a];
            playList[a] = playList[b];
            playList[b] = aux;
        }
    }

    void FadeHandler(FadeState fadeState, GameObject sender)
    {
        //Debug.Log("FadeHandler");
        switch (fadeState)
        {
            case FadeState.FadedIn:
                isFading = false;
                break;
            case FadeState.FadedOut:
                if (modeChars)
                {
                    foreach (WeaponValues cv in weapons)
                        cv.weapon.SetActive(false);
                    for (int i = 0; i < character.Count; i++)
                    {
                        if (i == curChar)
                        {
                            character[i].character.SetActive(true);
                            character[i].character.GetComponent<MainMenuCharManager>().LockCharacter(character[i].Locked, lockMaterial);
                        }
                        else
                            character[i].character.SetActive(false);
                    }

                    character[curChar].character.transform.localPosition = Vector3.zero;
                    character[curChar].character.transform.localRotation = Quaternion.identity;

                    if (character[curChar].Locked)
                    {
                        nameText.text = "Enemy Locked";
                        textDescription.text = "<size=25>Enemy Locked</size>\n\n<size=17>\n\nThis enemy is not yet accesible.\n\nYou will have to complete more levets until you can face this alien.\n\nKeep fighting for the Human Being! </size>";
                    }
                    else
                    {
                        nameText.text = character[curChar].charName;
                        textDescription.text = character[curChar].description;
                    }

                    character[curChar].character.GetComponent<MainMenuCharManager>().ForceFadeIn(0, ForcedFadeEnds);
                    if (curChar == 0)
                        platform.SetActive(false);
                    else
                        platform.SetActive(true);
                }
                else
                {
                    platform.SetActive(true);
                    foreach (CharValues cv in character)
                        cv.character.SetActive(false);
                    for (int i = 0; i < weapons.Count; i++)
                    {
                        if (i == curWeapon)
                        {
                            weapons[i].weapon.SetActive(true);
                            weapons[i].weapon.GetComponent<MainMenuWeaponManager>().LockCharacter(character[i].Locked, lockMaterial);
                        }
                        else
                            weapons[i].weapon.SetActive(false);
                    }

                    if (weapons[curWeapon].Locked)
                    {
                        nameText.text = "Weapon Locked";
                        textDescription.text = "<size=25>weapon Locked</size>\n\n<size=17>\n\nThis weapon is not yet accesible.\n\nYou will have to complete more levets until you can use this weapon.\n\nKeep fighting for the Human Being! </size>";
                    }
                    else
                    {
                        nameText.text = weapons[curWeapon].weaponName;
                        textDescription.text = weapons[curWeapon].description;
                    }

                    weapons[curWeapon].weapon.GetComponent<MainMenuWeaponManager>().ForceFadeIn(0, ForcedFadeEnds);
                }
                break;
        }
    }
    void ForcedFadeEnds(FadeState fadeState, GameObject sender)
    {
        isFading = false;
        if (modeChars)
        {
            character[curChar].character.GetComponent<MainMenuCharManager>().deltaTime = 0;
            sender.GetComponent<MainMenuCharManager>().inForceFade = false;
        }
        else
        {
            sender.GetComponent<MainMenuWeaponManager>().inForceFade = false;
        }
        
    }

    public void NextChar()
    {
        isFading = true;
        if (modeChars)
        {
            oldChar = curChar;
            curChar++;
            curChar %= character.Count;
            if (character[oldChar].character.activeSelf)
                character[oldChar].character.GetComponent<MainMenuCharManager>().ForceFadeOut(-1, FadeHandler);
        }
        else
        {
            oldWeapon = curWeapon;
            curWeapon++;
            curWeapon %= weapons.Count;
            if (weapons[oldWeapon].weapon.activeSelf)
                weapons[oldWeapon].weapon.GetComponent<MainMenuWeaponManager>().ForceFadeOut(-1, FadeHandler);
        }
    }
   
    public void PrevChar()
    {
        if(modeChars)
        {
            nameText.text = "";
            oldChar = curChar;
            curChar--;
            if (curChar < 0)
                curChar = character.Count - 1;
            if (character[oldChar].character.activeSelf)
                character[oldChar].character.GetComponent<MainMenuCharManager>().ForceFadeOut(-1, FadeHandler);
        }
        else
        {
            nameText.text = "";
            oldWeapon = curWeapon;
            curWeapon--;
            if (curWeapon < 0)
                curWeapon = weapons.Count - 1;
            if (weapons[oldWeapon].weapon.activeSelf)
                weapons[oldWeapon].weapon.GetComponent<MainMenuWeaponManager>().ForceFadeOut(-1, FadeHandler);
        }
        
    }

    // Update is called once per frame
    void Update () {

        if(!audioSource.isPlaying)
        {
            audioSource.clip = music[playList[currentClip++]];
            audioSource.Play();
            currentClip %= music.Count;
        }

        curTime += Time.deltaTime;
        if(curTime>timeChangeChar && false)
        {
            NextChar();
            curTime = 0;
        }  
	}

    public void Salir()
    {
        Application.Quit();
        Time.timeScale = 1;

    }


    public void ButtonInfoClick()
    {
        
        canvasMainMenu.enabled = false;
        canvasMenuInfo.enabled = true;
        Time.timeScale = 1;
        //NextChar();
    }

    public void ButtonBackMainMenu()
    {

        canvasMainMenu.enabled = true;
        canvasMenuInfo.enabled = false;
        Time.timeScale = 1;
    }

}
