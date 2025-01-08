using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNew : MonoBehaviour {
    public int speed;
    public float maxRotation = 45f;

		void Update()
		{
			transform.Rotate(Vector3.forward * -speed * Time.deltaTime);
		}

        void OnDisable()
        {
            GetComponent<RotateNew>().enabled = false;
        }
        void OnEnable()
        {
            GetComponent<RotateNew>().enabled = true;
        }
}
