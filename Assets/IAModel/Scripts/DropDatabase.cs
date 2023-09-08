using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDatabase : MonoBehaviour {
    
    public static DropDatabase instance;
    public List<DropObject> AllDrops;
    //public weaponselector selector;
    //public playercontroller controller;

    private void Awake()
    {
        instance = this;
    }
}
