using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy :MonoBehaviour {


	[Header("If laser Enemy Only")]
	public GameObject lasers;
	public Collider2D Phycoll;
	public bool lasersON = false;
	public AudioClip LaserGridSFX;
	public AudioClip CollisionSFX;
	[HideInInspector]
	public GameObject CollisionParticleFX;
	public float LaserDuration = 2f;

	// Update is called once per frame
	
	void Start()
	{
		CollisionParticleFX = Resources.Load("CFX_Hit_C White") as GameObject;
		if (lasersON)
		{
			Invoke("LasersActiveON", LaserDuration);
		}
	}
	void LasersActiveON()
	{
		///SoundManager.PlaySFX(LaserGridSFX);
		lasers.SetActive(true);
		Phycoll.enabled = true;
		Invoke("LasersActiveOFF", LaserDuration);
	}

	void LasersActiveOFF()
	{
		lasers.SetActive(false);
		Phycoll.enabled = false;
		Invoke("LasersActiveON", LaserDuration);
	}
}
