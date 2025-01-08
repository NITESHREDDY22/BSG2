using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class move : MonoBehaviour
{
    [SerializeField]
    private Transform childTransf;
    [SerializeField]
    private Transform transfB;
    [SerializeField]
    private float speed;
    private Vector3 startPosition;
    private Vector3 nextPosition;

    // Use this for initialization
    void Start()
    {

        startPosition = childTransf.localPosition;
        nextPosition = transfB.position;
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Bird")
        {
            //stick.DOLocalMove(Vector2.zero, 0.5f);
            Move();
        }
    }

    public void Move()
    {
        //childTransf.localPosition = Vector3.Lerp(childTransf.localPosition, nextPosition, speed * Time.deltaTime);
        childTransf.position = Vector3.MoveTowards(startPosition, nextPosition, speed * Time.deltaTime);
    }
}
