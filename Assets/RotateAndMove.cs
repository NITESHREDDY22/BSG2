using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateAndMove : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform transfB;
    private Vector3 posA;
    private Vector3 posB;

    private Vector3 nextPosition;
    private bool RotateDirection = true;
    [SerializeField]
    private Transform childTransf;
    // Use this for initialization
    void Start()
    {
        posA = childTransf.localPosition;
        posB = transfB.localPosition;
        nextPosition = posB;
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        if( RotateDirection)
            transform.Rotate(Vector3.forward * -180 * Time.deltaTime);
        else
            transform.Rotate(Vector3.forward * 180 * Time.deltaTime);

    }
    void OnDisable()
    {
        try
        {
            GetComponent<MoveLeftandRight>().enabled = false;
        }
        catch (Exception e)
        {
            //print("error");
        }
    }
    void OnEnable()
    {
        try
        {
            GetComponent<MoveLeftandRight>().enabled = true;
        }
        catch (Exception e)
        {
            //print("error");
        }
    }

    private void Move()
    {
        childTransf.localPosition = Vector3.MoveTowards(childTransf.localPosition, nextPosition, speed * Time.deltaTime);


        if (Vector3.Distance(childTransf.localPosition, nextPosition) <= 0.1)
        {

            changeDestination();
        }
    }

    void changeDestination()
    {
        nextPosition = nextPosition != posA ? posA : posB;
        if (RotateDirection == true)
            RotateDirection = false;
        else
            RotateDirection = true;
    }
}
