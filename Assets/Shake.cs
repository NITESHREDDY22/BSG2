using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {


    public float speed = 8.0f;
    public float amount = 0.1f;
    public Transform OriginalPos;
    private Vector3 pos;
    private bool isVibrate = false;
    public float startTime = 0.0f;
    public float timer = 1.0f;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (isVibrate)
        {
            Debug.LogError(OriginalPos.position.x);
            pos = OriginalPos.position;
            pos.x = pos.x+ Mathf.Sin(Time.time * speed) * amount ;
            transform.position= pos;

            startTime += Time.deltaTime;
            if (startTime > timer)
            {
                isVibrate = false;
                startTime = 0.0f;
            }

        }
        else
        {
            transform.position = OriginalPos.position;
        }



    }

    void OnCollisionEnter2D(Collision2D target)
    {
        isVibrate = true;
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(6, 0.2f);
        }
    }
}
