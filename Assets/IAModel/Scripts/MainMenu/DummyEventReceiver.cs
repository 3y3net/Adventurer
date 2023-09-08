using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEventReceiver : MonoBehaviour
{
    public GameObject spores;
    public void HitPlayer(int value)
    {
    }

    public void SporeAttackStart()
    {
        spores.GetComponent<ParticleSystem>().Play();
    }

    public void SporeAttackEnd()
    {
        spores.GetComponent<ParticleSystem>().Stop();
    }
}
