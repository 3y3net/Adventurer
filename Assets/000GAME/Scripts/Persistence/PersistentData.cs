using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersistentData : MonoBehaviour {

    public string uniqueIdentifier;             // A unique string set by a scene designer to identify what is being saved.
    public SaveLoad saveLoad;
    protected string key;                       // A string to identify what is being saved.  This should be set using information about the data as well as the uniqueIdentifier.

    private void Awake()
    {
        // Find the SceneController and store a reference to it.
        saveLoad = FindObjectOfType<SaveLoad>();

        // If the SceneController couldn't be found throw an exception so it can be added.
        if (!saveLoad)
            throw new UnityException("SaveLoad object could not be found, ensure that it exists in the Persistent scene.");

        // Set the key based on information in inheriting classes.
        key = SetKey();
        SpecificAwake();
    }

    protected virtual void SpecificAwake()
    { }

    private void OnEnable()
    {
        // Subscribe the Save function to the BeforeSceneUnload event.
        saveLoad.OnClose += Save;

        // Subscribe the Load function to the AfterSceneLoad event.
        saveLoad.OnOpen += Load;
    }


    private void OnDisable()
    {
        // Unsubscribe the Save function from the BeforeSceneUnloud event.
        saveLoad.OnClose -= Save;

        // Unsubscribe the Load function from the AfterSceneLoad event.
        saveLoad.OnOpen -= Load;
    }


    // This function will be called in awake and must return the intended key.
    // The key must be totally unique across all Saver scripts.
    protected abstract string SetKey();


    // This function will be called just before a scene is unloaded.
    // It must call saveData.Save and pass in the key and the relevant data.
    protected abstract void Save();


    // This function will be called just after a scene is finished loading.
    // It must call saveData.Load with a ref parameter to get the data out.
    protected abstract void Load();

}
