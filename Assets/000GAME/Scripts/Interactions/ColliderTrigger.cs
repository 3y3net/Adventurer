using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour {

    public GameObject toDetect;
    public GameConditionList[] conditonList;
    public ReactionList defaultReactionList;    // If none of the ConditionCollections are reacted to, this one is used.

    public Transform beforeEnter;

    public int depth = 5;
    public List<Vector3> positions = new List<Vector3>();
    Vector3 lastPosition = Vector3.zero;
    Vector3 lastPosition2 = Vector3.zero;

    public bool isIn;

    void Start()
    {
        for (int i = 0; i < depth; i++)
            positions.Add(Vector3.zero);
    }

    void PushPosition(Vector3 pos)
    {
        for (int i = depth - 1; i > 0; i--)
            positions[i] = positions[i - 1];
        positions[0] = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == toDetect)
        {
            isIn = true;
            Interact();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == toDetect)
            isIn = false;
    }

    void FixedUpdate()
    {        
        if (!isIn)
        {
            PushPosition(toDetect.transform.position);
        }
    }

    // This is called when the player arrives at the interactionLocation.
    public void Interact()
    {
        Vector3 np = positions[depth - 1];
        np.z = 1f;
        beforeEnter.position = np;
        // Go through all the ConditionCollections...
        for (int i = 0; i < conditonList.Length; i++)
        {
            // ... then check and potentially react to each.  If the reaction happens, exit the function.
            if (conditonList[i].CheckAndReact())
                return;
        }

        // If none of the reactions happened, use the default ReactionCollection.
        if (defaultReactionList)
            defaultReactionList.React();
    }
}
