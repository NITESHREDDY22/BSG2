using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoZoomMove : MonoBehaviour {

	// Use this for initialization

	private float orthoOrg;
	private float orthoCurr;
	//private Vector3 scaleOrg;
	private Vector3 posOrg;
	private Vector3 diffAdd;

	void Start () {
		orthoOrg = Camera.main.orthographicSize;//+0.5f;
		orthoCurr = orthoOrg;
		//scaleOrg = transform.localScale;

		posOrg = Camera.main.WorldToViewportPoint(transform.position);
		//Debug.Log (transform.position.x+"Before");
	}
	
	// Update is called once per frame
	void Update () {
		var osize = Camera.main.orthographicSize;
		if (orthoCurr != osize) {
			//transform.localScale = scaleOrg * osize / orthoOrg;
			orthoCurr = osize;
			transform.position  = Camera.main.ViewportToWorldPoint (posOrg);


			//Debug.Log (transform.position.x+"after");
		}
	}
}
