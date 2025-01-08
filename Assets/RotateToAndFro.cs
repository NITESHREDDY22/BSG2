using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToAndFro : MonoBehaviour {

    public int speed;
    public float maxRotation = 45f;
    public float startTime = 0.0f;
    public float timer = 20.0f;
    public bool clockwise ;

    private void Start()
    {
       // startTime = Time.time;
       //transform.Rotate(Vector3.forward * -speed * Time.deltaTime);
    }
    void Update()
    {
        startTime += Time.deltaTime;
        if(startTime > timer)
        {
            if (clockwise == true)
                clockwise = false;
            else
                clockwise = true;

            startTime = 0.0f;
        }
        if (clockwise)
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        else
            transform.Rotate(Vector3.forward * -speed * Time.deltaTime); 

    }

   
}
