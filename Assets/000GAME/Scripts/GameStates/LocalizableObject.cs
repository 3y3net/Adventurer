using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizableObject : MonoBehaviour {

    public int uniqueId = -1;
    public List<GameObject> selector = new List<GameObject>();

    // Use this for initialization
    void Start () {
        SetLanguagetext();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLanguagetext()
    {
        if (uniqueId != -1)
        {
            //Debug.Log("UNIQUEID " + uniqueId + " obj:"+gameObject.name);
            Text txt = GetComponent<Text>();
            if (txt != null)
                txt.text = LocalizableData.instance.languageText[uniqueId];
            ImText imText = GetComponent<ImText>();
            if(imText!=null)
                imText.message = LocalizableData.instance.languageText[uniqueId];   
        }

        if (selector.Count > (int)LocalizableData.instance.language)
            for (int i = 0; i < selector.Count; i++)
                if (i == (int)LocalizableData.instance.language)
                    selector[i].SetActive(true);
                else
                    selector[i].SetActive(false);
    }
}
