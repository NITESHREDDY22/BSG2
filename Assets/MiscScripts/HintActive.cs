using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintActive : MonoBehaviour {

	public GameObject hintLine;
	public GameObject bottle1, bottle2, bottle3;
	public bool firstTime = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((bottle1 == null && bottle2 == null && bottle3 == null ) && firstTime) {

			hintLine.SetActive (true);
			firstTime = false;
		}
		
	}
}
