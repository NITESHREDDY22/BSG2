using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMove1 : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    
    private Transform StartPosition;
    [SerializeField]
    private Transform TargetPosition;
    [SerializeField]
    private Transform P1;
    [SerializeField]
    private Transform P2;
    [SerializeField]
    private Transform P3;
    [SerializeField]
    private Transform P4;

    private Vector3 posA;
    private Vector3 posB;
    private Vector3 pos1,pos2,pos3,pos4;

    private Vector3 nextPosition;

    [SerializeField]
    
    // Use this for initialization
    void Start()
    {
        posA = StartPosition.localPosition;
        posB = TargetPosition.localPosition;
        nextPosition = posB;
        pos1 = P1.localPosition;
        pos2 = P2.localPosition;
        pos3 = P3.localPosition;
        pos4 = P4.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        Move();

    }
    void OnDisable()
    {
        GetComponent<MoveLeftandRight>().enabled = false;
    }
    void OnEnable()
    {
        GetComponent<MoveLeftandRight>().enabled = true;
    }

    private void Move()
    {
        StartPosition.localPosition = Vector3.MoveTowards(StartPosition.localPosition, nextPosition, speed * Time.deltaTime);


        if (Vector3.Distance(StartPosition.localPosition, nextPosition) <= 0.1)
        {

            changeDestination();
        }
    }

    void changeDestination()
    {
        if (nextPosition != pos2)
            nextPosition = pos3;
       else if (nextPosition == pos3)
            nextPosition = pos4;
       else if (nextPosition == pos4)
            nextPosition = pos1;
       else if (nextPosition == pos1)
            nextPosition = pos2;
       
    }
}
