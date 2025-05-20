using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateSprite : MonoBehaviour
{
    public float amplitude = 1f;     // Distance to move
    public float frequency = 1f;     // Speed of oscillation
    public Vector3 direction = Vector3.up;  // Direction (default: vertical)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + direction.normalized * offset;
    }
}