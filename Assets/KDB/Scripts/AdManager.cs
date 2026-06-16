using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//using com.unity3d.mediation;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;
using GoogleMobileAds.Common;
using System.Net.Http.Headers;
using System.Linq;
using System.Net.NetworkInformation;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Mediation.AppLovin.Api;
using GoogleMobileAds.Mediation.DTExchange.Api;
using GoogleMobileAds.Mediation.IronSource.Api;
using GoogleMobileAds.Ump.Api;
using System.Security.Cryptography;
using System.Text;

//using AudienceNetwork;
//using GoogleMobileAdsMediationTestSuite.Api;

public class AdManager : MonoBehaviour //, IUnityAdsListener
{
    public static AdManager _instance;
    public enum TargetPlatform { Android, IOS }
    public TargetPlatform targetPlatform;
    public float currentAdDisplayTime = 0;
    public float lastAdDisplayTime = 0;
    public readonly float levelReloadAdDuration = 60;


    [Header(" Settings ")]
    public bool testMode;
    public int GOFAdInterval =0;
    public int GOWAdInterval =0;
    public float ReplayAdInterval = 60f;
    
    public bool enableGreedy;

  

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
    public StoreManager storeManager;

    public GameObject LoadingPanelForBanner;
    public GameObject DummyLoadingPanelForBanner;

    public Image loadingFillBar;




    /*
    #region Admob
    [Header(" Android ")]
    //public static string interstitialId = "ca-app-pub-3411062052281263/3692188687";
    public string adMobInterstitialId;
    public bool useAdMobInterStitial;

    public string adMobLaunchinterStitialId;
    public bool useLaunchAdMobInterStitial;
    // public string admobexitInterstitialId;
    public string adMobRewardBasedVideoId;
    public bool useadMobRewardBasedVideo;

    public string adMobContinueModelRewardBasedVideoId;
    public bool useadMobContinueRewardBasedVideo;
    public string bannerId;

    [Header(" iOS ")]
    public string iOS_interstitialID;
    public string iOS_bannerID;
    public string iOS_rewardedId;


    #endregion

    ////fb ads
    //private AudienceNetwork.InterstitialAd fbInterstitialAd;
    //private bool isFBLoaded;
   // [SerializeField] string bannerAdUnitId = "thnfvcsog13bhn08";
    [SerializeField] string interstitialAdUnitId = "z8axy0332hnr585z";
    public bool useLevelPlayInterStital;
    [SerializeField] string rewardAdUnitId = "hgncqhneupu7bppt";
    public bool useLevelPlayReward;
    [SerializeField] string LaunchinterstitialAdUnitId = "68ozdvrgw2w8rw44";
    public bool useLaunchInterStitial;
    */

    private bool isAdMobInitialized;    
    private string appKey = "1ab7561b5";

    public AdMobNetworkHandler adMobNetworkHandler;
    public LevelPlayNetworkHandler levelPlayNetworkHandler;
    //public HybidNetworkHandler hybidNetworkHandler;
    public AdsConfig AdsConfiguration;

    [Space(10)]
    [Header("ADs display for every level after adIntervalLevelCheck reached")]
    public int adIntervalLevelCheck = 30;
    public static bool onlyOnce=false;
    public static Action<GameConfig> OnConfigLoaded;
    [SerializeField]private int gapBetweenAds=2;
    [SerializeField]private int gapBetweenAdsSecondary=3;
    public static Action OnIngameAdClosed;
    public bool isLaunchInterstitialEnabled = true;


    private bool isLoadingInTransit = false;
    private int bannerAdShowLevelFrom = 3;
    public DateTime lastAdShownDateTime;
    public bool isLaunchAdShown;

    private bool isHybidEnabled=false;

    [Header("GDPR Settings")]
    [SerializeField] private Text debugIdText;
    private bool isConsentProcessed = false;

    [SerializeField] private bool testGDPRInEditor = false;

    [Header("Mediation Settings")]
    public bool enableMediation = false; // Set to false to only use pure AdMob
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
        
        Initialize();
        
       
        lastAdDisplayTime = Time.time;
        lastAdShownDateTime = DateTime.UtcNow;
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }

    private IEnumerator Start()
    {
         HideLoadingPanel();
        Debug.Log($"loadedFromServer:{Global.loadedFromServer}");
        if (!Global.loadedFromServer)
        {
            Debug.Log($"{Application.internetReachability}");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogWarning("AdManager: no internet connection, skipping remote config fetch.");
                Global.loadedFromServer = true;
            }
            else
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
                    Debug.Log("config data" + www.text);
                    //Global.isNativeAdsEnabled = config.isNativeAdsEnabled;
                    GOFAdInterval = config.GOFAdInterval;
                    GOWAdInterval = config.GOWAdInterval;
                    Global.backFillAdGapToContinue = config.backFillAdGapToContinue;
                    Global.World2ReqStars = config.World2ReqStars;
                    Global.World3ReqStars = config.World3ReqStars;
                    Global.World4ReqStars = config.World4ReqStars;
                    Global.World5ReqStars = config.World5ReqStars;
                    gapBetweenAds = config.FIRST_LVLS_SET_AD_GAP;
                    gapBetweenAdsSecondary = config.SECOND_LVLS_SET_AD_GAP;
                    isLaunchInterstitialEnabled = config.showLaunchAd;
                    Global.isLaunchInterstitialEnabled = config.showLaunchAd;
                    Global.InterstitialAdGap = config.InterstitialAdGap;
                    Global.isSingularEnabled = config.isSingularEnabled;
                    Global.isBannerEnabled = config.isBannerEnabled;
                    Global.isIntersitialsEnabled = config.isIntersitialsEnabled;
                    Global.isRewaredAdsEnabled = config.isRewaredAdsEnabled;
                    Global.isAppOpenAdEnabled = config.isAppOpenAdEnabled;
                    Global.adRetryTime = config.adRetryTime;
                    bannerAdShowLevelFrom = config.showBannerFrom;
                    Global.coinsToReload = config.coinsToReload;
                    Global.defaultCoins = config.defaultCoins;
                    Global.tragectoryChallenge = config.tragectoryChallenge;
                    NotEnoughCoinsPopup.rewardCoins = config.notEnoughRewardCoins;
#if UNITY_EDITOR
                    //Global.coinsToReload = 0; // For test
                    Global.tragectoryChallenge = true;
#endif
#if CHEATS_ON
                    Global.tragectoryChallenge = true;
#endif
                    CustomAdManager.adsEnabled = config.customAdsEnabled;
                    CustomAdManager.showGameZopInterstitials = config.showGameZopInterstitials;
                    OnConfigLoaded?.Invoke(config);
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
            //StartCoroutine(InitializeAdNetworks());
            Debug.Log($"isEditor?{Application.isEditor},testGDPRInEditor?{testGDPRInEditor}");
            if (Application.isEditor && !testGDPRInEditor)
            {
                StartCoroutine(InitializeAdNetworks());
            }
            else
            {
                StartCoroutine(GatherConsentAndInit());
            }
            SetDefaultData();
        try
        {


            FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        }
        catch (Exception e)
        {
            //
        }
    }
    }

    public bool IsConsentGatheringFinished { get; private set; } = false;
    private IEnumerator GatherConsentAndInit()
{
    IsConsentGatheringFinished = false; 
    Debug.Log("<color=yellow>[GDPR]</color> Entering GatherConsentAndInit...");

    FireBaseActions("GDPR_Process", "Status", "Started");

    // 1. Prepare Parameters
    ConsentRequestParameters requestParameters;

#if GDPR_TEST_ON
    // TEST MODE LOGIC
    Debug.Log("<color=cyan>[GDPR TEST]</color> Test Mode Active. Resetting consent and setting EEA geography.");
    ConsentInformation.Reset(); // Force the form to appear for testing purposes
    
    string testId = GetAdMobHashedDeviceId();
    if (debugIdText != null) debugIdText.text = $"Test ID: {testId}";

    var debugSettings = new ConsentDebugSettings
    {
        DebugGeography = DebugGeography.EEA, // Force EEA behavior
        TestDeviceHashedIds = new List<string> { testId }
    };

    requestParameters = new ConsentRequestParameters
    {
        TagForUnderAgeOfConsent = false,
        ConsentDebugSettings = debugSettings
    };
#else
    // PRODUCTION MODE LOGIC
    // In production, we provide no debug settings.
    requestParameters = new ConsentRequestParameters
    {
        TagForUnderAgeOfConsent = false
    };
#endif

    bool updateCompleted = false;
    
    // 2. Request Consent Info Update
    ConsentInformation.Update(requestParameters, (FormError error) =>
    {
        if (error != null)
        {
            Debug.LogError($"<color=red>[GDPR Error]</color> Update Failed: {error.ErrorCode} - {error.Message}");
            FireBaseActions("GDPR_Error", "Step", "Update_Failed");
            AdTestToast.Instance?.Show($"GDPR Error: {error.ErrorCode}");
            updateCompleted = true;
            return;
        }

        // 3. Check Privacy Requirement Status (For Analytics)
        string userRegion = "Unknown";
        if (ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required)
            userRegion = "EEA_Regulated";
        else if (ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.NotRequired)
            userRegion = "Non_EEA";

        FireBaseActions("GDPR_Region_Detected", "Region", userRegion);

        // 4. Handle the Form
        if (ConsentInformation.IsConsentFormAvailable())
        {
            // IMPORTANT: GDPR Form is a native overlay. 
            // If your LoadingPanel is high depth, it might block interaction.
            if(LoadingPanel != null) LoadingPanel.SetActive(false);
            AdTestToast.Instance?.Show("GDPR: Showing Form...");
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                if (formError != null)
                {
                    Debug.LogError($"<color=red>[GDPR Error]</color> Form Show Failed: {formError.Message}");
                    FireBaseActions("GDPR_Error", "Step", "Show_Failed");
                    AdTestToast.Instance?.Show("GDPR Form Show Failed");
                }
                else
                {
                    bool hasConsent = (ConsentInformation.ConsentStatus == ConsentStatus.Obtained);
                    FireBaseActions("GDPR_User_Choice", "Consented", hasConsent.ToString());
                    AdTestToast.Instance?.Show("GDPR Form Processed");
                }
                updateCompleted = true;
            });
        }
        else
        {
            Debug.Log("<color=green>[GDPR]</color> Form not required for this user.");
            AdTestToast.Instance?.Show("GDPR: Form Not Required");
            updateCompleted = true;
        }
    });

    // 5. Safety Timeout (Wait max 5 seconds for UMP to respond)
    float timeoutCounter = 0;
    while (!updateCompleted && timeoutCounter < 15f) 
    {
        timeoutCounter += 0.1f;
        yield return new WaitForSeconds(0.1f);
    }

    IsConsentGatheringFinished = true;

    // 6. Final check: Can we show ads?
    if (ConsentInformation.CanRequestAds())
    {
        Debug.Log("<color=green>[GDPR]</color> Initializing Ad Networks...");
        AdTestToast.Instance?.Show("Ads Init: Starting SDK...");
        StartCoroutine(InitializeAdNetworks());
    }
    else
    {
        FireBaseActions("GDPR_Process", "Status", "Ads_Blocked_By_User");
        AdTestToast.Instance?.Show("Ads Init: BLOCKED by User Consent");
        Debug.LogWarning("<color=orange>[GDPR]</color> Consent denied or not yet obtained. Ads will not initialize.");
    }
}

    private string GetAdMobHashedDeviceId()
    {
        string deviceId = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        try {
            using (var clsSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
            {
                using (var clsUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var objActivity = clsUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        var objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                        deviceId = clsSettingsSecure.CallStatic<string>("getString", objResolver, "android_id");
                        Debug.Log($"<color=cyan>[GDPR TEST]</color> Native Android ID retrieved: {deviceId}");
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogError("Failed to get Native Android ID: " + e.Message);
            deviceId = SystemInfo.deviceUniqueIdentifier;
        }
#else
        deviceId = SystemInfo.deviceUniqueIdentifier;
        Debug.Log($"<color=cyan>[GDPR TEST]</color> Using SystemInfo ID: {deviceId}");
#endif

        string hashed = GetMD5Hash(deviceId);
        return hashed;
    }
    private string GetMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2")); // Hexadecimal uppercase
            }
            return sb.ToString();
        }
    }

    private static void SetDefaultData()
    {
        if (!PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", Global.defaultCoins);
        }
    }

    public void ShowLoadingPanel(bool showBannerAdFlag=false)
    {
        Debug.Log("ShowLoadingPanel called");
        LoadingPanel.SetActive(true);
    }
    public void HideLoadingPanel()
    {
        LoadingPanel.SetActive(false);
    }

    Coroutine cacheBannerAd;
    public void ShowLoadingForBanner(float timer,bool canCheckLastAdDisplay=false)
    {
        loadingFillBar.fillAmount = 0;
        LoadingPanelForBanner.SetActive(true);
        Time.timeScale = 1;
        if(cacheBannerAd!=null)
        {
            StopCoroutine(cacheBannerAd);
        }
        cacheBannerAd= StartCoroutine(handleBannerAdLoading(timer, ()=>
        {
            if (canCheckLastAdDisplay)
            {
                //currentAdDisplayTime = Time.time;
              
                if (adDelayMet())
                {
                    ShowAppOpenAd();
                }
            }
            else
            {
                ShowAppOpenAd();
            }
        }));
       
    }

    IEnumerator handleBannerAdLoading(float timerTarget,Action callback)
    {
        float timer = 0;
        float lerpValue = 0;
        float targetTimer = timerTarget-0.15f;
        yield return new WaitForSeconds(0.15f);
        callback?.Invoke();
        while (timer < targetTimer)
        {
            timer+= Time.deltaTime;
            lerpValue = timer / targetTimer;
            loadingFillBar.fillAmount=lerpValue;           
            yield return null;
        }
        HideLoadingForBanner();
    }

    public void HideLoadingForBanner()
    {
        LoadingPanelForBanner.SetActive(false);
        isLoadingInTransit = false;
    }

    public void ShowStorePanel()
    {
        //FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        StorePanel.SetActive(true);
        storeManager.dostorecoinsupdate();
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


    #region ADS REGION
    private bool canShowAd=true;
    void Initialize()
    {
        AdConfig adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
        if (adMobConfig != null)
        {
            adMobNetworkHandler.SetAdConfig(adMobConfig);
            adMobNetworkHandler.Init();
            adMobNetworkHandler.rewardedInterStitialrequestcallBack += CheckSecondaryInterstitialStatus;
            adMobNetworkHandler.rewardedrequestcallBack += CheckSecondaryRewardAdStatus;
        }

        AdConfig levelPlayConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.LevelPlay);
        if (levelPlayConfig != null)
        {
            appKey=levelPlayConfig.AppKey;
            //levelPlayNetworkHandler.SetAdConfig(levelPlayConfig);       
        }
    }

    private bool UserGaveConsent()
    {
        // If we can request ads, it usually means consent was obtained or not required
        // In a strict TCF 2.2 environment, you'd check the ConsentStatus
        return ConsentInformation.ConsentStatus == ConsentStatus.Obtained;
    }

    private bool hasRequestedLaunchAd = false; // The Gate
    private bool usingSecondaryInterstitialId = false;
    private bool usingSecondaryLaunchId = false;
    IEnumerator InitializeAdNetworks()
    {
        hasRequestedLaunchAd = false; // Reset
        Debug.Log("InitializeAdNetworks initialization");

        yield return null;

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{ 
        // --- DYNAMIC GDPR CHECK ---
    bool hasConsent = UserGaveConsent();
    Debug.Log($"<color=green>[GDPR]</color> Setting mediation consent flags to: {hasConsent}");
        if (enableMediation)
        {
            AdTestToast.Instance?.Show("Ads: Mediation Enabled.");
            DTExchange.SetGDPRConsent(hasConsent);
            AppLovin.SetHasUserConsent(hasConsent);
            IronSource.SetConsent(hasConsent);
        }
        else
        {
            AdTestToast.Instance?.Show("Ads: Mediation DISABLED. Pure AdMob mode.");
        }
        // ---------------------------
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            
            Debug.Log("InitializationStatus initialization");

            // This callback is called once the MobileAds SDK is initialized.
            isAdMobInitialized = true;
            AdTestToast.Instance?.Show("AdMob SDK Initialized!");
            adMobNetworkHandler.Initialize(isAdMobInitialized);
            bool hasConsent = UserGaveConsent();

            GoogleMobileAds.Mediation.UnityAds.Api.UnityAds.SetConsentMetaData("gdpr.consent", hasConsent);
            GoogleMobileAds.Mediation.UnityAds.Api.UnityAds.SetConsentMetaData("privacy.consent", hasConsent);

            if (!hasRequestedLaunchAd)
            {
                hasRequestedLaunchAd = true;
                AdTestToast.Instance?.Show("Requesting Launch Ad (via Success)");
                RequestLaunchInterstitial();
            }

            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        AdTestToast.Instance?.Show($"Adapter Fail: {className}");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        AdTestToast.Instance?.Show($"AdMob SDK Initialized! Adapter Success: {className}");
                        break;
                }
            }

            Debug.Log("InitializationStatus initialization 1111");

            /*
            List<string> testDeviceIds = new List<string>();
            testDeviceIds.Add("6FE696550ADF8FAAC961A42C626E43A7");
            RequestConfiguration requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = testDeviceIds
            };
            MobileAds.SetRequestConfiguration(requestConfiguration);
            */

        });

        try
        {
          //  if(isHybidEnabled)
          //  hybidNetworkHandler.Initialize();
        }
        catch
        {

        }
       // });


        float initWaitStart = Time.realtimeSinceStartup;
        const float initWaitTimeout = 5f;
        while (!isAdMobInitialized && Time.realtimeSinceStartup - initWaitStart < initWaitTimeout)
        {
            yield return null;
        }

        if (!isAdMobInitialized)
        {
            Debug.LogWarning("AdManager: MobileAds.Initialize callback did not arrive in time; continuing without ad initialization.");

            string timeoutReason = "Unknown Timeout";
            if (Application.internetReachability == NetworkReachability.NotReachable)
                timeoutReason = "No Internet Connection";
            else
                timeoutReason = "SDK Internal Hang / Google Play Services issue";

            AdTestToast.Instance?.Show($"AdMob Init Fail TIMEOUT! {timeoutReason}");
            isAdMobInitialized = true;
            //FireBaseActions("AdMob_Init_Fail_Timeout", "Reason", timeoutReason);
            if (!hasRequestedLaunchAd)
            {
                hasRequestedLaunchAd = true;
                AdTestToast.Instance?.Show("Strategy: Requesting Launch Ad (via Timeout)");
                RequestLaunchInterstitial();
            }
        }

        //yield return new WaitForSeconds(1);
       
        StartCoroutine(RequestAppOpenAd());        

       
        
        // IronSource.Agent.validateIntegration();
        // Debug.Log("unity-script: unity version" + IronSource.unityVersion());
        // SDK init
        Debug.Log("unity-script: LevelPlay SDK initialization");
            // LevelPlay.Init(appKey, null);

            //LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            //LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
    }

    /*
    void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: " + config);
        //EnableAds();
        levelPlayNetworkHandler.EnableAds();
    }

    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: " + error);
    }
    */
    public void RequestLaunchInterstitial()
    {
        /*
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
        */

        //Debug.Log("Asdf RequestLaunchInterstitial 0000");

        adMobNetworkHandler.RequestInterstitial(AdType.Launch);
        // levelPlayNetworkHandler.RequestInterstitial(AdType.Launch);
        //hybidNetworkHandler.RequestLaunchInterstitial();
        
    }
    public void ShowLaunchInterstitial(bool shownow=false)
    {
        
        try
        {
            if (adDelayMet() || shownow)
            {
                adMobNetworkHandler.ShowInterstitialAd(AdType.Launch, ShowLevelPlayLaunchInterStital);
                void ShowLevelPlayLaunchInterStital(bool flag)
                {
                    Debug.Log($"ShowLaunchInterstitial:{flag}");
                    isLaunchAdShown = flag;
                    if (!flag)
                    {
                        //levelPlayNetworkHandler.ShowInterstitialAd(AdType.Launch, (result)=>
                        //{
                        //    if(result)
                        //    {
                        //        lastAdDisplayTime = Time.time;
                        //    }
                        //});
                        //hybidNetworkHandler.ShowLaunchInterstitial((shown)=>
                        //{
                        //    if(shown)
                        //    {
                        //        lastAdDisplayTime = Time.time;
                        //        lastAdShownDateTime = DateTime.UtcNow;
                        //    }
                        //});
                        if (CustomAdManager.Instance)
                            CustomAdManager.Instance.ShowInterstitial();

                    }
                    else
                    {
                        lastAdDisplayTime = Time.time;
                        lastAdShownDateTime = DateTime.UtcNow;
                    }


                    //Debug.Log("lastAdDisplayTime " + lastAdDisplayTime);
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
    public void RequestInterstitial()
    {
        /*
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
        */

        /*
        GenericRequestInterstitial(adMobInterstitial, adMobInterstitialId, adMobInterstitialReady);
        if (levelPlayInterstitial != null)
        {
            levelPlayInterstitial.OnAdLoadFailed += HandleOnAdFailedToLoad;
            levelPlayInterstitial.OnAdClosed += HandleOnAdClosed;
            levelPlayInterstitial.LoadAd();
        }
        */

        adMobNetworkHandler.RequestInterstitial(AdType.Interstital);
        // levelPlayNetworkHandler.RequestInterstitial(AdType.Interstital);
        //hybidNetworkHandler.RequestInterstitial();
    }

    public void ShowInterstitial(Action<bool> callBack=null)
    {
        try
        {
            if (!canShowAd)
                return;

            adMobNetworkHandler.ShowInterstitialAd(AdType.Interstital, ShowLevelPlayLaunchInterStital);
            void ShowLevelPlayLaunchInterStital(bool flag)
            {
                Debug.Log($"ShowInterstitial:{flag}");
                if (!flag)
                {
                    //levelPlayNetworkHandler.ShowInterstitialAd(AdType.Interstital, (result)=>
                    //{
                    //    callBack?.Invoke(result);
                    //    if(result)
                    //    {
                    //        lastAdDisplayTime = Time.time;
                    //    }
                    //});

                    //hybidNetworkHandler.ShowInterstitial((shown)=>
                    //{
                    //    if (shown)
                    //    {
                    //        lastAdDisplayTime = Time.time;
                    //        lastAdShownDateTime = DateTime.UtcNow;
                    //    }
                    //   callBack?.Invoke(shown);
                    //});
                    if (CustomAdManager.Instance)
                            CustomAdManager.Instance.ShowInterstitial();
                }
                else
                {
                    callBack?.Invoke(true);
                    lastAdDisplayTime = Time.time;
                    lastAdShownDateTime = DateTime.UtcNow;

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

    public void ShowCommonInterstitial(Action<bool> callBack=null)
    {      
        Debug.Log($"Increase ShowCommon Interstitial Counter:{adDelayMet()}");
        try
        {
            if (adDelayMet())
            {
                AdTestToast.Instance?.Show("Logic: Ad Gap Met. Attempting Show...");
                ShowInterstitial((callBack)=>
                {
                    Debug.Log($"ShowCommonInterstitial:{callBack}");
                    if(callBack)
                    {
                        
                    }
                    else
                    {
                        if (CustomAdManager.Instance)
                            CustomAdManager.Instance.ShowInterstitial();
                    }

                });
            }
            else
            {
                double secondsLeft = Global.InterstitialAdGap - (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;
                AdTestToast.Instance?.Show($"Logic: Ad Gap NOT Met ({Mathf.CeilToInt((float)secondsLeft)}s remain)");
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
        //counter++;
        //Debug.Log("Increase Interstitial Counter "+counter + " Get "+GetCounter);
        try
        {

            if (adDelayMet())
            {
                AdTestToast.Instance?.Show("Trigger: Game Fail -> Showing Ad");
                ShowInterstitial((result) =>
                {
                    Debug.Log($"ShowGameFailInterstitial:{result}");
                    if(result)
                    {
                        //counter = 0;
                    }
                    else
                    {
                        if (CustomAdManager.Instance)
                            CustomAdManager.Instance.ShowInterstitial();
                    }
                });
                
            }
            else
            {
                double remaining = Global.InterstitialAdGap - (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;
            AdTestToast.Instance?.Show($"Ad Delay: Fail Ad blocked. Wait {remaining:F1}s.");
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
        //counter2++;
        //counter2++;
        //Debug.Log("Increase Interstitial Counter---" + counter2 + " Get " + GetCounter);

        try
        {
            Debug.Log($"[Ads]ShowGameWinInterstitial adDelayMet){adDelayMet()}");
            if (adDelayMet())
            {
                AdTestToast.Instance?.Show("Trigger: Game Win -> Showing Ad");
                Debug.Log($"[Ads]ShowGameWinInterstitial adDelayMet)");
                    ShowInterstitial((result) =>
                    {
                        Debug.Log($"ShowGameWinInterstitial:{result}");
                        if (result)
                        {
                            //counter2 = 0;
                        }
                        else
                        {
                            if (CustomAdManager.Instance)
                            CustomAdManager.Instance.ShowInterstitial();
                        }
                    });                
            }
            else
            {
                double remaining = Global.InterstitialAdGap - (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;
            AdTestToast.Instance?.Show($"Ad Delay: Win Ad blocked. Wait {remaining:F1}s.");
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
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowGameWinInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
    
    int GetCounter
    {
        get
        {
            int WorldNumber = GameConstants.getLastWorldUnlocked;      
            return ((WorldNumber < 1 && GameConstants.getLastUnlcokedLevel < (adIntervalLevelCheck))) ? gapBetweenAds: gapBetweenAdsSecondary;
        }
    }

    public bool LaunchInterstitialState()
    {
        return ((adMobNetworkHandler != null &&
            adMobNetworkHandler.adMobLaunchInterstitial != null && adMobNetworkHandler != null && adMobNetworkHandler.adMobLaunchInterstitial.CanShowAd()));
            //|| (levelPlayNetworkHandler.levelPlayLaunchInterstitial != null && levelPlayNetworkHandler.levelPlayLaunchInterstitial.IsAdReady()));
    }

    public void ShowRewardedVideo(Action<bool> callBack,AdType adType=AdType.Reward)
    {
        string rewardedVideoID = androidRewardedVideoID;

#if UNITY_IOS
        rewardedVideoID = iosRewardedVideoID;
#endif
       
        try
        {
           
            adMobNetworkHandler.ShowAdmobRewardedVideo(ShowLevelPlayRewarVideo,adType);       
            void ShowLevelPlayRewarVideo(bool flag)
            {
                if (!flag)
                {
                    //levelPlayNetworkHandler.ShowRewardBasedVideo((result) =>
                    //{
                    //    callBack?.Invoke(result);
                    //    if(result)
                    //    {
                    //        lastAdDisplayTime = Time.time;

                    //        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    //        {
                    //            if (adType == AdType.Reward)
                    //            {
                    //                FireBaseActions(AdContent.levelPlayRewardShown, AdMode.Shown, SuccessStatus.Success);
                    //            }
                    //        });

                    //    }
                    //});

                    //hybidNetworkHandler.ShowRewardAd((shown) =>
                    //{
                    //    if(shown)
                    //    {
                    //        lastAdDisplayTime = Time.time;
                    //        lastAdShownDateTime = DateTime.UtcNow;
                    //    }
                    //    callBack?.Invoke(shown);

                    //});
                }
                else
                {
                    lastAdDisplayTime = Time.time;

                    callBack?.Invoke(true);
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        if (adType == AdType.Reward)
                        {
                            FireBaseActions(AdContent.AdMobRewardShown, AdMode.Shown, SuccessStatus.Success);
                        }                        
                    });
                    lastAdShownDateTime = DateTime.UtcNow;

                }
            }
        }
        catch (Exception e)
        { }
    }

    public void RequestRewardBasedVideo(AdType adType=AdType.Reward)
    {
        //Debug.Log("Asdf RequestRewardBasedVideo..");       
        adMobNetworkHandler.RequestRewardBasedVideo(adType);
        //levelPlayNetworkHandler.RequestRewardBasedVideo(adType);
        //hybidNetworkHandler.RequestRewardAd();

    }
    int count = 0;
    public void RequestRewardedInterstitial(AdType adType)
    {
        adMobNetworkHandler.RequestRewardInterstitial(adType);
    }

    //public void CheckSecondaryInterstitialStatus(bool result)
    //{
    //if (!result && !adMobNetworkHandler.isInterstitialLoaded )
    //{
    //LoadSecondaryInterstitialAd();
    //}        
    //}
    /* public void CheckSecondaryInterstitialStatus(bool result)
    {
        if (result)
        {
            // RESET HERE: If an ad (Primary or Secondary) loads successfully, 
            // we reset the state for the NEXT request session.
            usingSecondaryInterstitialId = false;
            usingSecondaryLaunchId = false;
            return;
        }

        // HANDLE LAUNCH FALLBACK
        if (!adMobNetworkHandler.LaunchInterstitialState() && !usingSecondaryLaunchId) {
            LoadSecondaryLaunchAd();
        } 
        // HANDLE STANDARD INTERSTITIAL PING-PONG
        else if (!adMobNetworkHandler.adMobInterstitial.CanShowAd()) {
            if (!usingSecondaryInterstitialId) LoadSecondaryInterstitialAd();
            else ResetToPrimaryWithDelay();
        }
    } */

    public void CheckSecondaryInterstitialStatus(bool result, AdType type)
    {
        if (result)
        {
            if (type == AdType.Interstital) usingSecondaryInterstitialId = false;
            if (type == AdType.Launch) usingSecondaryLaunchId = false;
            return;
        }

        // ONLY fallback for Launch if the failing ad WAS a Launch ad
        if (type == AdType.Launch && !usingSecondaryLaunchId)
        {
            LoadSecondaryLaunchAd();
        }
        // ONLY fallback for regular Interstitial if the failing ad WAS Interstitial
        else if (type == AdType.Interstital)
        {
            if (!usingSecondaryInterstitialId)
                LoadSecondaryInterstitialAd();
            else
                ResetToPrimaryWithDelay();
        }
    }

    void LoadSecondaryLaunchAd() {
        usingSecondaryLaunchId = true;
        var adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
    if (adMobConfig == null) return;

        AdUnitConfig sec = AdsConfiguration.AdConfigContainer[0].adConfigs.Find(x => x.AdType == AdType.SecondaryInterstitial);
        AdTestToast.Instance?.Show("Fallback: Launch Primary Failed -> Trying Secondary");
        adMobNetworkHandler.SetLaunchId(sec.AdUnitId);
        adMobNetworkHandler.RequestInterstitial(AdType.Launch, "Secondary"); 
    }

    void LoadSecondaryInterstitialAd() {
        usingSecondaryInterstitialId = true;
        var adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
    if (adMobConfig == null) return;

        AdUnitConfig sec = AdsConfiguration.AdConfigContainer[0].adConfigs.Find(x => x.AdType == AdType.SecondaryInterstitial);
        AdTestToast.Instance?.Show("Fallback: Interstitial Primary Failed -> Trying Secondary");
        adMobNetworkHandler.SetInterStitalId(sec.AdUnitId);
        adMobNetworkHandler.RequestInterstitial(AdType.Interstital, "Secondary");
    }

    void ResetToPrimaryWithDelay() {
        usingSecondaryInterstitialId = false;
        AdUnitConfig pri = AdsConfiguration.AdConfigContainer[0].adConfigs.Find(x => x.AdType == AdType.Interstital);
        AdTestToast.Instance?.Show("Fallback: All Failed. Cooling down...");
        adMobNetworkHandler.SetInterStitalId(pri.AdUnitId);
        adMobNetworkHandler.RequestWithManualDelay(AdType.Interstital, Global.adRetryTime);
    }

    private bool isSecondaryInterstialLoaded;
    /* private void LoadSecondaryInterstitialAd()
    {
        if (isSecondaryInterstialLoaded)
            return;

        AdConfig adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
        if (adMobConfig != null)
        {
            AdUnitConfig adUnitConfig = adMobConfig.adConfigs.Find(x => x.AdType == AdType.SecondaryInterstitial);
            if (adUnitConfig != null && !string.IsNullOrEmpty(adUnitConfig.AdUnitId) && adUnitConfig.ActiveStatus == ActiveStatus.Active)
            {
                //Debug.Log("admob LoadSecondaryInterstitialAd requested");
                adMobNetworkHandler.SetInterStitalId(adUnitConfig.AdUnitId);
                //Remove once request ad with delay added.
                adMobNetworkHandler.StopPreviousCoroutine();
                adMobNetworkHandler.RequestInterstitial(AdType.Interstital);
                adMobNetworkHandler.rewardedInterStitialrequestcallBack = null;
                isSecondaryInterstialLoaded = true;
            }
        }
    } */

    public void CheckSecondaryRewardAdStatus(bool result)
    {
        if (!result)
        {
            LoadSecondaryRewardAd();
        }
    }

    private void LoadSecondaryRewardAd()
    {
        AdConfig adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
        if (adMobConfig != null)
        {
            AdUnitConfig adUnitConfig = adMobConfig.adConfigs.Find(x => x.AdType == AdType.SecondaryReward);
            if (adUnitConfig != null && !string.IsNullOrEmpty(adUnitConfig.AdUnitId) && adUnitConfig.ActiveStatus == ActiveStatus.Active)
            {
                Debug.Log("admob LoadSecondaryRewardAd requested");
                adMobNetworkHandler.SetRewardId(adUnitConfig.AdUnitId);
                //Remove once request ad with delay added.
                RequestRewardBasedVideo(AdType.Reward);
                Debug.Log("admob LoadSecondaryRewardAd requested  000");

                adMobNetworkHandler.rewardedrequestcallBack = null;
            }
        }
    }

    public void ShowRewardedInterstitial(Action<bool> callback,AdType adType)
    {
        adMobNetworkHandler.ShowRewardInterstitial((result) =>
        {
            callback?.Invoke(result);
            if (result)
            {
                lastAdDisplayTime = Time.time;

                if (adType == AdType.RewardedInterStitial)
                {
                    FireBaseActions(AdContent.AdMobRewardedInterstitialShown, AdMode.Shown, SuccessStatus.Success);
                }
                lastAdShownDateTime = DateTime.UtcNow;

            }
        }, adType);
    }

    public void RequestBannerAd()
    {
        if (!Global.isBannerEnabled)
        {
            return;
        }

        adMobNetworkHandler.RequestBannerView();
    }

    public void ShowbannerAd()
    {
        if (!Global.isBannerEnabled)
        {
            return;
        }
        int currentLevel = GameConstants.getLastUnlcokedLevel;
        if (currentLevel > (bannerAdShowLevelFrom))
        {
            adMobNetworkHandler.ShowBannerAd();
            //hybidNetworkHandler.RequestBannerAd();

        }
    }
    public void HidebannerAd()
    {
        adMobNetworkHandler.HideBannerView();
        //hybidNetworkHandler.HideBanner();
    }

    public IEnumerator RequestAppOpenAd()
    {
        /* int result = 0;
        yield return null; */
        /* if (Global.isAppOpenAdEnabled)
        {
            adMobNetworkHandler.RequestAppOpenAd((adAvailable) =>
            {
                if (adAvailable)
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                }
            });
        }
        else
        {
            result = 1;
        } */
        if (Global.isAppOpenAdEnabled)
        {
            AdTestToast.Instance?.Show("AppOpen: Requesting...");
            adMobNetworkHandler.RequestAppOpenAd();
        }

        /* if (result == 0)
        {
            Debug.LogWarning("AdManager: RequestAppOpenAd callback did not arrive in time; skipping app open ad.");
            result = 1;
        } */

        /* if (Global.isAppOpenAdEnabled && result == 2 && SceneManager.GetActiveScene().name.Contains("Splash"))
        {

            yield return new WaitForSeconds(1);
            ShowAppOpenAd();
        } */

        /* if (Global.isLaunchInterstitialEnabled)
        {
            yield return new WaitForSeconds(1);
            RequestLaunchInterstitial();
        } */
        
        if (Global.isIntersitialsEnabled)
        {
            yield return new WaitForSeconds(10);
            RequestInterstitial();
        }
        if (Global.isBannerEnabled)
        {
            yield return new WaitForSeconds(5);
            RequestBannerAd();          
        }
    }

    public void ShowAppOpenAd()
    {
        if (!Global.isAppOpenAdEnabled)
        {
            return;
        }

        adMobNetworkHandler.ShowAppOpenAd(
                (x) =>
                {
                    lastAdDisplayTime = Time.time;
                    lastAdShownDateTime= DateTime.UtcNow;
                    if (SceneManager.GetActiveScene().name.Contains("Splash"))
                        FireBaseActions(AdContent.AdmobAppopenSplashShown, AdMode.Shown, SuccessStatus.Success);
                    else
                        FireBaseActions(AdContent.AdmobAppopenAppForeGroundShown, AdMode.Shown, SuccessStatus.Success);


                }
            );
    }

    private void OnAppStateChanged(AppState state)
    {
        //Debug.Log("App State changed to : " + state);
        // if the app is Foregrounded and the ad is available, show it.
        if (!Global.isAppOpenAdEnabled)
        {
            return;
        }
        if (state==AppState.Foreground &&  adMobNetworkHandler.IsAdAvailable)
        {
            if (SceneManager.GetActiveScene().name.Contains("Splash"))
            {                
                ShowAppOpenAd();
            }
            else
            {
                ShowLoadingForBanner(2,true);
            }
        }       
    }

    public bool adDelayMet()
    {
        double seconds = (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;
       // Debug.LogError("current " + DateTime.UtcNow + "Last " + lastAdShownDateTime + " backFillAdGapToContinue" + Global.InterstitialAdGap + "seconds " + seconds);

        if (seconds > Global.InterstitialAdGap)
        {
            return true;
        }
        return false;
    }

    public void OnDestroy()
    {
      adMobNetworkHandler.rewardedInterStitialrequestcallBack -= CheckSecondaryInterstitialStatus;
        adMobNetworkHandler.rewardedrequestcallBack -= CheckSecondaryRewardAdStatus;

        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private bool wasPaused;

    void OnApplicationPause(bool pauseStatus)
    {
        wasPaused = pauseStatus;
    }

    void OnApplicationFocus(bool hasFocus)
    {

        if (!Global.isAppOpenAdEnabled)
        {
            return;
        }

#if UNITY_EDITOR

        return;
    #endif
        DummyLoadingPanelForBanner.SetActive(!hasFocus);
    }
    public void FireBaseActions(AdContent adContent, AdMode adMode, SuccessStatus status)
    {
        try
        {
            if (FirebaseEvents.instance != null)
            {
                FirebaseEvents.instance.LogFirebaseEvent(adContent.ToString(), adMode.ToString(), status.ToString());
            }
        }
        catch (Exception e)
        {

        }
    }

    public void FireBaseActions(GameEnum gameEnum, int currentWorld, int currentLevel)
    {
        try
        {
            if (FirebaseEvents.instance != null)
            {
                FirebaseEvents.instance.LogFirebaseEvent(gameEnum.ToString(),string.Concat( "WORLD : ",currentWorld), string.Concat("LEVEL : ", currentLevel));
            }
        }
        catch (Exception e)
        {

        }
    }

    public void FireBaseActions(string gameEnum, string currentWorld, string currentLevel)
    {
        try
        {
            if (FirebaseEvents.instance != null)
            {
                FirebaseEvents.instance.LogFirebaseEvent(gameEnum, currentWorld, currentLevel);
            }
        }
        catch (Exception e)
        {

        }
    }



    public void DelayOnShowAds()
    {
        StopCoroutine(DelayShowAds());
        StartCoroutine(DelayShowAds());
    }

    IEnumerator DelayShowAds()
    {
        canShowAd = false;
        yield return new WaitForSeconds(60);
        canShowAd = true;

    }
    #endregion

    #region AdMob




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
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
        //    //GameManager.Instance.gameState = GameState.Reward_Video_Started;
        //});
    }

    // Add this inside AdManager.cs
public bool IsRewardedVideoAvailable()
{
    if (adMobNetworkHandler != null)
    {
        return adMobNetworkHandler.IsRewardAdReady();
    }
    
    // Fallback: if you have LevelPlay or other handlers, check them here too
    /*
    if (levelPlayNetworkHandler != null && levelPlayNetworkHandler.IsRewardReady()) 
        return true;
    */

    return false;
}

    /*
    public void HandleRewardBasedVideoClosed(LevelPlayAdInfo levelPlayAdInfo)
    {
        try
        {
            GameManager.Instance.gameState = GameState.Reward_Video_Completed;           
            GameManager.Instance.ingamevideosuccess();
            this.RequestRewardBasedVideo();
        }
        catch (Exception e)
        { }
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
        //    //
        //});
        //FindObjectOfType<GameManager>().onrewardvideoSuccess();
    }

    public void HandleRewardBasedVideoRewarded(LevelPlayAdInfo obj, LevelPlayReward args)
    {
        try
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("rewardhandle___log");
            //rewardedvideosuccess = true;
        }
        catch (Exception e)
        { }
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
        //    ///
        //});
    }
    */

   

    public void onreward()
    {
        return;

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




    #endregion


   
    /*
    public void HandleOnAdFailedToLoad(LevelPlayAdError levelPlayAdError)
    {
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{
           
        //});
        try
        {
            //interstitialId = "ca-app-pub-3411062052281263/2087800960";
        }
        catch (Exception e)
        { }
    }



    public void HandleOnAdClosed(LevelPlayAdInfo levelPlayAdInfo)
    {
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
         
        //});

        try
        {
            if (Global.isIntersitialsEnabled)
            {
                //RequestInterstitial();
            }
        }
        catch (Exception e)
        { }
    }

    public void HandleOnAdClosedLaunch(LevelPlayAdInfo levelPlayAdInfo)
    {
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
           
        //});
        try
        {
            if (Global.isIntersitialsEnabled)
            {
              //  RequestLaunchInterstitial();
            }
        }
        catch (Exception e)
        { }
    }


    public void HandleOnAdClosedExit(LevelPlayAdInfo levelPlayAdInfo)
    { 
        //MobileAdsEventExecutor.ExecuteInUpdate(() => {
            
        //});
        try
        {
            
        }
        catch (Exception e)
        { }
    }
    */
   
    #region Init callback handlers

    #endregion

    public void ShowExitInterstitial()
    {
        return;

        try
        {

            

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

    private void RequestBanner()
    {
        /*
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
       */
    }

    private void RequestBannerExit()
    {
        /*
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
        */
    }

    /*
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
    */


    public void showbannerExit()
    {
        /*
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
        */
    }
    public void hidebannerExit()
    {
        /*
        if (bannerViewExit != null)
        {
            try
            {
                bannerViewExit.Hide();
            }
            catch (Exception e)
            { }
        }
        */
    }

    public void RequestExitInterstitial()
    {  
       
    }

    public void ResetIdToPrimary()
    {
        // 1. Reset local tracking flags
        usingSecondaryInterstitialId = false;
        usingSecondaryLaunchId = false;

        // 2. Find the AdMob Config safely
        AdConfig adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
        if (adMobConfig != null)
        {
            // 3. Get the Primary Interstitial ID
            AdUnitConfig priInter = adMobConfig.adConfigs.Find(x => x.AdType == AdType.Interstital);
            if (priInter != null && !string.IsNullOrEmpty(priInter.AdUnitId))
            {
                adMobNetworkHandler.SetInterStitalId(priInter.AdUnitId);
            }

            // 4. Get the Primary Launch ID
            AdUnitConfig priLaunch = adMobConfig.adConfigs.Find(x => x.AdType == AdType.Launch);
            if (priLaunch != null && !string.IsNullOrEmpty(priLaunch.AdUnitId))
            {
                adMobNetworkHandler.SetLaunchId(priLaunch.AdUnitId);
            }

            AdTestToast.Instance?.Show("Ads: IDs Reset to Primary");
            Debug.Log("AdManager: All IDs reset to Primary configuration.");
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
   continuegame =7,
   retryLevel = 8
}

public enum GameEnum
{
    UserSkippedLevel,
    RewardAdForExtraBall,
}


