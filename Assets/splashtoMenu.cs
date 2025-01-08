using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class splashtoMenu : MonoBehaviour {

    public Image progressslider;
    public int timetoload = 2;
    void Start()
    {
     //PlayerPrefs.DeleteAll();
        StartCoroutine(fadAction_(0.1f));
    }
    IEnumerator fadAction_(float t)
    {
        yield return new WaitForSeconds(t);
        float progress = 0;
        while (progress<timetoload)
        {
            yield return new WaitForSeconds(0.01f);
            progress += 0.01f;
            progressslider.fillAmount = (progress/timetoload);
            yield return null;
        }
        AdManager._instance.ShowLaunchInterstitial();
        SceneManager.LoadScene("MainMenu");
    }
}
