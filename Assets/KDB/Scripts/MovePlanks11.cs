using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlanks11 : MonoBehaviour {
	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform positionB;
	private Vector3 posA;
	private Vector3 posB;
	private Vector3 posC;


	private Vector3 nextPosition;

	[SerializeField]
	private Transform positionA;

	[SerializeField]
	private Transform positionC;
	// Use this for initialization
	void Start () {

		posA = positionA.localPosition;
		posB = positionB.localPosition;
		posC = positionC.localPosition;
		nextPosition = posB;

	}

	// Update is called once per frame
	void Update () {

		Move();

	}

	private void Move()
	{
		positionA.localPosition = Vector3.MoveTowards(positionA.localPosition, nextPosition, speed * Time.deltaTime);


		if (Vector3.Distance(positionA.localPosition, nextPosition) <= 0.1) {

			changeDestination();
		}
	}

	void changeDestination() {
		
		nextPosition = nextPosition != posB
			? posB : posC;
	}
}