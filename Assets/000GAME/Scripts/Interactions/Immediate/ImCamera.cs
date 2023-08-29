using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImCamera : ReactionBase
{

    public GameObject vCam;
    CamManager cm;

    protected override void SpecificInit()
    {
        cm = FindObjectOfType<CamManager>();
        if (!cm)
            throw new UnityException("CamManager could not be found, ensure that it exists in the Persistent scene.");
    }

    protected override void ImmediateReaction()
    {
        cm.AddvCam(vCam, null, null);
    }
}
