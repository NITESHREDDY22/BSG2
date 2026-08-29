using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Extensions;

public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance { get; private set; }

    // Common Log Header
    private const string LogHeader = "<b>[REMOTE CONFIG]</b> ";

    public AdManager adManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
#if !PRODUCTION_BUILD_OFF
        Debug.unityLogger.logEnabled = false;
#endif
    }

    void Start()
    {
        Debug.Log($"{LogHeader} Initializing Firebase SDK...");
        if(adManager == null) adManager = FindObjectOfType<AdManager>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log($"{LogHeader} Firebase Dependencies resolved successfully.");
                FirebaseEvents.FirebaseInitDone = true;
                InitializeRemoteConfig();
            }
            else
            {
                Debug.Log($"{LogHeader} <color=red>Dependencies Error:</color> {dependencyStatus}. Using fallbacks.");
                ApplyValuesToGame(); 
            }
        });
    }

    async void InitializeRemoteConfig()
    {
        // 1. Define ALL Local Defaults (Merged from legacy AdManager JSON and existing RC)
        var defaults = new Dictionary<string, object>
        {
            // --- Values from legacy AdManager JSON ---
            { "GOFAdInterval", 0 },
            { "GOWAdInterval", 0 },
            { "backFillAdGapToContinue", 60.0f },
            { "World2ReqStars", 10 },
            { "World3ReqStars", 20 },
            { "World4ReqStars", 30 },
            { "World5ReqStars", 40 },
            { "FIRST_LVLS_SET_AD_GAP", 2 },
            { "SECOND_LVLS_SET_AD_GAP", 3 },
            { "showLaunchAd", true },
            { "InterstitialAdGap", 30 },
            { "isSingularEnabled", true },
            { "isBannerEnabled", true },
            { "isIntersitialsEnabled", true },
            { "isRewaredAdsEnabled", true },
            { "isAppOpenAdEnabled", true },
            { "adRetryTime", 30 },
            { "showBannerFrom", 3 },
            { "coinsToReload", 100 },
            { "defaultCoins", 500 },
            { "tragectoryChallenge", false },
            { "notEnoughRewardCoins", 50 },
            { "rewardAdsRequestDelay", 180 },
            { "notificationInterval", 180 },
            { "secondNotificationDelay", 360 },
            { "customAdsEnabled", true },
            { "showGameZopInterstitials", true },

            // --- Existing RemoteConfig Defaults ---
            { "adGap1", 90 }, { "adGap2", 70 }, { "adGap3", 60 }, { "adGap4", 75 },
            { "bannerLevel", 6 },
            { "backFillAds", true },
            { "backFillAdGap", 180 },
            { "useAnalytics", true },
            { "useIAP", true },
            { "crossPromoEnabled", true },
            { "package", "com.knockdown.bottleshootgame" },
            { "level_timer_config", "{ \"levelTimers\": [] }" },
            { "multiplayerOn", true }
        };

        await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
        Debug.Log($"{LogHeader} Local default dictionary successfully set.");

        FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += OnConfigUpdateReceived;

        FetchAndActivate();
    }

    public void FetchAndActivate()
    {
        Debug.Log($"{LogHeader} Fetching remote values from server...");
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(task =>
        {
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            if (task.IsCompleted && info.LastFetchStatus == LastFetchStatus.Success)
                Debug.Log($"{LogHeader} <color=green>FETCH SUCCESS:</color> Fresh values obtained.");
            else
                Debug.Log($"{LogHeader} <color=orange>FETCH OFFLINE:</color> Using cached values/defaults.");

            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
            {
                ApplyValuesToGame();
            });
        });
    }

    private void ApplyValuesToGame()
    {
        Debug.Log($"{LogHeader} --- Mapping Config Values to Global & Managers ---");

        // --- AD MANAGER INSTANCE MAPPING ---
        if (AdManager._instance != null)
        {
            var ad = AdManager._instance;
            ad.GOFAdInterval = GetSafeInt("GOFAdInterval", 0);
            ad.GOWAdInterval = GetSafeInt("GOWAdInterval", 0);
            ad.isLaunchInterstitialEnabled = GetSafeBool("showLaunchAd", true);
            ad.adIntervalLevelCheck = GetSafeInt("adIntervalLevelCheck", 30);
            
            // Syncing the gaps and levels (Assumes you made these public or added the setters in AdManager)
            ad.SetGaps(GetSafeInt("FIRST_LVLS_SET_AD_GAP", 2), GetSafeInt("SECOND_LVLS_SET_AD_GAP", 3));
            ad.SetBannerLevel(GetSafeInt("showBannerFrom", 3));
        }

        // --- GLOBAL VARIABLES (Previously from legacy JSON) ---
        Global.backFillAdGapToContinue = GetSafeFloat("backFillAdGapToContinue", 60f);
        Global.World2ReqStars = GetSafeInt("World2ReqStars", 10);
        Global.World3ReqStars = GetSafeInt("World3ReqStars", 20);
        Global.World4ReqStars = GetSafeInt("World4ReqStars", 30);
        Global.World5ReqStars = GetSafeInt("World5ReqStars", 40);
        Global.InterstitialAdGap = GetSafeInt("InterstitialAdGap", 30);
        Global.isSingularEnabled = GetSafeBool("isSingularEnabled", true);
        Global.isBannerEnabled = GetSafeBool("isBannerEnabled", true);
        Global.isIntersitialsEnabled = GetSafeBool("isIntersitialsEnabled", true);
        Global.isRewaredAdsEnabled = GetSafeBool("isRewaredAdsEnabled", true);
        Global.isAppOpenAdEnabled = GetSafeBool("isAppOpenAdEnabled", true);
        Global.adRetryTime = GetSafeInt("adRetryTime", 30);
        Global.coinsToReload = GetSafeInt("coinsToReload", 100);
        Global.defaultCoins = GetSafeInt("defaultCoins", 500);
        Global.tragectoryChallenge = GetSafeBool("tragectoryChallenge", false);
#if UNITY_EDITOR
        Global.tragectoryChallenge = true;
#endif
#if CHEATS_ON
        Global.tragectoryChallenge = true;
#endif
        Global.rewardAdsRequestDelay = GetSafeInt("rewardAdsRequestDelay", 180);
        Global.notificationInterval = GetSafeInt("notificationInterval", 180);
        Global.secondNotificationDelay = GetSafeInt("secondNotificationDelay", 360);
        Global.isLaunchInterstitialEnabled = GetSafeBool("showLaunchAd", true);
        Global.adsEnabledFromLevel = GetSafeInt("adsEnabledFromLevel", 1);
        InternetValidator.mandatoryInternetToPlayFromLevel = GetSafeInt("InternetMandtoryLevel", 1); 
        Global.notificationDaysCount = GetSafeInt("notificationDaysCount", 7);
        string rawGaps = GetSafeString("interstitial_tier_gaps", "60,70,80,90");
        ParseAdGaps(rawGaps);

        Global.requestNextAdOnShow = GetSafeBool("requestNextAdOnShow", false);
        Global.useRegularInterstitialAsLaunch = GetSafeBool("useRegularInterstitialAsLaunch", false);

        // --- COMPONENT SETTINGS ---
        NotEnoughCoinsPopup.rewardCoins = GetSafeInt("notEnoughRewardCoins", 300);
        CustomAdManager.adsEnabled = GetSafeBool("customAdsEnabled", true);
        CustomAdManager.showGameZopInterstitials = GetSafeBool("showGameZopInterstitials", true);

        Debug.Log($"{LogHeader} --- Mapping Complete. Global.loadedFromServer = TRUE ---");
    }
    

    private void OnConfigUpdateReceived(object sender, ConfigUpdateEventArgs args)
    {
        if (args.Error != RemoteConfigError.None) return;

        FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($"{LogHeader} <color=cyan>REAL-TIME UPDATE RECEIVED:</color> Keys: {string.Join(", ", args.UpdatedKeys)}");
            ApplyValuesToGame();
            FirebaseEvents.FirebaseInitDone = true;
        });
    }

    private void ParseAdGaps(string raw)
    {
        try
        {
            string[] split = raw.Split(',');
            // CHANGE: Check for 4 instead of 5
            if (split.Length >= 4)
            {
                Global.GapVeryHighCPM = int.Parse(split[0]);
                Global.GapHighCPM = int.Parse(split[1]);
                Global.GapMediumCPM = int.Parse(split[2]);
                Global.GapLowCPM = int.Parse(split[3]);

                Debug.Log($"{LogHeader} Interstitial Gaps Updated: VH:{split[0]}s, H:{split[1]}s, M:{split[2]}s, L:{split[3]}s");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"{LogHeader} Error parsing gaps string: {e.Message}");
        }
    }

    // --- LOGGING GET METHODS ---

    public int GetSafeInt(string key, int fallback)
    {
        var configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        int result = (configValue.Source == ValueSource.StaticValue) ? fallback : (int)configValue.LongValue;
        LogValueSource(key, result, configValue.Source);
        return result;
    }

    public string GetSafeString(string key, string fallback = "")
    {
        var configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        string result = (configValue.Source == ValueSource.StaticValue) ? fallback : configValue.StringValue;
        LogValueSource(key, result, configValue.Source);
        return result;
    }

    public float GetSafeFloat(string key, float fallback = 0f)
    {
        var configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        float result = (configValue.Source == ValueSource.StaticValue) ? fallback : (float)configValue.DoubleValue;
        LogValueSource(key, result, configValue.Source);
        return result;
    }

    public bool GetSafeBool(string key, bool fallback = false)
    {
        var configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        bool result = (configValue.Source == ValueSource.StaticValue) ? fallback : configValue.BooleanValue;
        LogValueSource(key, result, configValue.Source);
        return result;
    }

    private void LogValueSource(string key, object value, ValueSource source)
    {
        string color = "white";
        string sourceName = "UNKNOWN";

        switch (source)
        {
            case ValueSource.RemoteValue:
                color = "yellow";
                sourceName = "SERVER (Cloud)";
                break;
            case ValueSource.DefaultValue:
                color = "cyan";
                sourceName = "LOCAL DEFAULT (Dictionary)";
                break;
            case ValueSource.StaticValue:
                color = "red";
                sourceName = "FALLBACK (Not found)";
                break;
        }

        Debug.Log($"{LogHeader} Key: <b>{key}</b> | Value: <color={color}>{value}</color> | Source: {sourceName}");
    }

    private void OnDestroy()
    {
        FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= OnConfigUpdateReceived;
    }
}