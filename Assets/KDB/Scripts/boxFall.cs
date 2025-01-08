using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxFall : MonoBehaviour {
	[SerializeField]
	private Transform box1;
	[SerializeField]
	private Transform box2;
	[SerializeField]
	private Transform box3;
	[SerializeField]
	private Transform box4;
	[SerializeField]
	private Transform box5;
	[SerializeField]
	private Transform box6;
	[SerializeField]
	private Transform box7;
	float time;
	/*[SerializeField]
	private Transform box8;
	[SerializeField]
	private Transform box9;
	[SerializeField]
	private Transform box10;
	[SerializeField]
	private Transform box11;
	[SerializeField]
	private Transform box12; */


	// Use this for initialization
	void Start () {
		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time > time + 3.5) {
			box1.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 8) {
			box2.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 12) {
			box3.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 16) {
			box4.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 20) {
			box5.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 24) {
			box6.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
		if (Time.time > time + 28) {
			box7.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}
	}
}
