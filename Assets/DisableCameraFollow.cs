using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraFollow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera.main.GetComponent<CameraFollow>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
