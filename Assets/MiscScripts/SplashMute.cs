using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashMute : MonoBehaviour {
    public Toggle Soundbtn;
    void Start()
    {
        if (PlayerPrefs.GetInt("sound",1) ==0) 
        {
            Soundbtn.isOn = false;
        }
        else
        {
            Soundbtn.isOn = true;
        }
    }

    public void MuteAudios(Toggle t)
    {
        if (t.isOn)
        {
            PlayerPrefs.SetInt("sound", 1);
            SoundManager.Mute(false);

        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
            SoundManager.Mute(true);
        }
        if (!SoundManager.IsMuted())
        {
            SoundManager.PlaySFX("Click2");
        }
    }
}
