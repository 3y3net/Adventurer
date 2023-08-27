using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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

    // Targeting ray length
    private float targetingRayLength = Mathf.Infinity;

    // Camera component reference
    public Camera camera;
    private HighlightObject lastHc;

    private bool clickStarted = false;
    private HighlightObject hcClicked = null;

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
        camera = Camera.main;
        lastHc = null;
    }

    // 
    void Update()
    {
        TargetingRaycast();
    }

    // 
    public void TargetingRaycast()
    {
        // Current target object transform component
        Transform targetTransform = null;
        float distance = 1000000f;

        //Debug.Log("BUSCANDO");

        // If camera component is available
        if (camera != null)
        {
            //Debug.Log("CAM NO NULL");
            RaycastHit hitInfo;
            // Create a ray from mouse coords
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

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
                Debug.Log("CAM HIT " + targetTransform.gameObject.name);
                if (lastHc != null && lastHc != hc)
                {
                    lastHc.MouseExit(0f);
                    lastHc = hc;
                }
                if (lastHc == null || lastHc != hc)
                {
                    hc.MouseEnter(distance);
                    lastHc = hc;
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
            else if (lastHc != null)
            {
                lastHc.MouseExit(0f);
                lastHc = null;
            }
        }
        else if (lastHc != null)
        {
            lastHc.MouseExit(0f);
            lastHc = null;
        }
    }
}
