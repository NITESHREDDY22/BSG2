using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulamOscillation : MonoBehaviour
{
    public float maxAngle = 30f;
    public float swingSpeed = 1f;

    private Rigidbody2D rb;
    private float initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed * Mathf.PI * 2) * maxAngle;
        rb.MoveRotation(initialRotation + angle);
    }
}
