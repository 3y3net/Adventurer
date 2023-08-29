using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine.UI;

public class HighlightGroup : MonoBehaviour {

    public List<GameObject> objToHighlight = new List<GameObject>();
    public float flashDuration = 0.5f;
    public Color lightColor = Color.cyan;
    public GameObject UIElement;
    public Transform characterPosition;
    public Transform CameraPosition;
    public Transform LabelPosition;
    public bool autoFire = false;
    public bool useCamera = true, showCursor=true, moveCharacter=true;
    public bool deactivateOnZoom = true;
    public bool exclusiveFocus = true;
    public int disableOnly = -1;
    public bool showExit = true;
    public bool charToOrigin = false;
    public bool openInventory = false;
    public bool seeThroug = false;
    public bool seeIcon = true;
    public int textId = -1;

    Transform characterOriginalPosition, CameraOriginalPosition;

    bool triggered = false;

    public void ManualTrigger(Collider other)
    {
        OnTriggerEnter(other);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player" && SceneManager.instance.gameMode == SceneManager.GameMode.Locomotion)
        {
            if (triggered)
                return;
            triggered = true;
            EnableHighlight();
            DoFlash();
            HighlighterManager.instance.SetCurrentGroup(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player" && SceneManager.instance.gameMode == SceneManager.GameMode.Locomotion)
        {
            DisableHighlight();
            triggered = false;
            StopFlash();
            HighlighterManager.instance.SetCurrentGroup(null);
        }
    }

    public void GroupActivated()
    {
        if(deactivateOnZoom)
            DisableHighlight();
        StopFlash();
    }

    public void GroupDeActivated()
    {
        if (deactivateOnZoom)
            EnableHighlight();
    }

    public void EnableHighlight()
    {
        foreach (GameObject go in objToHighlight)
        {
            //Debug.Log(go.name);
            go.GetComponent<Highlighter>().enabled = true;
            go.GetComponent<HighlightObject>().isActive = true;
            go.GetComponent<HighlightObject>().enabled = true;
        }
    }

    public void DisableHighlight()
    {
        int i = 0;
        foreach (GameObject go in objToHighlight)
        {
            if (disableOnly == i || disableOnly == -1)
            {
                go.GetComponent<Highlighter>().enabled = false;
                go.GetComponent<HighlightObject>().isActive = false;
                go.GetComponent<HighlightObject>().enabled = false;
            }
            i++;
        }
    }

    public void DoFlash()
    {
        foreach (GameObject go in objToHighlight)
        {
            go.GetComponent<Highlighter>().tween = true;
            go.GetComponent<Highlighter>().tweenDuration = flashDuration;
            //go.GetComponent<Highlighter>().ConstantOn();
        }
        //Invoke("StopFlash", flashDuration);
    }

    public void StopFlash()
    {
        foreach(GameObject go in objToHighlight)
        {
            go.GetComponent<Highlighter>().tween = false;
            //go.GetComponent<Highlighter>().ConstantOff();
        }
        triggered = false;
    }

}