using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance = null;
    public static SceneManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SceneManager)FindObjectOfType(typeof(SceneManager));
                if (_instance == null)
                {
                    GameObject managers = GameObject.Find("/GameManagers");
                    if (managers == null)
                        managers = new GameObject("GameManagers");
                    _instance = (new GameObject("GameManager")).AddComponent<SceneManager>();
                    _instance.transform.parent = managers.transform;
                }
            }
            return _instance;
        }
    }

    public enum GameMode
	{
        Locomotion,
        ZoomArea,
        Inspect,
        Menu
	}

    public GameMode gameMode;

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.T)) {
            if (gameMode == GameMode.Locomotion)
                gameMode = GameMode.ZoomArea;
            else
                gameMode = GameMode.Locomotion;
		}

    }
}
