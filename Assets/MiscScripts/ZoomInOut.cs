using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ZoomInOut : MonoBehaviour {

	Camera mainCamera;

	float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier, defaultSize;

	[HideInInspector]
	public BirdState birdState;
	Vector2 firstTouchPrevPos, secondTouchPrevPos;

	[SerializeField]
	float zoomModifierSpeed = 0.01f;
	[HideInInspector]
	public GameObject birdToThrow;
	public Transform birdWaitPosition;
	//[SerializeField]
	//Text text;

	// Use this for initialization
	void Start () {
		mainCamera = GetComponent<Camera> ();
		defaultSize = mainCamera.orthographicSize;
		//Debug.Log (birdState + "BirdState");
		//mainCamera.orthographicSize +=0.5f;
	}

	// Update is called once per frame


	void FixedUpdate ()
	{
		//GameObject[] balls = GameObject.FindGameObjectsWithTag ("Bird");


		/*	if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
		{
			Debug.Log("Touch greater than 1");
			Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out raycastHit))
			{
				Debug.Log("Something Hit");


				//OR with Tag

				if (raycastHit.collider.CompareTag("Bird"))
				{
					Debug.Log("Soccer Ball clicked");
				}
			}
		}*/
		
		//if (slingShootState == SlingshotState.Idle ||(!( gameState == GameState.Playing)) || (!(gameState == GameState.BirdMovingToSlingshot)))
		if (!(birdState == BirdState.Thrown)) {
			
			if (Input.touchCount == 2) {



					Touch firstTouch = Input.GetTouch (0);
					Touch secondTouch = Input.GetTouch (1);

					firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
					secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

					touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
					touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

					zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

					if (touchesPrevPosDifference > touchesCurPosDifference)
						mainCamera.orthographicSize += zoomModifier;
					if (touchesPrevPosDifference < touchesCurPosDifference)
						mainCamera.orthographicSize -= zoomModifier;



					mainCamera.orthographicSize = Mathf.Clamp (mainCamera.orthographicSize, defaultSize, 10.5f);


				}
					//	text.text = "Camera size " + mainCamera.orthographicSize;
				}
			}
		}



