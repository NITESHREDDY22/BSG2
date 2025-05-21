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
    private float positionOffset = 0;
    void Awake()
    {
        startingPosition = transform.position;
        float x = Mathf.Clamp(0, 0f, positionOffset+12f);//Note: 1f,minCameraX, maxCameraX;
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
                        float x = Mathf.Clamp(birdPosition.x, positionOffset, startingPosition.x+12f);//Note: 1f,minCameraX, maxCameraX;
                        transform.position = Vector3.Lerp(transform.position, new Vector3(x, startingPosition.y, startingPosition.z),3*Time.deltaTime);
                    }
                    else
                    {
                        isFollowing = false;
                    }
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startingPosition, 2f * Time.deltaTime);
        }
    }

    public void ResetCameraTargetPosition(Vector3 pos)
    {
        isFollowing = false;
        startingPosition.x = pos.x+5.28f;
        positionOffset = startingPosition.x;

    }
    public void SetOffsetValue()
    {
        positionOffset = startingPosition.x;
    }

} // CameraFollow