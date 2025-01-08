using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyObj : MonoBehaviour {

	public Cage myCage;
   // public AudioClip KeyCollectSFX;
    public GameObject ParticleFX;

	public void callkeycageAction()
    {
        Instantiate(ParticleFX, gameObject.transform.position, Quaternion.identity);
        myCage.checkCageOpen();
        if (!SoundManager.IsMuted()) 
        {
            SoundsHandler.Instance.PlaySource1Clip(3,0);
        }
    }
}
