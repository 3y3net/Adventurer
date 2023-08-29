using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizableData : MonoBehaviour {

    public Dictionary<int, string> languageText = new Dictionary<int, string>();

    public static LocalizableData instance;

    public enum Language
    {
        English, Spanish, French
    }
    public Language language = Language.Spanish;

    void Awake()
    {
        instance = this;
        LoadLanguageText();
    }

    void LoadLanguageText()
    {
        languageText.Clear();
        //Debug.Log(language.ToString());
        Object[] allText = Resources.LoadAll(language.ToString(), typeof(TextAsset));
        for (int i = 0; i < allText.Length; i++)
        {
            //Debug.Log("carga "+allText[i].ToString());
            LoadTextAsset((TextAsset)allText[i]);
        }

        /*
        TextAsset txt = (TextAsset)Resources.Load(language.ToString(), typeof(TextAsset));
        LoadTextAsset(txt);
        */
    }

    void LoadTextAsset(TextAsset txt)
    {
        char[] archDelim = new char[] { '\r', '\n' };
        if (!txt)
            return;
        string[] objs = txt.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        int objIndex = -1;
        string textObject = "";
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].StartsWith("#")) //Comment 
            {
                //Debug.Log("Skipping " + objs[i]);
                continue;
            }

            //Detects object reference
            if (objs[i].StartsWith(">"))
            {
                if (objIndex != -1)
                {
                    //Debug.Log(objIndex+" - "+ textObject.Substring(0, (textObject.Length - 1) > 0 ? textObject.Length - 1 : 0));
                    languageText.Add(objIndex, textObject.Substring(0, (textObject.Length - 1) > 0 ? textObject.Length - 1 : 0));
                }
                objIndex = int.Parse(objs[i].Substring(1, objs[i].Length - 1).Trim().Split(' ')[0]);
                textObject = objs[i].Substring(objs[i].IndexOf(' ') + 1) + "\n";
            }
            else
                textObject += objs[i] + "\n";
        }

        if (objIndex != -1)
        {
            //Debug.Log(objIndex + " - " + textObject.Substring(0, (textObject.Length - 1) > 0 ? textObject.Length - 1 : 0));
            languageText.Add(objIndex, textObject.Substring(0, (textObject.Length - 1) > 0 ? textObject.Length - 1 : 0));
        }

    }
}
