using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCustomSave : MonoBehaviour
{
    public int level;
    public float timeElapsed;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        timeElapsed = 47.5f;
        playerName = "Dr Charles Francis";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string CustomSave()
    {
        return JsonUtility.ToJson(this);
    }

    public void CustomLoad(string values)
    {
        TestCustomSave tcs = new TestCustomSave();
        JsonUtility.FromJsonOverwrite(values, tcs);
        level = tcs.level;
        timeElapsed = tcs.timeElapsed;
        playerName = tcs.playerName;
    }    
}
