using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2 : MonoBehaviour {
	public int degrees;
	
	void Update()
	{
		transform.Rotate(Vector3.forward *degrees * Time.deltaTime);
	}

    void OnDisable()
    {
        GetComponent<Rotate2>().enabled = false;
    }
    void OnEnable()
    {
        GetComponent<Rotate2>().enabled = true;
    }
}