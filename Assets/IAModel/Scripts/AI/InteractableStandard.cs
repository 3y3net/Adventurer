using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStandard : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player" || other.GetComponent<Collider>().tag == "Interactable")
        {
            Debug.Log(other.gameObject.name + " entered");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player" || other.GetComponent<Collider>().tag == "Interactable")
        {
            Debug.Log(other.gameObject.name + " exit");
        }
    }
}
