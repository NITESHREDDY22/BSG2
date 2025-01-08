    using UnityEngine;
using System.Collections;
using EZCameraShake;

public class CameraFollow : MonoBehaviour
{

    [HideInInspector]
    public Vector3 startingPosition;

    //[HideInInspector]
    public bool isFollowing;

    public Transform birdToFollow;

    void Awake()
    {
        startingPosition = transform.position;
        float x = Mathf.Clamp(1f, 0f, 12f);//Note: 1f,minCameraX, maxCameraX;
        transform.position = new Vector3(x, startingPosition.y, startingPosition.z);
    }

    void Update()
    {
        if (isFollowing)
        {
            if(GameManager.Instance.slingShot!=null){
                if(GameManager.Instance.slingShot.isLeftSide){
                    if (CameraShaker.Instance != null)
                    {
                        if (CameraShaker.Instance.shakeon)
                        {
                            return;
                        }
                    }
                    if (birdToFollow != null)
                    {
                        var birdPosition = birdToFollow.position;
                        float x = Mathf.Clamp(birdPosition.x, 0f, 12f);//Note: 1f,minCameraX, maxCameraX;
                        transform.position = new Vector3(x, startingPosition.y, startingPosition.z);
                    }
                    else
                    {
                        isFollowing = false;
                    }
                }
            }
        }
    }

} // CameraFollow