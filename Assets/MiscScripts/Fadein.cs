using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour {
	Image rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Image> ();
		Color c = rend.material.color;
		c.a = 0f;
		rend.material.color = c;
	
	}


	IEnumerator FadeIn()
	{
		for (float f = 0.05f; f <= 1; f += 0.05f) {
			Color c = rend.material.color;
			c.a = f;
			rend.material.color = c;
			yield return new WaitForSeconds (0.05f);
		}
	}

	public void startFadin (){
	
		StartCoroutine ("FadeIn");
	}
	// Update is called once per frame
	void Update () {
		startFadin ();
	}
}
