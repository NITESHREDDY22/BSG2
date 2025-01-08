using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//s UnityEngine.Experimental.UIElements;

public class Cage : MonoBehaviour
{
	public GameObject[] Key;
   // public AudioClip CageOpenSFX;
    bool taskdone= false;
    int k = 0;
    bool keyscollected = false;

    public void checkCageOpen()
    {
        for(int i = 0; i < Key.Length; i++)
        {
            if (!Key[i].GetComponent<SpriteRenderer>().enabled)
            {
                k +=1;
                //Debug.LogWarning("keycollected");
            }
        }
        if(k == Key.Length)
        {
            keyscollected = true;
        }
        else
        {
            keyscollected= false;
        }
        callCage();
        k = 0;
    }

    public void callCage()
    {
        if (keyscollected&&!this.taskdone)
        {
            this.taskdone = true;
            //do reveal cage animation
            GetComponent<Animation>().Play();

            if(!SoundManager.IsMuted())
            {
                SoundsHandler.Instance.PlaySource1Clip(2,0);
            }
        }
    }

}
