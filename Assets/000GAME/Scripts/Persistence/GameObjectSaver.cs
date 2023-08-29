using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSaver : PersistentData
{
    public List<Transform> toSave = new List<Transform>();

    protected override string SetKey()
    {        
        // Here the key will be based on the name of the transform, the transform's type and a unique identifier.        
        return transform.name + "_ObjList_" + uniqueIdentifier;        
    }

    protected override void Save()
    {
        for(int i=0; i<toSave.Count; i++)
        {
            saveLoad.Save(key+"_" +i+ "_pos", toSave[i].position);
            saveLoad.Save(key + "_" + i + "_rot", toSave[i].rotation);
            saveLoad.Save(key + "_" + i + "_ena", toSave[i].gameObject.activeSelf);            
        }        
    }

    protected override void Load()
    {
        Debug.Log(key);
        for (int i = 0; i < toSave.Count; i++)
        {

            // Create a variable to be passed by reference to the Load function.
            Vector3 position = toSave[i].position;
            Quaternion rotation = toSave[i].rotation;
            bool enabled = toSave[i].gameObject.activeSelf;

            // If the load function returns true then the position can be set.
            if (saveLoad.Load(key + "_" + i + "_pos", ref position))
                toSave[i].position = position;
            if (saveLoad.Load(key + "_" + i + "_rot", ref rotation))
                toSave[i].rotation = rotation;
            if (saveLoad.Load(key + "_" + i + "_ena", ref enabled))
            {
                toSave[i].gameObject.SetActive(enabled);
                Debug.Log(key + "_" + i + "_ena="+ enabled);
            }
        }
    }
}
