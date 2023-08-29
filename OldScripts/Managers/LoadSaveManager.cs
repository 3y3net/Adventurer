using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LoadSaveManager : MonoBehaviour
{
    //Objects to store serialized data for this object
    [System.Serializable]
    public class SerializableList<T>
    {
        public List<T> list;
    }

    [SerializeField] SerializableList<string> keys = new SerializableList<string>();
    [SerializeField] SerializableList<bool> values = new SerializableList<bool>();
    [SerializeField] SerializableList<string> valuesJSON = new SerializableList<string>();

    private static LoadSaveManager _instance = null;
    public static LoadSaveManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (LoadSaveManager)FindObjectOfType(typeof(LoadSaveManager));
                if (_instance == null)
                {
                    GameObject managers = GameObject.Find("/GameManagers");
                    if (managers == null)
                        managers = new GameObject("GameManagers");
                    _instance = (new GameObject("LoadSaveManager")).AddComponent<LoadSaveManager>();
                    _instance.transform.parent = managers.transform;
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    public Saveable[] saveables;

    public void Refresh()
    {
        saveables = FindObjectsOfType<Saveable>(true);
    }

    void Start()
    {
        saveables = FindObjectsOfType<Saveable>(true);
    }

    public void SaveObjectsToPlayerPrefs(string prefix="")
	{
        if (prefix.Length>0 && !PlayerPrefs.HasKey(prefix))
            PlayerPrefs.SetString(prefix, "1");

        keys.list.Clear();
        values.list.Clear();
        foreach (Saveable sb in saveables)
        {
            sb.SaveObjectToPlayerPrefs(prefix);
            
            if (sb.saveActive)
            {
                keys.list.Add(sb.UID);
                values.list.Add(sb.gameObject.activeSelf);
            }
        }
        //Convert list to json
        string jsonKeys = JsonUtility.ToJson(keys);
        string jsonValues = JsonUtility.ToJson(values);

        //Save jsoned list into playerprefs
        PlayerPrefs.SetString(prefix+ gameObject.scene.name + ".object_keys", jsonKeys);
        PlayerPrefs.SetString(prefix+ gameObject.scene.name + ".active_values", jsonValues);
    }

    public void SaveObjectsToJSON(string prefix = "")
    {
        if (prefix.Length > 0 && !PlayerPrefs.HasKey(prefix))
            PlayerPrefs.SetString(prefix, "1");

        keys.list.Clear();
        values.list.Clear();
        valuesJSON.list.Clear();
        foreach (Saveable sb in saveables)
        {
            keys.list.Add(sb.UID);
            values.list.Add(sb.gameObject.activeSelf);
            valuesJSON.list.Add(sb.SaveObjectToJSON(prefix));
        }
        //Convert list to json
        string jsonKeys = JsonUtility.ToJson(keys);
        string jsonValues = JsonUtility.ToJson(values);
        string jsonValuesJSON = JsonUtility.ToJson(valuesJSON);

        //Save jsoned list into playerprefs
        PlayerPrefs.SetString(prefix + gameObject.scene.name + ".object_keys", jsonKeys);
        PlayerPrefs.SetString(prefix + gameObject.scene.name + ".active_values", jsonValues);
        PlayerPrefs.SetString(prefix + gameObject.scene.name + ".json_values", jsonValuesJSON);
    }

    public void LoadObjectsFromPlayerPrefs(string prefix = "")
    {
        keys.list.Clear();
        values.list.Clear();

        string jsonKeys = null;
        string jsonValues = null;

        if (PlayerPrefs.HasKey(prefix + gameObject.scene.name + ".object_keys") && PlayerPrefs.HasKey(prefix + gameObject.scene.name + ".active_values"))
        {
            jsonKeys = PlayerPrefs.GetString(prefix + gameObject.scene.name + ".object_keys");
            jsonValues = PlayerPrefs.GetString(prefix + gameObject.scene.name + ".active_values");
            //Deserialize keys and values
            keys = JsonUtility.FromJson<SerializableList<string>>(jsonKeys);
            values = JsonUtility.FromJson<SerializableList<bool>>(jsonValues);
        }
        
        foreach (Saveable sb in saveables)
        {            
            if (keys.list.IndexOf(sb.UID)>=0)
            {
				sb.gameObject.SetActive(values.list[keys.list.IndexOf(sb.UID)]);
            }
            sb.LoadObject(prefix);
        }

    }
}
