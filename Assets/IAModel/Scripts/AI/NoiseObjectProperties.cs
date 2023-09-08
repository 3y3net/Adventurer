using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseObjectProperties : MonoBehaviour
{

    public float noiseRange = 0f;    
    public bool noiseOnce = true;
    bool active = true;

    public void SetRange(float range)
    {
        noiseRange = range;
    }

    public bool IsNoiseActive()
    {
        return active;
    }

    public void DeactivateNoise()
    {
        if (noiseOnce)
            active = false;
    }

    public void ActivateNoise()
    {
        active = true;
    }
}
