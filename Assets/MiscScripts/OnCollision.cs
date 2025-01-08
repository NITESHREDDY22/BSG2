using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour {
	bool collisionOver = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D target)
	{

		Debug.Log ("COLLISION CO");
		//  if (!firstTime)
		// {
		//   audioSource.Play();

		// }
		if (target.gameObject.name == "bottle" && collisionOver) {

			Debug.Log ("COLLISION CO");
			target.transform.parent = this.transform;
			collisionOver = false;


		}
	}
}
