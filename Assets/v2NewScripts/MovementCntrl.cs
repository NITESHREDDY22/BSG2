using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCntrl : MonoBehaviour
{
    public Vector2 direction =Vector2.zero;
    public float speed=1f;
    public float length=1f;
    public bool _isPingpong =false;
    public float delaymystate = 1f;
    Transform mTransform;
    Vector2 startPos;

    bool _Activeself = false;
    private void Start()
    {
        mTransform = transform;
        startPos = transform.localPosition;
        Invoke("activate", delaymystate);
    }
    void OnDisable()
    {
        GetComponent<MovementCntrl>().enabled = false;
    }
    void OnEnable()
    {
        GetComponent<MovementCntrl>().enabled = true;
    }

    public void activate()
    {
        _Activeself = true;
    }
    private void FixedUpdate()
    {
        if (_Activeself)
        {
            if (_isPingpong)
            {
                mTransform.localPosition = startPos + direction * Mathf.PingPong(Time.time * speed, length);
            }
            else
            {
                float amplitude = Mathf.Sin(Time.time * speed) * length;
                Vector2 updatedPos = startPos + direction * amplitude;
                mTransform.localPosition = updatedPos;
            }
        }
    }
}
