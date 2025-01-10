using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Common;

//using AudienceNetwork;
//using GoogleMobileAdsMediationTestSuite.Api;

public class AdManager : MonoBehaviour //, IUnityAdsListener
{
    public static AdManager _instance;
    public enum TargetPlatform { Android, IOS }
    public TargetPlatform targetPlatform;
    public float currentAdDisplayTime = 0;
    public float lastAdDisplayTime = 0;


    [Header(" Settings ")]
    public bool testMode;
    public int GOFAdInterval =0;
    public int GOWAdInterval =0;
    public float ReplayAdInterval = 60f;
    
    public bool enableBanner;
    public bool enableGreedy;

    #region Admob

    [Header(" Android ")]
    public static string interstitialId = "ca-app-pub-3411062052281263/3692188687";
    public string bannerId;
    public string rewardedId;

    [Header(" iOS ")]
    public string iOS_interstitialID;
    public string iOS_bannerID;
    public string iOS_rewardedId;

    public InterstitialAd interstitial,launchInterstitial,exitInterstitial;
    public BannerView bannerView, bannerViewExit;
    public RewardedAd rewardBasedVideo;

    #endregion

    #region Unity Ads

    [Header(" Game ID ")]
    public bool enableUnityAds;
    public string androidGameID;
    
    //public string iosGameID;
    public string androidRewardedVideoID;
    public string iosRewardedVideoID;
    public bool unityRewardReady = false;

    //public Text Logger;

    #endregion
    [Header("Params")]
    int counter = 0;
    int counter2 =0;
    public RewardType rewardTypeToUnlock = RewardType.None;
    public bool rewardedvideosuccess = false;
    
    [Header("Loading Panel")]
    public GameObject LoadingPanel;
    [Header("Store Panel")]
    public GameObject StorePanel;

    ////fb ads
    //private AudienceNetwork.InterstitialAd fbInterstitialAd;
    //private bool isFBLoaded;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();

        //Debug.Log("LOGISDISPLAYED");
        try
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != this)
            {
                DestroyImmediate(this.gameObject);
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_Awake", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    private IEnumerator Start()
    {

        if (!Global.loadedFromServer)
        {

            string url = "https://puzzle-games-d9a78.firebaseapp.com/bsg2_prams.json";
            WWW www = new WWW(url);
            yield return www;
            try
            {
                if (www.error == null)
                {

                    GameConfig config = new GameConfig();

                    config = JsonUtility.FromJson<GameConfig>(www.text);
                    Debug.LogWarning(www.text);
                    Global.isBannerEnabled = config.isBannerEnabled;
                    Global.isIntersitialsEnabled = config.isIntersitialsEnabled;
                    Global.isRewaredAdsEnabled = config.isRewaredAdsEnabled;
                    //Global.isNativeAdsEnabled = config.isNativeAdsEnabled;
                    AdManager._instance.GOFAdInterval = config.GOFAdInterval;
                    AdManager._instance.GOWAdInterval = config.GOWAdInterval;
                    Global.backFillAdGapToContinue = config.backFillAdGapToContinue;
                    Global.World2ReqStars = config.World2ReqStars;
                    Global.World3ReqStars = config.World3ReqStars;
                    Global.World4ReqStars = config.World4ReqStars;
                    Global.World5ReqStars = config.World5ReqStars;
                    AdManager._instance.enableBanner = Global.isBannerEnabled;
                }
                else
                {
                    Debug.Log("ERROR: " + www.error);
                }
                Global.loadedFromServer = true;


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
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_Start", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
        }
            //if (!Advertisement.isInitialized && Advertisement.isSupported)
            //{
            //    Advertisement.Initialize(androidGameID, testMode, this);
            //}
            /*//testsuite
            MediationTestSuite.AdRequest = new AdRequest.Builder()
                .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
                .Build();
            */

            //fb intialise
            //AudienceNetworkAds.Initialize();
            //AdSettings.AddTestDevice("07b03dd5-2c63-4b49-bb75-4c5ad7068bb6");
            //LoadFBInterstitial();
            yield return new WaitForSeconds(1f);
        try
        {
            Debug.Log("AdMANAGE START MEthod --------------------------------------------------------------------------------");
            if (Global.isIntersitialsEnabled)
            {
                RequestInterstitial();
                RequestLaunchInterstitial();
                RequestExitInterstitial();
            }
            if (enableBanner)
            {
                RequestBanner();
                RequestBannerExit();
            }
            if (Global.isRewaredAdsEnabled)
            {
                this.RequestRewardBasedVideo();
            }
            FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        }catch(Exception e)
        {
            //
        }

    }
    //public void OnInitializationComplete()
    //{
    //    Debug.Log("Unity Ads initialization complete.");
    //}

    //public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    //{
    //    Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    //}
    // Start is called before the first frame update

    /*
    public void LoadFBInterstitial()
    {
        //string testfbid = "VID_HD_16_9_15S_APP_INSTALL#{366962342180685_366963662180553}";
        //this.fbInterstitialAd = new AudienceNetwork.InterstitialAd(testfbid);
        this.fbInterstitialAd = new AudienceNetwork.InterstitialAd("366962342180685_366963662180553");
        this.fbInterstitialAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.fbInterstitialAd.InterstitialAdDidLoad = (delegate () {
            Debug.Log("Interstitial ad loaded.");
            this.isFBLoaded = true;
        });
        fbInterstitialAd.InterstitialAdDidFailWithError = (delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
        });
        fbInterstitialAd.InterstitialAdWillLogImpression = (delegate () {
            Debug.Log("Interstitial ad logged impression.");
        });
        fbInterstitialAd.InterstitialAdDidClick = (delegate () {
            Debug.Log("Interstitial ad clicked.");
        });

        this.fbInterstitialAd.interstitialAdDidClose = (delegate () {
            Debug.Log("Interstitial ad did close.");
            if (this.fbInterstitialAd != null)
            {
                this.fbInterstitialAd.Dispose();
            }
        });

        // Initiate the request to load the ad.
        this.fbInterstitialAd.LoadAd();
    }
    */

    public void ShowLoadingPanel()
    {
        LoadingPanel.SetActive(true);
    }
    public void HideLoadingPanel()
    {
        LoadingPanel.SetActive(false);
    }

    public void ShowStorePanel()
    {
        //FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        StorePanel.SetActive(true);
    }
    public void HideStorePanel()
    {
        StorePanel.SetActive(false);
    }

    public void storeBack()
    {
        if (SceneManager.GetActiveScene().name.Contains("GamePlay"))
        {
            HideStorePanel();
        }
        else
        {
           /* try
            {
                GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[0]);
            }
            catch (Exception e)
            { }
           */
            HideStorePanel();
            // SceneManager.LoadScene("MainMenu");
        }
    }

    /*
    public void ShowFBInterstitial()
    {
        if (this.isFBLoaded)
        {
            this.fbInterstitialAd.Show();
            this.isFBLoaded = false;

        }
        else
        {
            Debug.Log("Interstitial Ad not loaded!");
        }
    }
    */

    public void ShowCommonInterstitial()
    {
        Debug.Log("Increase Interstitial Counter");
        try
        {
            if (interstitial.IsLoaded())
            {
                ShowInterstitial();
            }
            /*
            else
            {
                try
                {
                    
                    if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
                    {
                        GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                        lastAdDisplayTime = Time.time;
                        bannerView.Hide();
                    }
                }
                catch (Exception e)
                { }
            }
            
            else
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
                lastAdDisplayTime = Time.time;

            }*/
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowCommonInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }

    public void ShowGameFailInterstitial()
    {
        Debug.Log("Increase Interstitial Counter");
        counter++;
        try
        {
            if (interstitial.IsLoaded())
            {
                if (counter >= GOFAdInterval)
                {
                    ShowInterstitial();
                    counter = 0;
                }
            }
            /*
            else
            {
                try
                {
                    if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
                    {
                        GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                        lastAdDisplayTime = Time.time;
                        bannerView.Hide();
                    }
                }
                catch (Exception e)
                { }
            }
            else
                if (Advertisement.IsReady())
            {
                Advertisement.Show();
                lastAdDisplayTime = Time.time;

            }*/
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowFailInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }


    public void ShowGameWinInterstitial()
    {
        Debug.Log("Increase Interstitial Counter");
        counter2++;
        try
        {
            if (interstitial.IsLoaded())
            {
                if (counter2 >= GOWAdInterval)
                {
                    ShowInterstitial();
                    counter2 = 0;
                }
            }
            /*
            else if (GifAdsManager.Instance._adObjs.Length > 1)
            {
                if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
                {
                    if (counter2 >= GOWAdInterval)
                    {
                        GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                        lastAdDisplayTime = Time.time;
                        bannerView.Hide();
                        counter2 = 0;
                    }
                }
            }
            else
                if (Advertisement.IsReady())
            {
                Advertisement.Show();
                lastAdDisplayTime = Time.time;

            }*/
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowGameWinInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
    // Update is called once per frame
    #region Unity Ads

    public void ShowInterstitial()
    {
        try
        {
            if (this.interstitial != null && this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
                lastAdDisplayTime = Time.time;
            }
            /*
            else
            if (Advertisement.IsReady() && enableUnityAds)
            {
                Advertisement.Show();
                lastAdDisplayTime = Time.time;

            }
            else if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
            {
                GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                lastAdDisplayTime = Time.time;
                bannerView.Hide();
            }
            */
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void ShowLaunchInterstitial()
    {
        try
        {
            if (this.launchInterstitial != null && this.launchInterstitial.IsLoaded())
            {
                this.launchInterstitial.Show();
                lastAdDisplayTime = Time.time;
            }
            /*
            else
            if (Advertisement.IsReady() && enableUnityAds)
            {
                Advertisement.Show();
                lastAdDisplayTime = Time.time;

            }
            
            else if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
            {
                GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                lastAdDisplayTime = Time.time;
                bannerView.Hide();
            }*/
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowLaunchInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }

    public void ShowExitInterstitial()
    {
        try
        {
            if (this.exitInterstitial != null && this.exitInterstitial.IsLoaded())
            {
                this.exitInterstitial.Show();
               // lastAdDisplayTime = Time.time;
            }
            /*
            else
            if (Advertisement.IsReady() && enableUnityAds)
            {
                Advertisement.Show();
                //lastAdDisplayTime = Time.time;

            }
            
            else if (GifAdsManager.Instance._adObjs[1]._Adtype == ADType.Interstitial)
            {
                GifAdsManager.Instance.ShowAd(GifAdsManager.Instance._adObjs[1]);
                lastAdDisplayTime = Time.time;
                bannerView.Hide();
            }
            */
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
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowExitInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }


    public void ShowRewardedVideo()
    {
        string rewardedVideoID = androidRewardedVideoID;

#if UNITY_IOS
        rewardedVideoID = iosRewardedVideoID;
#endif


       
        try
        {
            if (rewardBasedVideo.IsLoaded())
            {
                ShowAdmobRewardedVideo();
            }
            /*
            else
            if(Advertisement.IsReady(rewardedVideoID) && enableUnityAds)
            {
                Advertisement.Show(rewardedVideoID);
            }*/


        }
        catch (Exception e)
        { }



    }
    
    #endregion

    #region AdMob

    public void RequestInterstitial()
    {
         string adUnitId;
        if (testMode)
        {
        #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-3940256099942544/1033173712";
                   
        #elif UNITY_IOS
                       adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
                       adUnitId = "unexpected_platform";
        #endif
                }
                else if(enableGreedy)
                {
        #if UNITY_ANDROID
                    adUnitId = "/419163168/com.knockdown.bottleshootgame.interstitial";
        #elif UNITY_IOS
                    adUnitId = iOS_interstitialID;
        #else
                    adUnitId = "unexpected_platform";
        #endif

                }
                else
                {
        #if UNITY_ANDROID
                    adUnitId = interstitialId;
        #elif UNITY_IOS
                    adUnitId = iOS_interstitialID;
        #else
                    adUnitId = "unexpected_platform";
        #endif
        }
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        //this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        //this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void RequestLaunchInterstitial()
    {
        string adUnitId;
        if (testMode)
        {
        #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IOS
                       adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
                       adUnitId = "unexpected_platform";
        #endif
        }
        else
        {
        #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-3411062052281263/3169444505";
        #elif UNITY_IOS
                    adUnitId = iOS_interstitialID;
        #else
                    adUnitId = "unexpected_platform";
        #endif
        }
        // Initialize an InterstitialAd.
        launchInterstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        //this.launchInterstitial.OnAdLoaded += HandleOnAdLoadedLaunch;
        // Called when an ad request failed to load.
        //this.launchInterstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadLaunch;
        // Called when an ad is shown.
        //this.launchInterstitial.OnAdOpening += HandleOnAdOpenedLaunch;
        // Called when the ad is closed.
        this.launchInterstitial.OnAdClosed += HandleOnAdClosedLaunch;
        // Called when the ad click caused the user to leave the application.
        //this.launchInterstitial.OnAdLeavingApplication += HandleOnAdLeavingApplicationLaunch;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();


         //Load the interstitial with the request.
        launchInterstitial.LoadAd(request);
    }

    public void RequestExitInterstitial()
    {
        string adUnitId;
        if (testMode)
        {
            #if UNITY_ANDROID
                        adUnitId = "ca-app-pub-3940256099942544/1033173712";
            #elif UNITY_IOS
                           adUnitId = "ca-app-pub-3940256099942544/4411468910";
            #else
                           adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                        adUnitId = "ca-app-pub-3411062052281263/8827945071";
            #elif UNITY_IOS 
                        adUnitId = iOS_interstitialID;
            #else
                        adUnitId = "unexpected_platform";
            #endif
        }
        // Initialize an InterstitialAd.
        exitInterstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        //this.exitInterstitial.OnAdLoaded += HandleOnAdLoadedExit;
        // Called when an ad request failed to load.
        //this.exitInterstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadExit;
        // Called when an ad is shown.
        //this.exitInterstitial.OnAdOpening += HandleOnAdOpenedExit;
        // Called when the ad is closed.
        this.exitInterstitial.OnAdClosed += HandleOnAdClosedExit;
        // Called when the ad click caused the user to leave the application.
        //this.exitInterstitial.OnAdLeavingApplication += HandleOnAdLeavingApplicationExit;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();


        // Load the interstitial with the request.
        exitInterstitial.LoadAd(request);
    }


    private void RequestBanner()
    {
        string adUnitId;
        if (testMode)
        {
        #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-3940256099942544/6300978111";
            
        #elif UNITY_IPHONE
                    adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
                    adUnitId = "unexpected_platform";
        #endif
                }
                else if(enableGreedy)
                {
        #if UNITY_ANDROID
                    adUnitId = "/419163168/com.knockdown.bottleshootgame.banner";
        #elif UNITY_IPHONE
                    adUnitId = iOS_bannerID;
        #else
                    adUnitId = "unexpected_platform";
        #endif
        }
        else
        {
        #if UNITY_ANDROID
                      adUnitId = bannerId;
                   // adUnitId = "ca-app-pub-3940256099942544/6300978111"; // test id
        #elif UNITY_IPHONE

                    adUnitId = iOS_bannerID;
        #else
                    adUnitId = "unexpected_platform";
        #endif
                }

        // Create a 320x50 banner at the top of the screen.
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this.bannerView = new BannerView(adUnitId,AdSize.SmartBanner , AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        bannerView.OnAdLoaded += HandleOnBannerLoaded;
       // bannerView.Show();
       // bannerView.Hide();
    }

    private void RequestBannerExit()
    {
        string adUnitId;
        if (testMode)
        {
            #if UNITY_ANDROID
                        adUnitId = "ca-app-pub-3940256099942544/6300978111";

            #elif UNITY_IPHONE
                                adUnitId = "ca-app-pub-3940256099942544/2934735716";
            #else
                                adUnitId = "unexpected_platform";
            #endif
        }
        else if (enableGreedy)
        {
            #if UNITY_ANDROID
                        adUnitId = "/419163168/com.knockdown.bottleshootgame.banner";
            #elif UNITY_IPHONE
                                adUnitId = iOS_bannerID;
            #else
                                adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                        adUnitId = "ca-app-pub-3411062052281263/6044245894";
                        // adUnitId = "ca-app-pub-3940256099942544/6300978111"; // test id
            #elif UNITY_IPHONE

                                adUnitId = iOS_bannerID;
            #else
                                adUnitId = "unexpected_platform";
            #endif
        }

        // Create a 320x50 banner at the top of the screen.
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this.bannerViewExit = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerViewExit.LoadAd(request);
        bannerViewExit.OnAdLoaded += HandleOnBannerLoadedExit;
        // bannerView.Show();
        // bannerView.Hide();
    }

    public void HandleOnBannerLoadedExit(object sender, EventArgs args)
    {
        try
        {
            if (bannerViewExit != null)
            {
                bannerViewExit.Hide();
            }
        }
        catch (Exception e)
        { }
    }



    public void HandleOnBannerLoaded(object sender, EventArgs args){
        try
        {
            if (bannerView != null)
            {
                bannerView.Hide();
            }
        }
        catch (Exception e)
        { }
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
        });
    }

    public void showbanner(){
        if (enableBanner)
        {
            if (bannerView != null)
            {
                try
                {
                    bannerView.Show();
                }
                catch (Exception e)
                { }
            }
        }
    }
    public void hidebanner(){
        if(bannerView!=null){
            try
            {
                bannerView.Hide();
            }
            catch (Exception e)
            { }
        }
    }

    public void showbannerExit()
    {
        if (enableBanner)
        {
            if (bannerViewExit != null)
            {
                try
                {
                    bannerViewExit.Show();
                }
                catch (Exception e)
                { }
            }
        }
    }
    public void hidebannerExit()
    {
        if (bannerViewExit != null)
        {
            try
            {
                bannerViewExit.Hide();
            }
            catch (Exception e)
            { }
        }
    }


    public void RequestRewardBasedVideo()
    {
        string adUnitId;


        if (testMode)
        {
        #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
                    adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
                    adUnitId = "unexpected_platform";
        #endif
                }
                else
                {
        #if UNITY_ANDROID
                    adUnitId = rewardedId;
        #elif UNITY_IPHONE
                    adUnitId = iOS_rewardedId;
        #else
                    adUnitId = "unexpected_platform";
        #endif
        }

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = new RewardedAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardBasedVideo.LoadAd(request);
        // Called when an ad request has successfully loaded.
        //rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        //rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        // Load the rewarded video ad with the request.
    }

    private void ShowAdmobRewardedVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardedvideosuccess = false;
           
            try
            {
                rewardBasedVideo.Show();
            }
            catch (Exception e)
            { }
        }
    }

    #region Rewarded Video Callbacks

    // Admob reward callbacks
    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        try
        {
            GameManager.Instance.gameState = GameState.Reward_Video_Started;
        }
        catch (Exception e)
        { }
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            //GameManager.Instance.gameState = GameState.Reward_Video_Started;
        });
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        try
        {
            GameManager.Instance.gameState = GameState.Reward_Video_Completed;
            onreward();
            GameManager.Instance.ingamevideosuccess();
            this.RequestRewardBasedVideo();
        }
        catch (Exception e)
        { }
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            //
        });
        //FindObjectOfType<GameManager>().onrewardvideoSuccess();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        try
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("rewardhandle___log");
            rewardedvideosuccess = true;
        }
        catch (Exception e)
        { }
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            ///
        });
    }

    void OnApplicationFocus(bool hasFocus)
    {
        try
        {
            if (hasFocus)
            {
                onreward();
            }
        }
        catch (Exception e)
        { }

    }

    public void onreward()
    {
        if (rewardedvideosuccess)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelSelection")
            {
                FindObjectOfType<SpinWheel>().OnWatchVideoSuccess();
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            {

                FindObjectOfType<StoreManager>().onRewardVideoSuccess();

            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 2)

            {
                if (FindObjectOfType<StoreManager>().RewardPanel.activeInHierarchy)
                {
                    FindObjectOfType<StoreManager>().onRewardVideoSuccess();
                }

            }
        }
    }
    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {

    }
    /*
    //Unity reward callbacks

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("rewardhandle Unity___log");
            rewardedvideosuccess = true;
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            GameManager.Instance.gameState = GameState.Reward_Video_Completed;
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
            Debug.Log("rewardhandle Unity___log");
            rewardedvideosuccess = true;
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == androidRewardedVideoID)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            unityRewardReady = true;
        }
        else
        {
            unityRewardReady = false;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        GameManager.Instance.gameState = GameState.Reward_Video_Started;
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        //Advertisement.RemoveListener(this);
    }

    */



#endregion


    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            try
            {
                interstitialId = "ca-app-pub-3411062052281263/2087800960";
            }
            catch (Exception e)
            { }
        });
    }



    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            try
            {
                if (Global.isIntersitialsEnabled)
                {
                    RequestInterstitial();
                }
            }
            catch (Exception e)
            { }
        });
    }

    public void HandleOnAdClosedLaunch(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            try
            {
                if (Global.isIntersitialsEnabled)
                {
                    RequestLaunchInterstitial();
                }
            }
            catch (Exception e)
            { }
        });
    }


    public void HandleOnAdClosedExit(object sender, EventArgs args)
    { 
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            try
            {
                if (Global.isIntersitialsEnabled)
                {
                    RequestExitInterstitial();
                }
            }
            catch (Exception e)
            { }
        });
    }

    

    #endregion
}

[Serializable]
public enum RewardType {
    doublereward =0,
    extraball =1,
    AvailDoubleDaily=2,

    skiplevel =3,
    None=5,
   store =6,
   continuegame =7
}


