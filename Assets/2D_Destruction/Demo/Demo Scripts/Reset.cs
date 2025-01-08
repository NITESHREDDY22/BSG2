using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {
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
	public SlingShot slingShot;
	// Use this for initialization
	void Start () {
		mainCamera = GetComponent<Camera> ();
		defaultSize = mainCamera.orthographicSize;
		//Invoke ("DefaultZoom", 0.01f);
	}

	void DefaultZoom(){
		mainCamera.orthographicSize -=0.8f;
	}
	//void FixedUpdate()
	//{
	//	if (slingShot != null)
	//	{
	//		if (slingShot.slingShootState == SlingshotState.Idle && GameManager.Instance.gameState == GameState.Playing)
	//		{
	//			if (Input.touchCount == 2)
	//			{
	//				Touch firstTouch = Input.GetTouch(0);
	//				Touch secondTouch = Input.GetTouch(1);

	//				firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
	//				secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

	//				touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
	//				touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

	//				zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

	//				if (touchesPrevPosDifference > touchesCurPosDifference)
	//				{
	//					mainCamera.orthographicSize += zoomModifier;
	//				}


	//				if (touchesPrevPosDifference < touchesCurPosDifference)
	//				{
	//					mainCamera.orthographicSize -= zoomModifier;
	//				}
	//				mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, defaultSize, 10f);
	//			}
	//		}
	//	}
	//}
}