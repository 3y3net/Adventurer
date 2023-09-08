using AI_3y3net;
using Exploder.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamageControl : MonoBehaviour {

    public float hitpoints = 400f;
    public AudioClip breakObject;
    
    public bool explode = false;
    public Transform ExplosionPosition;
    public GameObject ExplosionPrefab;

    private bool active = true;

    private void Start()
    {
        gameObject.tag = "Exploder";
    }

    void Update()
    {
        if (hitpoints <= 0 && active)
        {
            active = false;            
            if (explode)
                PlayExplosions();
            else
            {
                ExploderSingleton.Instance.transform.position = transform.position;
                ExploderSingleton.Instance.ExplodeObject(gameObject);
                if (breakObject != null)
                {
                    GameLogic.instance.FX.clip = breakObject;
                    GameLogic.instance.FX.Play();
                }
            }            

        }

    }

    public void PlayExplosions()
    {
        //Object currentPrefabObject = 
        GameObject.Instantiate(ExplosionPrefab, ExplosionPosition.transform.position, Quaternion.identity);
        ExplodeObject();
    }

    void Damage(ImpactInfo impactInfo)
    {
        if (!active)
            return;
        hitpoints -= impactInfo.damage;
    }

    void ExplodeObject( )
    {

    }

    void Explosion(float damage)
    {
        if (!active)
            return;
        hitpoints -= damage;
    }
 }
