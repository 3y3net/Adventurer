using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellObjectProperties : MonoBehaviour {

    public float smellRange = 15f;
    public bool smellOnce = true;
    bool active = true;        

    public bool IsSmeelActive()
    {
        return active;
    }

    public void DeactivateSmell()
    {
        if(smellOnce)
            active = false;
    }

    public void ActivateSmell()
    {
        active = true;
    }
}
