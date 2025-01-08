using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Fadeout : MonoBehaviour {
	//public Text fadingText;
	private int fadeTime = 10 ;
	private bool isFading = false;
	private float  startTime;
	private float timeLeft;
	// Use this for initialization
	void Start () {

		startFading (10);
	}

	// Update is called once per frame
	void Update () {
		//fadingText.text = "Text is faded out";
		float timePassed;
		timePassed = Time.time - startTime;
		timeLeft = fadeTime - timePassed;
		float alphaRemaining;
		if (timeLeft > 0) {
			alphaRemaining = timeLeft / fadeTime;

			Color c = gameObject.GetComponent<SpriteRenderer> ().color;
			c.a = alphaRemaining;
			gameObject.GetComponent<SpriteRenderer> ().color = c;
		}
	}

	public  void startFading(int fadeTimeInput) {
		fadeTime = fadeTimeInput;
		startTime = Time.time;
	}
}
