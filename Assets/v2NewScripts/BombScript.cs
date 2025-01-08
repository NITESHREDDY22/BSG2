using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BombScript : MonoBehaviour {

	public float fieldOfImpact;
	public float force;
	public LayerMask LayerToHit;
	public GameObject ExplosionEffect;
	//public AudioClip BombSFX;
	public bool isTimerBomb = false;


	
	// Update is called once per frame
	/*void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			explode();
		}
	}*/

	public void explode(bool destroy) {
		Collider2D[] objects =	Physics2D.OverlapCircleAll(transform.position, fieldOfImpact,LayerToHit);

		foreach (Collider2D obj in objects)
		{
			Vector2 direction = obj.transform.position - transform.position;

			if (obj.GetComponent<Rigidbody2D>() != null)
			{
				obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
			}
		}
		CameraShaker.Instance.ShakeOnce(4, 4, 0.1f,1f);
        if (!SoundManager.IsMuted()) 
        {
            SoundsHandler.Instance.PlaySource1Clip(4,0);
            }
        GetComponent<SpriteRenderer>().enabled = false;
		if (!isTimerBomb)
		{
			GetComponent<CircleCollider2D>().enabled = false;
		}
		GameObject Go = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
		StartCoroutine(CameraShakeOff(destroy,Go));
	}

	IEnumerator CameraShakeOff(bool destroy,GameObject Go) {
		yield return new WaitForSeconds(2f);
		CameraShaker.Instance.shakeon = false;
		Destroy(Go, 0f);
		if (destroy)
		{
			Destroy(gameObject);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
	}
}
