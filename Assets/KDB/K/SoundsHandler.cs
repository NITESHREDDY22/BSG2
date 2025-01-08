using UnityEngine;
using System.Collections;

public class SoundsHandler : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource AudioSource1;
    public AudioSource AudioSource2;

    public AudioClip[] music_clips;
    public AudioClip[] source1clips = new AudioClip[] { };
    public AudioClip[] source2clips = new AudioClip[] { };

    public static bool ismutemusic = false;
    public static bool ismutesound = false;

    public static SoundsHandler Instance;
    public bool PlayMusicDefault = true;
    public bool PlayMusicLoop = true;

    public float bgMusicDelay = 0;

    //public delegate void musicStatus();

    void Awake()
    {
        Instance = this;
       
        if (PlayMusicDefault)
        {
            PlayMusic(PlayMusicLoop, bgMusicDelay);
        }
		DontDestroyOnLoad(gameObject);
    }

    public void EnableSounds()
    {
        ismutesound = false;
        if (AudioSource1) AudioSource1.enabled = true;
        if (AudioSource2) AudioSource2.enabled = true;

        if (AudioSource1) AudioSource1.mute = false;
        if (AudioSource2) AudioSource2.mute = false;
    }

    public void DisableSounds()
    {
        ismutesound = true;

        if (AudioSource1) AudioSource1.enabled = false;
        if (AudioSource2) AudioSource2.enabled = false;

        if (AudioSource1) AudioSource1.mute = true;
        if (AudioSource2) AudioSource2.mute = true;
    }

    public void EnableMusic()
    {

         MusicSource.enabled = true;
         MusicSource.mute = false;
        ismutemusic = false;
        PlayMusic(true, 0);
        print("EnableMusic");
        //musicStatusEvent();
    }

    public void DisableMusic()
    {
        print("DisableMusic");
        ismutemusic = true;
        MusicSource.enabled = false;
        MusicSource.mute = true;

    }


    public void PlayMusic(bool sloop, float _delay = 0)
    {

        if (!ismutemusic)
        {
            MusicSource.clip = music_clips[0];
            MusicSource.loop = sloop;
            MusicSource.PlayDelayed(_delay);
        }
    }

    public void StopMusic()
    {
        if (!ismutemusic)
        {
            MusicSource.Stop();
        }
    }

    public void PlaySource1Clip(int clipindex, float delayy)
    {
        if (clipindex > source1clips.Length)
        {
         
            return;
        }
        if (!ismutesound)
        {
            AudioSource1.clip = source1clips[clipindex];
            AudioSource1.PlayDelayed(delayy);
        }
    }

    public void PlaySource2Clip(int clipindex, float delayy)
    {
        if (clipindex > source2clips.Length)
        {
            return;
        }
        if (!ismutesound)
        {
            if (clipindex < source2clips.Length)
            {
                AudioSource2.clip = source2clips[clipindex];
                AudioSource2.PlayDelayed(delayy);
            }
        }
    }


}
