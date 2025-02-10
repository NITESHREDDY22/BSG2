using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class MainMenuScript : MonoBehaviour
{
    public GameObject ExitPanel,storePanel;
    // Use this for initialization
    void Start()
    {
        /*
        try
        {
            if (GifAdsManager.Instance._adObjs[0] != null)
            {
                if (GifAdsManager.Instance._adObjs[0]._Adtype == ADType.Native && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[0]);
                }
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                //if (FirebaseEvents.instance != null)
                {
                   // FirebaseEvents.instance.LogFirebaseEvent("Exception", "Mainmenuscript", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
        */
        
    }
    private void OnEnable()
    {
        if (AdManager._instance)
        {
            checkIntenetConnection();
            InternetValidator.Instance.OnInterNetCheck += checkIntenetConnection;
            if (InappManager.Instance)
            {               
                InappManager.Instance.CheckPremiumPopup();                
            }
        }
    }

    private void OnDisable()
    {
       InternetValidator.Instance.OnInterNetCheck -= checkIntenetConnection;
    }

    void checkIntenetConnection(bool status = false)
    {
        InternetValidator.Instance?.CheckNoInterNetPopup();
    }

    public void LevelSelection()
    {
        //GifAdsManager.Instance.HideAD(GifAdsManager.Instance._adObjs[0]);
        SoundManager.PlaySFX("Click2");
        SceneManager.LoadScene("LevelSelection");
    }

    public void Exit()
    {
        AdManager._instance.ShowExitInterstitial();
        ExitPanel.SetActive(true);
        if (AdManager._instance.enableBanner)
        {
            AdManager._instance.showbannerExit();
        }
       // GifAdsManager.Instance.HideAD(GifAdsManager.Instance._adObjs[0]);
    }

    public void okBtn()
    {
        Application.Quit();
    }

    public void cancelBtn()
    {
        ExitPanel.SetActive(false);
        AdManager._instance.hidebannerExit();
       // GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[0]);
    }

    public void rateus()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.knockdown.bottleshootgame");
    }
    public void back()
    {
        SceneManager.LoadScene("MainMenu");
      //  GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[0]);
    }


    public void StorePanel()
    {

        if (FindObjectOfType<StoreManager>().gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            AdManager._instance.HideStorePanel();
        }
        else
        {
            AdManager._instance.ShowStorePanel();

            StoreManager._instance.ScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            StoreManager._instance.ScrollContent2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            FirebaseEvents.instance.LogFirebaseEvent("StoreBtn_clicked_MainMenu");
            GifAdsManager.Instance.HideAD(GifAdsManager.Instance._adObjs[0]);

        }

        FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }

 

}
