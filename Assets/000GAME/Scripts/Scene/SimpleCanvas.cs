using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCanvas : MonoBehaviour {

    public GameObject canvas;
    public bool disableAll = false;
    //public GraphicRaycaster gr;

	public void CloseCanvas()
    {
        canvas.SetActive(false);
        if (disableAll)
        {
            PlayerMovement pm = FindObjectOfType<PlayerMovement>();
            if (pm)
                pm.ResumeAll();
        }
        //gr.enabled = false;
    }

    void OnEnable()
    {
        if(disableAll)
        {
            PlayerMovement pm = FindObjectOfType<PlayerMovement>();
            if (pm)
                pm.StopAll();
        }
        //if (gr == null)
        //    gr = GetComponentInParent<GraphicRaycaster>();
        //gr.enabled = true;
    }
}
