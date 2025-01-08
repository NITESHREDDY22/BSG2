using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToRight : MonoBehaviour
{
	float time;
	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform bottle1;
	[SerializeField]
	private Transform bottle2;
	[SerializeField]
	private Transform bottle3;
	[SerializeField]
	private Transform bottle4;

	//private Vector3 posA;
	private Vector3 posB;

	private Vector3 nextPosition;

	[SerializeField]
	private Transform targetPosObj;
	// Use this for initialization
	void Start()
	{
		time = Time.time;
		//posA = bottle1.localPosition;
		posB = targetPosObj.localPosition;
		nextPosition = posB;

	}

	// Update is called once per frame
	void Update()
	{
		Move ();

	}
	private void Move() {

		  

		if (bottle1 != null && Time.time > time + 2 ) {
			bottle1.localPosition = Vector3.MoveTowards (bottle1.localPosition, nextPosition, speed * Time.deltaTime);
		}
		if (bottle2 != null && Time.time > time + 10) {
			//(bottle1 != null || bottle1 == null ) && 
			bottle2.localPosition = Vector3.MoveTowards (bottle2.localPosition, nextPosition, speed * Time.deltaTime);
		}
		if (bottle3 != null && Time.time > time + 18) {

			bottle3.localPosition = Vector3.MoveTowards (bottle3.localPosition, nextPosition, speed * Time.deltaTime);
		}
					if (bottle4 != null && Time.time > time + 24) {

						bottle4.localPosition = Vector3.MoveTowards (bottle4.localPosition, nextPosition, speed * Time.deltaTime);

								
			}
		

	


	}
	}


