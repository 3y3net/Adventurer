using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HighlighterManager : MonoBehaviour
{
    private static HighlighterManager _instance = null;
    public static HighlighterManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (HighlighterManager)FindObjectOfType(typeof(HighlighterManager));
                if (_instance == null)
                {
                    GameObject managers = GameObject.Find("/GameManagers");
                    if (managers == null)
                        managers = new GameObject("HighlighterManager");
                    _instance = (new GameObject("HighlighterManager")).AddComponent<HighlighterManager>();
                    _instance.transform.parent = managers.transform;
                }
            }
            return _instance;
        }
    }

    // Which layers targeting ray must hit (-1 = everything)
    public LayerMask targetingLayerMask = -1;
    public float zoomSpeed=10;

    // Targeting ray length
    private float targetingRayLength = Mathf.Infinity;

    // Camera component reference
    public Camera usedCamera;
    private HighlightObject lastHo;

    private bool clickStarted = false;
    private HighlightObject hcClicked = null;

    private HighlightGroup current, lastDiscarded;
    private Stack groupStack = new Stack();

    private Vector3 zoomPosition;
    private Quaternion zoomRotation;

    public void AddLayer(string layerName)
    {
        targetingLayerMask = targetingLayerMask | (1 << LayerMask.NameToLayer(layerName));
    }

    public void RemoveLayer(string layerName)
    {
        if (targetingLayerMask == (targetingLayerMask | (1 << LayerMask.NameToLayer(layerName))))
            targetingLayerMask ^= (1 << LayerMask.NameToLayer(layerName));
    }

    // 
    void Awake()
    {
        if(usedCamera==null)
            usedCamera = Camera.main;
        lastHo = null;
    }

    // 
    void Update()
    {
        //If we are at locomotion mode look for highloighGroups
        if (SceneManager.instance.gameMode == SceneManager.GameMode.Locomotion)
        {
            if (Input.GetButtonDown("Action")) {

                Debug.Log(groupStack.Count);
                //If there is a group in the stack
                if (groupStack.Count>0)
                {
                    HighlightGroup hg = (HighlightGroup)groupStack.Peek();
                    zoomPosition = current.CameraPosition.position;
                    zoomRotation = current.CameraPosition.rotation;
                    SceneManager.instance.SetGameMode(SceneManager.GameMode.ZoomArea);

                    hg.GroupActivated();
                }                
            }
        }
        //If we are at zoom mode look for highlighObjects
        if (SceneManager.instance.gameMode == SceneManager.GameMode.ZoomArea)
        {

            usedCamera.transform.rotation = Quaternion.Slerp(usedCamera.transform.rotation, zoomRotation, Time.deltaTime * zoomSpeed);
            usedCamera.transform.position = Vector3.Slerp(usedCamera.transform.position, zoomPosition, Time.deltaTime * zoomSpeed);

            if (Input.GetButtonUp("Fire2")) {

                if(current!=null)
                    current.GroupDeActivated();
                //If we are in a nested artificial group
                //POP current and leave next at top
                if (groupStack.Count > 1)
                {
                    lastDiscarded = (HighlightGroup)groupStack.Pop();
                    current = (HighlightGroup)groupStack.Peek();
                }
                else
                    SceneManager.instance.SetGameMode(SceneManager.GameMode.Locomotion);
            }

            LookForHighlighObjects();
        }

    }

    public void SetCurrentGroup(HighlightGroup grp)
    {
        current = grp;
        if (grp != null)
            groupStack.Push(grp);
        else if(groupStack.Count>0)
            lastDiscarded=(HighlightGroup)groupStack.Pop();
    }

    // 
    public void LookForHighlighObjects()
    {
        // Current target object transform component
        Transform targetTransform = null;
        float distance = 1000000f;

        // If camera component is available
        if (usedCamera != null)
        {
            //Debug.Log("CAM NO NULL");
            RaycastHit hitInfo;
            // Create a ray from mouse coords
            Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);

            // Targeting raycast
            if (Physics.Raycast(ray, out hitInfo, targetingRayLength, targetingLayerMask.value))
            {
                // Cache what we've hit
                targetTransform = hitInfo.collider.transform;
                distance = Vector3.Distance(targetTransform.position, SceneManager.instance.player.transform.position);                
            }
        }

        // If we've hit an object during raycast
        if (targetTransform != null)
        {
            // And this object has HighlighterController component
            HighlightObject hc = targetTransform.GetComponentInParent<HighlightObject>();
            if (hc != null)
            {
                //Debug.Log("CAM HIT " + targetTransform.gameObject.name);
                //Only highlight objects of current group
                if (!current.objToHighlight.Contains(hc.gameObject) && current.exclusiveFocus)
                    return;

                if (lastHo != null && lastHo != hc)
                {
                    lastHo.MouseExit(0f);
                    lastHo = hc;
                }
                if (lastHo == null || lastHo != hc)
                {
                    hc.MouseEnter(distance);
                    lastHo = hc;                   
                }
                //Debug.Log("CAM HIT " + targetTransform.gameObject.name+" tiene HC a "+ distance);
                if (Input.GetButtonDown("Fire1")) { clickStarted = true; hcClicked = hc; }
                if (Input.GetButtonUp("Fire2")) { clickStarted = true; hcClicked = hc; }

                // Transfer input information to the found HighlighterController
                if (Input.GetButtonUp("Fire1"))
                {
                    if (clickStarted && hcClicked == hc)
                        hc.Fire1(distance);
                    clickStarted = false;
                    hcClicked = null;
                }
                if (Input.GetButtonUp("Fire2"))
                {
                    if (clickStarted && hcClicked == hc)
                        hc.Fire2(distance);
                    clickStarted = false;
                    hcClicked = null;
                }
                hc.MouseOver(distance);
            }
            else if (lastHo != null)
            {
                lastHo.MouseExit(0f);
                lastHo = null;
            }

            HighlightGroup hg = targetTransform.GetComponentInParent<HighlightGroup>();
            if(hg!=null)
            {

            }
        }
        else if (lastHo != null)
        {
            lastHo.MouseExit(0f);
            lastHo = null;
        }
    }
}
