using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour {

    public event Action OnClose;          // Event delegate that is called just before a scene is unloaded.
    public event Action OnOpen;             // Event delegate that is called just after a scene is loaded.
    private SceneController sceneController;    // Reference to the SceneController so that this can subscribe to events that happen before and after scene loads.

    private void Awake()
    {
        // Find the SceneController and store a reference to it.
        sceneController = FindObjectOfType<SceneController>();

        // If the SceneController couldn't be found throw an exception so it can be added.
        if (!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
        LoadAll();
        Debug.Log(Application.persistentDataPath);
    }


    private void OnEnable()
    {    
        
        // Subscribe the Save function to the BeforeSceneUnload event.
        sceneController.BeforeSceneUnload += CloseEvent;

        // Subscribe the Load function to the AfterSceneLoad event.
        sceneController.AfterSceneLoad += OpenEvent;
        
    }


    private void OnDisable()
    {     
        
        // Unsubscribe the Save function from the BeforeSceneUnloud event.
        sceneController.BeforeSceneUnload -= CloseEvent;

        // Unsubscribe the Load function from the AfterSceneLoad event.
        sceneController.AfterSceneLoad -= OpenEvent;
               
    }

    public void CloseEvent() {
        if(OnClose!=null)
            OnClose();
    }

    public void OpenEvent()
    {
        if (OnOpen!=null)
            OnOpen();
    }    

    public void ClearAll()
    {
        boolKeyValuePairLists.Clear();
        intKeyValuePairLists.Clear();
        stringKeyValuePairLists.Clear();
        vector3KeyValuePairLists.Clear();
        quaternionKeyValuePairLists.Clear();
        SaveAll();
    }

    public void SaveGame()
    {
        CloseEvent();
        SaveAll();
    }

    public void SaveAll()
    {
        /*
        string dataAsJson = File.ReadAllText(Application.persistentDataPath + "/boolKeyValuePairLists.gd");
        boolKeyValuePairLists = JsonUtility.FromJson<KeyValuePairLists<bool>>(dataAsJson);
        */

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileBool = File.Create(Application.persistentDataPath + "/boolKeyValuePairLists.gd");
        bf.Serialize(fileBool, boolKeyValuePairLists);
        fileBool.Close();

        /*
        List<string> keys = boolKeyValuePairLists.keys;
        foreach (string k in keys)
        {
            Debug.Log("KEY: "+k);
        }
        */

        FileStream fileInt = File.Create(Application.persistentDataPath + "/intKeyValuePairLists.gd");
        bf.Serialize(fileInt, intKeyValuePairLists);
        fileInt.Close();

        FileStream fileString = File.Create(Application.persistentDataPath + "/stringKeyValuePairLists.gd");
        bf.Serialize(fileString, stringKeyValuePairLists);
        fileString.Close();

        FileStream fileVector = File.Create(Application.persistentDataPath + "/vector3KeyValuePairLists.gd");
        bf.Serialize(fileVector, vector3KeyValuePairLists);
        fileVector.Close();

        FileStream fileQuat = File.Create(Application.persistentDataPath + "/quaternionKeyValuePairLists.gd");
        bf.Serialize(fileQuat, quaternionKeyValuePairLists);
        fileQuat.Close();
    }


    public void LoadAll()
    {
        Debug.Log("Load ALL");
        if (File.Exists(Application.persistentDataPath + "/boolKeyValuePairLists.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/boolKeyValuePairLists.gd", FileMode.Open);
            boolKeyValuePairLists = (KeyValuePairLists<bool>)bf.Deserialize(file);
            file.Close();
            
        }

        if (File.Exists(Application.persistentDataPath + "/intKeyValuePairLists.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/intKeyValuePairLists.gd", FileMode.Open);
            intKeyValuePairLists = (KeyValuePairLists<int>)bf.Deserialize(file);
            file.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/stringKeyValuePairLists.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/stringKeyValuePairLists.gd", FileMode.Open);
            stringKeyValuePairLists = (KeyValuePairLists<string>)bf.Deserialize(file);
            file.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/vector3KeyValuePairLists.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/vector3KeyValuePairLists.gd", FileMode.Open);
            vector3KeyValuePairLists = (KeyValuePairLists<SerializableVector3>)bf.Deserialize(file);
            file.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/quaternionKeyValuePairLists.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/quaternionKeyValuePairLists.gd", FileMode.Open);
            quaternionKeyValuePairLists = (KeyValuePairLists<SerializableQuaternion>)bf.Deserialize(file);
            file.Close();
        }
    }

    [System.Serializable]
    public class KeyValuePairLists<T>
    {
        public List<string> keys = new List<string>();      // The keys are unique identifiers for each element of data. 
        public List<T> values = new List<T>();              // The values are the elements of data.


        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }


        public void TrySetValue(string key, T value)
        {
            // Find the index of the keys and values based on the given key.
            int index = keys.FindIndex(x => x == key);

            // If the index is positive...
            if (index > -1)
            {
                // ... set the value at that index to the given value.
                values[index] = value;
            }
            else
            {
                // Otherwise add a new key and a new value to the collection.
                keys.Add(key);
                values.Add(value);
            }
        }


        public bool TryGetValue(string key, ref T value)
        {
            // Find the index of the keys and values based on the given key.
            int index = keys.FindIndex(x => x == key);

            // If the index is positive...
            if (index > -1)
            {
                // ... set the reference value to the value at that index and return that the value was found.
                value = values[index];
                return true;
            }

            // Otherwise, return that the value was not found.
            return false;
        }
    }


    // These are collections for various different data types.
    public KeyValuePairLists<bool> boolKeyValuePairLists = new KeyValuePairLists<bool>();
    public KeyValuePairLists<int> intKeyValuePairLists = new KeyValuePairLists<int>();
    public KeyValuePairLists<string> stringKeyValuePairLists = new KeyValuePairLists<string>();
    public KeyValuePairLists<SerializableVector3> vector3KeyValuePairLists = new KeyValuePairLists<SerializableVector3>();
    public KeyValuePairLists<SerializableQuaternion> quaternionKeyValuePairLists = new KeyValuePairLists<SerializableQuaternion>();


    public void Reset()
    {
        boolKeyValuePairLists.Clear();
        intKeyValuePairLists.Clear();
        stringKeyValuePairLists.Clear();
        vector3KeyValuePairLists.Clear();
        quaternionKeyValuePairLists.Clear();
    }


    // This is the generic version of the Save function which takes a
    // collection and value of the same type and then tries to set a value.
    private void Save<T>(KeyValuePairLists<T> lists, string key, T value)
    {
        lists.TrySetValue(key, value);
    }


    // This is similar to the generic Save function, it tries to get a value.
    private bool Load<T>(KeyValuePairLists<T> lists, string key, ref T value)
    {
        return lists.TryGetValue(key, ref value);
    }


    // This is a public overload for the Save function that specifically
    // chooses the generic type and calls the generic version.
    public void Save(string key, bool value)
    {
        Save(boolKeyValuePairLists, key, value);
    }


    public void Save(string key, int value)
    {
        Save(intKeyValuePairLists, key, value);
    }


    public void Save(string key, string value)
    {
        Save(stringKeyValuePairLists, key, value);
    }


    public void Save(string key, Vector3 value)
    {
        Save(vector3KeyValuePairLists, key, value);
    }


    public void Save(string key, Quaternion value)
    {
        Save(quaternionKeyValuePairLists, key, value);
    }


    // This works the same as the public Save overloads except
    // it calls the generic Load function.
    public bool Load(string key, ref bool value)
    {
        return Load(boolKeyValuePairLists, key, ref value);
    }


    public bool Load(string key, ref int value)
    {
        return Load(intKeyValuePairLists, key, ref value);
    }


    public bool Load(string key, ref string value)
    {
        return Load(stringKeyValuePairLists, key, ref value);
    }


    public bool Load(string key, ref Vector3 value)
    {
        SerializableVector3 sv3 = new SerializableVector3();
        bool fine= Load(vector3KeyValuePairLists, key, ref sv3);
        value = sv3;
        return fine;
    }


    public bool Load(string key, ref Quaternion value)
    {
        SerializableQuaternion sq = new SerializableQuaternion();
        bool fine= Load(quaternionKeyValuePairLists, key, ref sq);
        value = sq;
        return fine;

    }
    
}
