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


    /* [Header("Launch Ad Settings")]
    [Tooltip("If true, the game will ignore the specific Launch Ad ID and use the regular Interstitial Waterfall for the first ad.")]
    public static bool useRegularInterstitialAsLaunch = false; */

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
        yield return new WaitForSeconds(1f);
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
    public void ShowLoadingForBanner(float timer, bool canCheckLastAdDisplay = false)
    {
        Debug.Log($"[Ads] ShowLoadingForBanner called. CheckGap: {canCheckLastAdDisplay}");

        loadingFillBar.fillAmount = 0;
        LoadingPanelForBanner.SetActive(true);

        // Ensure game isn't paused while loading ad
        Time.timeScale = 1;

        if (cacheBannerAd != null)
        {
            StopCoroutine(cacheBannerAd);
        }

        cacheBannerAd = StartCoroutine(handleBannerAdLoading(timer, () =>
        {
            if (canCheckLastAdDisplay)
            {
                // Use the updated tier-based delay check for AppOpenAd
                if (adDelayMet(AdType.AppOpenAd))
                {
                    AdTestToast.Instance?.Show("AOA: Delay met. Showing ad...");
                    ShowAppOpenAd();
                }
                else
                {
                    // adDelayMet(AdType.AppOpenAd) already shows a toast with the time remaining.
                    Debug.Log("[Ads] AppOpenAd suppressed: Cooling down.");
                    HideLoadingForBanner(); // Hide if we aren't showing the ad
                }
            }
            else
            {
                AdTestToast.Instance?.Show("AOA: Forced Show (No Gap Check)");
                ShowAppOpenAd();
            }
        }));
    }

    IEnumerator handleBannerAdLoading(float timerTarget, Action callback)
    {
        float timer = 0;
        float lerpValue = 0;
        float targetTimer = timerTarget - 0.15f;

        // Small delay before starting
        yield return new WaitForSeconds(0.15f);

        // If ad isn't even loaded in the background, don't make user wait
        if (!adMobNetworkHandler.IsAdAvailable)
        {
            Debug.Log("[Ads] AOA not loaded. Aborting loading bar.");
            AdTestToast.Instance?.Show("AOA Fail: Ad not ready");
            HideLoadingForBanner();
            yield break;
        }

        // Trigger the logic (which calls ShowAppOpenAd)
        callback?.Invoke();

        // Progress the bar while the ad is preparing to pop up
        while (timer < targetTimer)
        {
            timer += Time.unscaledDeltaTime; // Use unscaled to ignore pause
            lerpValue = timer / targetTimer;
            loadingFillBar.fillAmount = lerpValue;
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
    private int currentInterstitialTierIndex = 0; 
private readonly AdType[] interstitialTierOrder = {
    AdType.VeryHighCPMInterstitial,
    AdType.HighCPMInterstitial,
    AdType.MediumCPMInterstitial,
    AdType.LowCPMInterstitial
};
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

        /* if (!isAdMobInitialized)
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
        } */

        bool isOffline = (Application.internetReachability == NetworkReachability.NotReachable);

        if (!isAdMobInitialized || isOffline)
        {
            Debug.LogWarning($"AdManager: Ads not ready. Init: {isAdMobInitialized}, Offline: {isOffline}");

            // Inform the user why ads aren't appearing yet
            if (isOffline)
                AdTestToast.Instance?.Show("Ads: No Internet. Waiting for connection...");
            else
                AdTestToast.Instance?.Show("Ads: SDK initializing...");


            if (!hasRequestedLaunchAd)
            {
                hasRequestedLaunchAd = true;
                RequestLaunchInterstitial();
            }

            // Start the watchdog if it's not already running
            if (!_isRetryLoopRunning)
            {
                StartCoroutine(RetryInitWhenInternetReturns());
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

    private bool _isRetryLoopRunning = false;

    private IEnumerator RetryInitWhenInternetReturns()
    {
        _isRetryLoopRunning = true;
        Debug.Log("<color=orange>[Ads Watchdog]</color> Started.");

        // We stay in this loop until we have internet AND have successfully triggered ads
        while (true)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // INTERNET IS BACK!

                if (!isAdMobInitialized)
                {
                    // If for some reason the SDK isn't even locally ready, try the full init
                    AdTestToast.Instance?.Show("Ads: Retrying Full Initialization...");
                    yield return StartCoroutine(InitializeAdNetworks());
                }
                else
                {
                    // The SDK is already "Initialized" but we were offline.
                    // Just trigger the ad loading sequence.
                    TriggerAllAdLoadSequences();
                }

                // Once we've triggered the loads, we can stop the loop.
                break;
            }
            else
            {
                // STILL OFFLINE
                AdTestToast.Instance?.Show("Ads: Still Offline. Checking again in 10s...");
            }

            yield return new WaitForSeconds(10f);
        }

        _isRetryLoopRunning = false;
        Debug.Log("<color=green>[Ads Watchdog]</color> Successfully triggered ads and stopped.");
    }

    private void TriggerAllAdLoadSequences()
    {
        AdTestToast.Instance?.Show("Ads: Internet found. Loading all ad types...");

        // This starts the chain of requests (AppOpen -> Interstitial -> Banner -> Reward)
        // exactly like your original script does at the end of initialization.
        StartCoroutine(RequestAppOpenAd());

        // Note: Your RequestAppOpenAd() already handles the 1s/5s/10s delays 
        // for Interstitials, Banners, and Rewards internally.
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
        if (Global.useRegularInterstitialAsLaunch)
        {
            Debug.Log("[Ads] Flow: useRegularInterstitialAsLaunch is TRUE. Requesting VeryHighCPM Interstitial instead of Launch ID.");
            // We request the start of the regular waterfall
            adMobNetworkHandler.RequestInterstitial(AdType.VeryHighCPMInterstitial);
        }
        else
        {
            Debug.Log("[Ads] Flow: useRegularInterstitialAsLaunch is FALSE. Requesting dedicated Launch Ad.");
            adMobNetworkHandler.RequestInterstitial(AdType.Launch);
        }
        //adMobNetworkHandler.RequestInterstitial(AdType.Launch);   
    }
    public void ShowLaunchInterstitial(bool shownow = false)
    {
        try
        {

            // --- NEW FLOW SWITCH ---
            if (Global.useRegularInterstitialAsLaunch)
            {
                Debug.Log("[Ads] ShowLaunchInterstitial: Redirecting to regular Interstitial Waterfall.");
                // We call ShowInterstitial with a callback to handle the isLaunchAdShown flag
                AdTestToast.Instance?.Show("Launch Ad: Attempting show Regular Interstitial...");
                ShowInterstitial((success) =>
                {
                    isLaunchAdShown = success;
                },true);
                return;
            }
            // --- END NEW FLOW SWITCH ---
            // 1. Check if the gap for Launch ads is met (Launch gap is usually 0)
            if (adDelayMet(AdType.Launch) || shownow)
            {
                Debug.Log("[Ads] ShowLaunchInterstitial: Attempting to display...");
                AdTestToast.Instance?.Show("Launch Ad: Attempting show...");

                // 2. Try the primary Launch ID first
                if (adMobNetworkHandler.IsInterstitialReady(AdType.Launch))
                {
                    adMobNetworkHandler.ShowInterstitialAd(AdType.Launch, (success) =>
                    {
                        HandleLaunchAdResult(success, "Primary Launch");
                    });
                }
                // 3. WATERFALL FALLBACK: If specific Launch ad isn't ready, try the Very High CPM tier
                else if (adMobNetworkHandler.IsInterstitialReady(AdType.VeryHighCPMInterstitial))
                {
                    AdTestToast.Instance?.Show("Launch Primary not ready. Using VeryHigh tier fallback...");
                    adMobNetworkHandler.ShowInterstitialAd(AdType.VeryHighCPMInterstitial, (success) =>
                    {
                        HandleLaunchAdResult(success, "VeryHigh Fallback");
                    });
                }
                else
                {
                    // 4. ULTIMATE FALLBACK: Custom Ads
                    Debug.Log("[Ads] ShowLaunchInterstitial: No AdMob units ready. Trying Custom Ad.");
                    AdTestToast.Instance?.Show("Launch: All AdMob units failed. Trying Custom...");

                    if (CustomAdManager.Instance)
                        CustomAdManager.Instance.ShowInterstitial();
                }
            }
            else
            {
                Debug.Log("[Ads] ShowLaunchInterstitial: Delay not met.");
                // adDelayMet already shows a toast with the time remaining.
            }
        }
        catch (Exception exp)
        {
            // Preserving your original detailed exception logging
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Admanager_ShowLaunchInterstitial", exp.Message + "at " + errorline);
                }
            }
            catch (Exception) { }
        }
    }

    // Helper to handle result and update timestamps
    private void HandleLaunchAdResult(bool flag, string sourceName)
    {
        Debug.Log($"[Ads] ShowLaunchInterstitial ({sourceName}) success: {flag}");
        isLaunchAdShown = flag;

        if (flag)
        {
            lastAdDisplayTime = Time.time;
            lastAdShownDateTime = DateTime.UtcNow;
            AdTestToast.Instance?.Show($"{sourceName} Shown Successfully");
        }
        else
        {
            // Rare case: ad reported ready but failed to show
            AdTestToast.Instance?.Show($"{sourceName} Display Failed. Trying Custom...");
            if (CustomAdManager.Instance)
                CustomAdManager.Instance.ShowInterstitial();
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

        adMobNetworkHandler.RequestInterstitial(AdType.VeryHighCPMInterstitial);
        // levelPlayNetworkHandler.RequestInterstitial(AdType.Interstital);
        //hybidNetworkHandler.RequestInterstitial();
    }

    public void ShowInterstitial(Action<bool> callBack = null,bool isLaunchAd = false)
    {
        int displayLevel = Global.CurrentLeveltoPlay;
        int currentWorld = WorldSelectionHandler.worldSelected;

        // We block ads ONLY if we are in the first world AND below the level threshold
        // If we are in World 1, 2, etc., ads will show regardless of level number.
        bool isAdFreeZone = (currentWorld < 1 && displayLevel < Global.adsEnabledFromLevel);

        // 1. Level Check Feedback
        if (!isLaunchAd && isAdFreeZone)// displayLevel < Global.adsEnabledFromLevel)
        {
            string adFreeMsg = $"World {currentWorld} Level {displayLevel} is Ad-Free (Ads start at Lvl {Global.adsEnabledFromLevel})";
            Debug.Log($"[Ads] {adFreeMsg}");
            AdTestToast.Instance?.Show(adFreeMsg);
            callBack?.Invoke(false);
            return;
        }

        /* if (!canShowAd)
        {
            AdTestToast.Instance?.Show("Ads currently suppressed (canShowAd = false)");
            return;
        } */

        // 2. Preparation for Detailed Feedback
        System.Text.StringBuilder statusReport = new System.Text.StringBuilder();
        statusReport.AppendLine("<b>Waterfall Status:</b>");

        double secondsSinceLastAd = (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;

        // 3. Loop through tiers to find a winner or build the failure report
        foreach (AdType tier in interstitialTierOrder)
        {
            bool isReady = adMobNetworkHandler.IsInterstitialReady(tier);
            float requiredGap = GetGapForTier(tier);
            bool timeMet = isLaunchAd || secondsSinceLastAd >= requiredGap;

            if (isReady && timeMet)
            {
                string gapInfo = isLaunchAd ? "Launch Bypass" : $"Gap {requiredGap}s met";
                // SUCCESS CASE
                AdTestToast.Instance?.Show($"<b>SHOWING: {tier}</b>\n(gapInfo)");

                adMobNetworkHandler.ShowInterstitialAd(tier, (success) =>
                {
                    if (success)
                    {
                        lastAdDisplayTime = Time.time;
                        lastAdShownDateTime = DateTime.UtcNow;
                        callBack?.Invoke(true);
                    }
                    else
                    {
                        AdTestToast.Instance?.Show($"{tier} object error. Trying Custom.");
                        TryCustomAd(callBack);
                    }
                });
                return; // Exit function, we found an ad!
            }
            else
            {
                // FAILURE CASE FOR THIS TIER - Build string for toast
                string reason = "";
                if (!isReady)
                    reason = "<color=red>Not Loaded</color>";
                else
                    reason = $"Wait <color=yellow>{(requiredGap - secondsSinceLastAd):F1}s</color>";

                statusReport.AppendLine($"- {tier}: {reason}");
            }
        }

        // 4. If we reach here, NO tier was eligible. Show the final detailed report.
        string finalReport = statusReport.ToString();
        Debug.Log($"[Ads] {finalReport}");
        AdTestToast.Instance?.Show(finalReport);

        TryCustomAd(callBack);
    }

    private void TryCustomAd(Action<bool> callBack)
    {
        if (CustomAdManager.Instance)
            CustomAdManager.Instance.ShowInterstitial();
        callBack?.Invoke(false);
    }

    public void ShowCommonInterstitial(Action<bool> callBack = null) => ShowInterstitial(callBack);
    public void ShowGameFailInterstitial() => ShowInterstitial();
    public void ShowGameWinInterstitial() => ShowInterstitial();

    int GetCounter
    {
        get
        {
            int WorldNumber = GameConstants.getLastWorldUnlocked;      
            return ((WorldNumber < 1 && GameConstants.getLastUnlcokedLevel < (adIntervalLevelCheck))) ? gapBetweenAds: gapBetweenAdsSecondary;
        }
    }

    /* public bool LaunchInterstitialState()
    {
        return ((adMobNetworkHandler != null &&
            adMobNetworkHandler.adMobLaunchInterstitial != null && adMobNetworkHandler != null && adMobNetworkHandler.adMobLaunchInterstitial.CanShowAd()));
            //|| (levelPlayNetworkHandler.levelPlayLaunchInterstitial != null && levelPlayNetworkHandler.levelPlayLaunchInterstitial.IsAdReady()));
    } */

    public bool LaunchInterstitialState()
    {
        if (adMobNetworkHandler == null) return false;

        if (Global.useRegularInterstitialAsLaunch)
        {
            // --- COVERAGE FOR NEW FLOW ---
            // We check the waterfall items because 'adMobLaunchInterstitial' is null in this mode
            foreach (AdType tier in interstitialTierOrder)
            {
                // This checks item.Interstitial != null && item.Interstitial.CanShowAd()
                if (adMobNetworkHandler.IsInterstitialReady(tier)) return true;
            }
            return false;
        }
        else
        {
            // --- COVERAGE FOR ORIGINAL FLOW ---
            // This replaces your original line with a safer dictionary-based check 
            // that looks at the exact same 'CanShowAd()' status.
            return adMobNetworkHandler.IsInterstitialReady(AdType.Launch);
        }
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
        if (result) return; // Load success, stop waterfall

        // LAUNCH FALLBACK
        if (type == AdType.Launch && !usingSecondaryLaunchId)
        {
            usingSecondaryLaunchId = true;
            AdTestToast.Instance?.Show("Launch Primary Failed -> Pinging VeryHigh Tier");
            adMobNetworkHandler.RequestInterstitial(AdType.VeryHighCPMInterstitial);
            return;
        }

        // REGULAR INTERSTITIAL WATERFALL
        int tierIndex = Array.IndexOf(interstitialTierOrder, type);
        if (tierIndex != -1)
        {
            int nextTierIndex = tierIndex + 1;
            if (nextTierIndex < interstitialTierOrder.Length)
            {
                AdType nextType = interstitialTierOrder[nextTierIndex];
                AdTestToast.Instance?.Show($"Waterfall: {type} failed -> Pinging {nextType}");

                // Logic: AdMobNetworkHandler already has the IDs mapped from SetAdConfig
                adMobNetworkHandler.RequestInterstitial(nextType);
            }
            else
            {
                AdTestToast.Instance?.Show("Waterfall: All tiers failed. Cooling down.");
                ResetIdToPrimary();
                // Start cooling down the VeryHigh tier for the next retry
                adMobNetworkHandler.RequestWithManualDelay(AdType.VeryHighCPMInterstitial, Global.adRetryTime);
            }
        }
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
        if (Global.isBannerEnabled)
        {
            yield return new WaitForSeconds(5);
            RequestBannerAd();          
        }

        if (Global.isIntersitialsEnabled)
        {
            if (!Global.useRegularInterstitialAsLaunch)
            {
                yield return new WaitForSeconds(20);
                RequestInterstitial();
            }
        }

        if (Global.isRewaredAdsEnabled)
        {
            yield return new WaitForSeconds(Global.rewardAdsRequestDelay);
            RequestRewardAds();          
        }
    }

    private void RequestRewardAds()
    {
        RequestRewardBasedVideo(AdType.Reward);
        RequestRewardBasedVideo(AdType.RewardContinue);
        //RequestRewardedInterstitial(AdType.RewardedInterStitial);
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

    public bool adDelayMet(AdType type)
    {
        double secondsSinceLastAd = (DateTime.UtcNow - lastAdShownDateTime).TotalSeconds;
        float requiredGap = GetGapForTier(type);
        bool isMet = secondsSinceLastAd >= requiredGap;

        if (!isMet && type != AdType.Launch)
        {
            float timeLeft = requiredGap - (float)secondsSinceLastAd;
            string statusMsg = $"{type} Cooling Down: {timeLeft:F1}s remaining";

            Debug.Log($"<color=orange>[Cooldown]</color> {statusMsg}");
            AdTestToast.Instance?.Show(statusMsg);
        }

        return isMet;
    }

    private float GetGapForTier(AdType type)
    {
        float gap = type switch
        {
            AdType.VeryHighCPMInterstitial => Global.GapVeryHighCPM,
            AdType.HighCPMInterstitial => Global.GapHighCPM,
            AdType.MediumCPMInterstitial => Global.GapMediumCPM,
            AdType.LowCPMInterstitial => Global.GapLowCPM,
            AdType.AppOpenAd => Global.InterstitialAdGap,
            AdType.Launch => 0f,
            _ => Global.GapHighCPM // Fallback
        };

        // Logging for traceability
        if (type != AdType.Launch)
        {
            Debug.Log($"<b>[AD TIMING]</b> {type} required gap: {gap}s");
        }

        return gap;
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

    void LoadSecondaryLaunchAd()
    {
        usingSecondaryLaunchId = true;

        // Find the Low CPM config (formerly Secondary)
        AdUnitConfig lowCpmConfig = AdsConfiguration.AdConfigContainer[0].adConfigs.Find(x => x.AdType == AdType.LowCPMInterstitial);

        if (lowCpmConfig != null)
        {
            AdTestToast.Instance?.Show("Fallback: Launch Primary Failed -> Trying LowCPM Interstitial");

            // Temporarily swap the Launch Ad ID to the Low CPM ID
            adMobNetworkHandler.SetLaunchId(lowCpmConfig.AdUnitId);
            adMobNetworkHandler.RequestInterstitial(AdType.Launch, "Secondary_Launch");
        }
    }

    public void ResetIdToPrimary()
    {
        usingSecondaryLaunchId = false;
        
        AdConfig adMobConfig = AdsConfiguration.AdConfigContainer.Find(x => x.NetworkType == NetworkType.AdMob);
        if (adMobConfig == null) return;

        // Reset the Launch ID
        AdUnitConfig priLaunch = adMobConfig.adConfigs.Find(x => x.AdType == AdType.Launch);
        if (priLaunch != null) adMobNetworkHandler.SetLaunchId(priLaunch.AdUnitId);

        foreach (AdType tier in interstitialTierOrder)
        {
            AdUnitConfig config = adMobConfig.adConfigs.Find(x => x.AdType == tier);
            if (config != null)
            {
                adMobNetworkHandler.SetInterStitalId(tier, config.AdUnitId);
                
                // --- NEW: Hard reset the 'Requested' flag so the next request isn't blocked ---
                // This ensures that if a tier failed earlier, it's allowed to try again now.
                var item = adMobNetworkHandler.GetAdItem(tier); // Add this helper to Handler
                if (item != null) item.isAdRequested = false;
            }
        }
        Debug.Log("[Ads] Waterfall IDs and Flags Reset to Primary.");
    }
    // --- NEW HELPER METHODS FOR REMOTE CONFIG ---
    public void SetGaps(int primary, int secondary)
    {
        this.gapBetweenAds = primary;
        this.gapBetweenAdsSecondary = secondary;
    }

    public void SetBannerLevel(int level)
    {
        this.bannerAdShowLevelFrom = level;
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
   continuegame =7,
   retryLevel = 8
}

public enum GameEnum
{
    UserSkippedLevel,
    RewardAdForExtraBall,
}


