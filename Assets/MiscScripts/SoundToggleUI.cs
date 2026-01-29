using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Toggle Soundbtn;

    void OnEnable()
    {
        Debug.Log($"IsMuted?{SoundManager.IsMuted()}, {SoundManager.IsMusicMuted()}, soundOn? {PlayerPrefs.GetInt("sound",1) }");

        if(Soundbtn == null)Soundbtn = GetComponent<Toggle>();
        if(Soundbtn)
        Soundbtn.isOn = (PlayerPrefs.GetInt("sound",1)==1);
    }
}
