using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackCamManager : MonoBehaviour {

    public GameObject cam1, cam2;

    public float zMin, zMax, yDelta;

    public float coldDown = 3;

    float lastChange = 0;

    public void OnGroundClick(float zDes, float yCoord)
    {
        Debug.Log("zDes: " + zDes + " yCoord: "+yCoord);

        if(zDes > zMin && yCoord < yDelta && cam1.activeSelf && Time.time > lastChange + coldDown) {
            lastChange = Time.time;
                cam1.SetActive(false);
                cam2.SetActive(true);            
        }

        if (zDes < zMax && yCoord < yDelta && cam2.activeSelf && Time.time > lastChange + coldDown) {
            lastChange = Time.time;
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
    }

    // Use this for initialization

}
