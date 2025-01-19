using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class splashtoMenu : MonoBehaviour {

    public Image progressslider;
    public Text loadingText;
    public int timetoload = 2;
    bool adReady = false;
    public bool editorTest = false;
    void Start()
    {
     //PlayerPrefs.DeleteAll();
        StartCoroutine(fadAction_(0.1f));
    }
    IEnumerator fadAction_(float t)
    {
        yield return new WaitForSeconds(t);
        float progress = 0;
        float currentTime = 0;
        adReady = false;
        while (progress < timetoload)
        {
            progressslider.fillAmount = (progress / timetoload);

            if (progress - currentTime >= 1 && !adReady)
            {
                if (AdManager._instance.LaunchInterstitialState())
                {
                    Debug.Log("IXD Check");
                    adReady = true;
                }
                currentTime = progress;
            }
            if (adReady)
                progress += .2f;
            else
                progress += Time.deltaTime;


            if (progress < 3) loadingText.text = "loading assets...";
            else if (progress > 3 && progress < 6) loadingText.text = "loading player data...";
            else if (progress > 6 && progress < 9) loadingText.text = "loading scene...";
            else if (progress > 9) loadingText.text = "starting ...";

            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (adReady || editorTest || progress >= timetoload) 
        {
            Debug.Log("IXD Ready");
            AdManager._instance.ShowLaunchInterstitial();

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //Internet not connected
                //if (FirebaseEvents.instance != null)
                //{
                //    FirebaseEvents.instance.LogFirebaseEvent("Internet_Not_Connected");
                //}
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}
