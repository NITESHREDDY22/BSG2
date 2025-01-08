using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public int HealthCount = 1;
	public float killtime = 2f;
	[SerializeField]
	private GameObject EnemyDieAnim;
	//public AudioClip DyingSFX;

	public void Damage () {
        if (HealthCount > 0)
        {
			HealthCount -= 1;
            if (HealthCount == 0)
            {
				//kill me
				StartCoroutine(this.Outfromgame());
            }
        }
	}

	IEnumerator Outfromgame()
    {
        //2f for some particle on enemy disappear..
        if (!SoundManager.IsMuted()) 
        {
            SoundsHandler.Instance.PlaySource1Clip(5, 0);
        }
        GameObject Go = Instantiate(EnemyDieAnim, gameObject.transform.position, Quaternion.identity);
		Go.transform.parent = gameObject.transform.parent;
		yield return new WaitForSeconds(0);
		gameObject.SetActive(false);
    }
}
