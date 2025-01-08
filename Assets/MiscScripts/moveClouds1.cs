using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveClouds1 : MonoBehaviour {

	[SerializeField]
	private float speed;

	[SerializeField]
	private Transform transfB;
	private Vector3 posA;
	private Vector3 posCloudSmall;
	private Vector3 posB;

	private Vector3 nextPosition;

	[SerializeField]
	private Transform childTransf;
	[SerializeField]
	private Transform cloudSmall;
	// Use this for initialization
	void Start () {
		posA = childTransf.localPosition;
		posCloudSmall = cloudSmall.localPosition;



		posB = transfB.localPosition;
		nextPosition = posB;

	}

	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move()
	{
		childTransf.localPosition = Vector3.MoveTowards(childTransf.localPosition, nextPosition, speed * Time.deltaTime);
		cloudSmall.localPosition = Vector3.MoveTowards (cloudSmall.localPosition, nextPosition, speed * Time.deltaTime);
		if (Vector3.Distance(childTransf.localPosition, nextPosition) <= 0.1) {

			//changeDestination();
			childTransf.localPosition = posA;
		}

		if (Vector3.Distance(cloudSmall.localPosition, nextPosition) <= 0.1) {

			//changeDestination();
			cloudSmall.localPosition = posCloudSmall;
		}
	}

	void changeDestination() {
		nextPosition = nextPosition != posA ? posA : posB;
	}
}
