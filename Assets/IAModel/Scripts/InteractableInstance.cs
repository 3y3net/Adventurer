using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInstance : MonoBehaviour {

    BoxCollider col;
    public float defaultSize = 2;
    public int interactionKey = 0;
    public bool IsActive = true;
    bool isIn = false;

    // Use this for initialization
    void Start () {
        col = GetComponent<BoxCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(defaultSize, defaultSize, defaultSize);
        }
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsActive)
            return;
        if (other.GetComponent<Collider>().tag == "Player")
        {
            GameLogic.instance.ShowInteract();
            isIn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsActive)
            return;
        if (other.GetComponent<Collider>().tag == "Player")
        {
            GameLogic.instance.HideInteract();
            isIn = false;
        }
    }

    private void Update()
    {
        if (!IsActive)
            return;
        if (isIn)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameLogic.instance.Interact(interactionKey);
            }
        }
    }
}
