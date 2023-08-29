using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImCameraComplex : ReactionBase
{
    public GameObject vCam;
    CamManager cm;

    public List<GameObject> HideObject = new List<GameObject>();
    public List<GameObject> ShowObject = new List<GameObject>();

    public GameConditionList exitConditions;
    public ReactionList onExitReactionList;

    protected override void SpecificInit()
    {
        cm = FindObjectOfType<CamManager>();
        if (!cm)
            throw new UnityException("CamManager could not be found, ensure that it exists in the Persistent scene.");
    }

    protected override void ImmediateReaction()
    {
        cm.AddvCam(vCam, ExitCam, ExitCondition);

        foreach (GameObject go in HideObject)
        {
            InteractionObject io = go.GetComponent<InteractionObject>();
            Collider col = go.GetComponent<Collider>();
            if(col!=null)
                col.enabled = false;            
            if(io!=null)
            {
                HighlightingSystem.Highlighter hl = io.HighLightObject.GetComponent<HighlightingSystem.Highlighter>();
                if(hl!=null)
                    hl.enabled = false;
            }
            else
            {
                HighlightingSystem.Highlighter hl = go.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = false;
            }
        }

        foreach (GameObject go in ShowObject)
        {
            Collider col = go.GetComponent<Collider>();
            InteractionObject io = go.GetComponent<InteractionObject>();

            if(col!=null) //Si hay collider
            {
                //Si hay objeto destino y está activo...
                if (io != null && io.HighLightObject.activeSelf)
                    col.enabled = true;
                //Si objeto no activo quito collider
                if (io != null && !io.HighLightObject.activeSelf)
                    col.enabled = false;
            }
                       
            if (io != null )
            {
                HighlightingSystem.Highlighter hl = io.HighLightObject.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = true;
            }
            else
            {
                HighlightingSystem.Highlighter hl = go.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = true;
            }
        }
    }

    public void ExitCam()
    {
        //cm.BackClicked();
        foreach (GameObject go in HideObject)
        {
            Collider col = go.GetComponent<Collider>();
            InteractionObject io = go.GetComponent<InteractionObject>();
            if (col != null) //Si hay collider
            {
                //Si hay objeto destino y está activo...
                if (io != null && io.HighLightObject.activeSelf)
                    col.enabled = true;
                //Si objeto no activo quito collider
                if (io != null && !io.HighLightObject.activeSelf)
                    col.enabled = false;
            }

            if (io != null)
            {
                HighlightingSystem.Highlighter hl = io.HighLightObject.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = true;
            }
            else
            {
                HighlightingSystem.Highlighter hl = go.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = true;
            }
        }

        foreach (GameObject go in ShowObject)
        {
            Collider col = go.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
            InteractionObject io = go.GetComponent<InteractionObject>();
            if (io != null)
            {
                HighlightingSystem.Highlighter hl = io.HighLightObject.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = false;
            }
            else
            {
                HighlightingSystem.Highlighter hl = go.GetComponent<HighlightingSystem.Highlighter>();
                if (hl != null)
                    hl.enabled = false;
            }
        }
    }

    public bool ExitCondition()
    {
        if(exitConditions.requiredConditions.Length>0)
        {
            bool value= !exitConditions.CheckAndReact();
            if(value)
            {
                if (onExitReactionList)
                    onExitReactionList.React();

            }
            return value;
        }

        if (onExitReactionList)
            onExitReactionList.React();
        return true;
    }
}
