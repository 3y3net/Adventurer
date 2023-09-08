using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    public float rotateSpeed = 10;
    public Transform rotateAround = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(rotateAround==null)
            transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        else
            transform.RotateAround(rotateAround.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
