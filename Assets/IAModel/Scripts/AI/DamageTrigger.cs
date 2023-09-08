using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour {

    public float minDamage = 1f;
    public float maxDamage = 1f;
    public List<AudioClip> hits;
    public List<GameObject> bloodSplatts;

    AudioSource audioSource;

    void Start()
    {
        if (hits!=null && hits.Count > 0)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.loop = false;
                audioSource.playOnAwake = false;
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playercontroller>().Damage(Random.Range(minDamage, maxDamage));
            if(audio)
            {
                Debug.Log(gameObject.name);
                audio.clip = hits[Random.Range(0,hits.Count)];
                audio.Play();
                var rot = Quaternion.FromToRotation(Vector3.up, other.ClosestPoint(transform.position));
                Instantiate(bloodSplatts[Random.Range(0, bloodSplatts.Count)], transform.position, rot, other.transform);
            }
        }
    }
    */

    public void HitPlayer(int value)
    {
        //other.gameObject.GetComponent<playercontroller>().Damage(Random.Range(minDamage, maxDamage));
        if (audioSource)
        {
            Debug.Log(value+" "+gameObject.name);
            audioSource.clip = hits[Random.Range(0, hits.Count)];
            audioSource.Play();
            //var rot = Quaternion.FromToRotation(Vector3.up, other.ClosestPoint(transform.position));
            //Instantiate(bloodSplatts[Random.Range(0, bloodSplatts.Count)], transform.position, rot, other.transform);
        }
    }
}
