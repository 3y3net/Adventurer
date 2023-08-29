using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImScene : ReactionBase
{

    public string sceneName;                    // The name of the scene to be loaded.
    public string startingPointInLoadedScene;   // The name of the StartingPosition in the newly loaded scene.

    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.
    private SaveLoad saveLoad;

    protected override void SpecificInit()
    {
        saveLoad = GameObject.FindObjectOfType<SaveLoad>();
        sceneController = GameObject.FindObjectOfType<SceneController>();
    }


    protected override void ImmediateReaction()
    {
        saveLoad.Save(PlayerMovement.startingPositionKey, "startingPointInLoadedScene");        
        // Start the scene loading process.
        sceneController.FadeAndLoadScene(sceneName);
    }
}
