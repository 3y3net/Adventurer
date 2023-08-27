using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class Saveable : MonoBehaviour
{
    public bool savePosition, saveRotation, saveActive;

    public Vector3 pos;
    public Quaternion rot;
    public string UID;

    UniqueId uid;
    public MonoBehaviour[] components;

    //Objects to store serialized data for this object
    [System.Serializable]
    public class SerializableList<T>
    {
        public List<T> list;
    }

    [SerializeField] SerializableList<string> keys = new SerializableList<string>();
    [SerializeField] SerializableList<string> values = new SerializableList<string>();

    [System.Serializable]
    public struct ObjData
	{
        public string keys;
        public string values;
	}
    
    // Start is called before the first frame update
    void Start()
    {
        //if not existst, assign an unique id
        components = GetComponents< MonoBehaviour>();
        uid = gameObject.GetComponent<UniqueId>();
        if (null == uid)
        {
            uid = this.gameObject.AddComponent<UniqueId>();
            UID = uid.uniqueId;
        }
        UID = uid.uniqueId;
        LoadSaveManager.instance.Refresh();
    }

    void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        LoadSaveManager.instance.Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        //Double-check for assign a unique id
        uid = gameObject.GetComponent<UniqueId>();
        if (null == uid)
        {
            uid = this.gameObject.AddComponent<UniqueId>();
            UID = uid.uniqueId;
        }
    }

    public void SaveObjectToPlayerPrefs(string prefix="")
    {        
        //
        pos = transform.position;
        rot = transform.rotation;

        keys.list.Clear();
        values.list.Clear();

        //Save this object data into list
        keys.list.Add("this");
        values.list.Add(JsonUtility.ToJson(this));
        
        //Check for all monobehaviors attached to call customSave
        for (int i = 0; i < components.Length; i++)
        {
            var thisType = components[i].GetType();
            if (thisType.GetMethod("CustomSave") != null && thisType.GetMethod("CustomLoad") != null)
            {
                MethodInfo mInfo = thisType.GetMethod("CustomSave");
                string returnValue = (string)mInfo.Invoke(components[i], null);                

                //Save object data to list
                keys.list.Add(thisType.ToString());
                values.list.Add(returnValue);
            }
        }

        //Convert list to json
        string jsonKeys = JsonUtility.ToJson(keys);
        string jsonValues = JsonUtility.ToJson(values);

        //Debug.Log(name + " - " + jsonKeys);
        //Debug.Log(name + " - " + jsonValues);


        //Save jsoned list into playerprefs
        PlayerPrefs.SetString(prefix + UID +"_keys", jsonKeys);
        PlayerPrefs.SetString(prefix + UID + "_values", jsonValues);        
    }

    public string SaveObjectToJSON(string prefix = "")
    {
        pos = transform.position;
        rot = transform.rotation;

        keys.list.Clear();
        values.list.Clear();

        //Save this object data into list
        keys.list.Add("this");
        values.list.Add(JsonUtility.ToJson(this));

        //Check for all monobehaviors attached to call customSave
        for (int i = 0; i < components.Length; i++)
        {
            var thisType = components[i].GetType();
            if (thisType.GetMethod("CustomSave") != null && thisType.GetMethod("CustomLoad") != null)
            {
                MethodInfo mInfo = thisType.GetMethod("CustomSave");
                string returnValue = (string)mInfo.Invoke(components[i], null);

                //Save object data to list
                keys.list.Add(thisType.ToString());
                values.list.Add(returnValue);
            }
        }

        //Convert list to json
        string jsonKeys = JsonUtility.ToJson(keys);
        string jsonValues = JsonUtility.ToJson(values);

        //Debug.Log(name + " - " + jsonKeys);
        //Debug.Log(name + " - " + jsonValues);

        ObjData objData = new ObjData();

        objData.keys = jsonKeys;
        objData.values = jsonValues;        

        return JsonUtility.ToJson(objData);
    }

    public void LoadObject(string prefix="")
	{
        string jsonKeys = null;
        string jsonValues = null;
        keys.list.Clear();
        values.list.Clear();

        //Get data from playerprefs
        if (PlayerPrefs.HasKey(prefix + UID + "_keys") && PlayerPrefs.HasKey(prefix + UID + "_values"))
        {
            jsonKeys = PlayerPrefs.GetString(prefix + UID + "_keys");
            jsonValues = PlayerPrefs.GetString(prefix + UID + "_values");

            //Debug.Log(name + " - " + jsonKeys);
            //Debug.Log(name + " - " + jsonValues);

            //Deserialize keys and values
            keys = JsonUtility.FromJson<SerializableList<string>>(jsonKeys);
            values = JsonUtility.FromJson<SerializableList<string>>(jsonValues);

            Saveable svb = new Saveable();
            JsonUtility.FromJsonOverwrite(values.list[keys.list.IndexOf("this")], svb);

            if (savePosition)
                transform.position = svb.pos;
            if (saveRotation)
                transform.rotation = svb.rot;

            Destroy(svb);

            for (int i = 0; i < components.Length; i++)
            {
                var thisType = components[i].GetType();
                if (thisType.GetMethod("CustomSave") != null && thisType.GetMethod("CustomLoad") != null)
                {
                    MethodInfo mInfo = thisType.GetMethod("CustomLoad");

                    object[] parametersArray = new object[] { values.list[keys.list.IndexOf(thisType.ToString())] };

                    mInfo.Invoke(components[i], parametersArray);

                }
            }
        }
    }

    
}
