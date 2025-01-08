using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderText : MonoBehaviour {
	//public TextMesh fadingText;
	private int fadeTime = 10;
	private bool isFading = false;
	private float startTime;
	private float timeLeft;
	public Color textColor = Color.black;
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "Foreground";
		this.gameObject.GetComponent<MeshRenderer> ().sortingOrder = '2';
		StartFading(fadeTime);
	}
	
	// Update is called once per frame
	void Update () {
		float timePassed;
		timePassed = Time.time - startTime;
		timeLeft = fadeTime - timePassed;
		float alphaRemaining;
		if (timeLeft > 0) {
			alphaRemaining = timeLeft / fadeTime;
			Color c = this.gameObject.GetComponent<MeshRenderer> ().material.color;
			c.a = alphaRemaining;
			this.gameObject.GetComponent<MeshRenderer> ().material.color = c;
		}
	}
	public void StartFading (int fadeTimeInput){
		fadeTime = fadeTimeInput;
		startTime = Time.time;
		}
}
