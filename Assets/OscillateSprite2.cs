using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    public float amplitude = 1f;           // Distance to move
    public float frequency = 1f;           // Oscillations per second
    public Vector3 direction = Vector3.up; // Direction of movement
    public bool startDownward = true;      // Start motion by going down?

    private Vector3 startPos;
    private float phaseOffset;

    void Start()
    {
        startPos = transform.position;

        // Phase offset: -π/2 makes the sine wave start moving downward
        phaseOffset = startDownward ? -Mathf.PI / 2f : 0f;
    }

    void Update()
    {
        float time = Time.time * frequency * Mathf.PI * 2;
        float offset = Mathf.Sin(time + phaseOffset) * amplitude;
        transform.position = startPos + direction.normalized * offset;
    }
}