using UnityEngine;
using GameAnalyticsSDK;

public class AnalyticsManager:MonoBehaviour
{
    private const string TAG = "[AnalyticsManager]";

    public static bool IsInitialized { get; private set; }

    private void Awake()
    {
        Debug.Log(">>>>>>>> [" + TAG + "] Awake <<<<<<<<");

        DontDestroyOnLoad(gameObject);

        GameAnalytics.Initialize();

        Debug.Log($">>>>>>>> [{TAG}] Initialize Called <<<<<<<<");
    }

    public static void LogEvent(GAProgressionStatus status)
    {
        try
        {
            int world = WorldSelectionHandler.worldSelected;
            int level = Global.CurrentLeveltoPlay;

            string worldId = $"World_{world}";
            string levelId = $"Level_{level}";

#if UNITY_EDITOR || PRODUCTION_BUILD_OFF
            Debug.Log(
                $"{TAG} Progression Event -> " +
                $"Status:{status}, World:{worldId}, Level:{levelId}"
            );
#endif

            GameAnalytics.NewProgressionEvent(
                status,
                worldId,
                levelId
            );

#if UNITY_EDITOR || PRODUCTION_BUILD_OFF
            Debug.Log($"{TAG} Progression Event Sent Successfully");
#endif
        }
        catch (System.Exception ex)
        {
            Debug.LogError(
                $"{TAG} Failed to send progression event.\n" +
                $"Status:{status}\n" +
                $"World:{WorldSelectionHandler.worldSelected}\n" +
                $"Level:{Global.CurrentLeveltoPlay}\n" +
                $"Exception:{ex}"
            );
        }
    }

    public static void LogAdAction(GAAdAction action, AdType internalTarget, string placement)
    {
        GAAdType gaType = MapToGAAdType(internalTarget);
        GameAnalytics.NewAdEvent(action, gaType, "admob", placement);
    }
    private static GAAdType MapToGAAdType(AdType type)
    {
        switch (type)
        {
            case AdType.BannerAd:
            case AdType.VeryHighCPMInterstitial:
            case AdType.HighCPMInterstitial:
            case AdType.MediumCPMInterstitial:
            case AdType.LowCPMInterstitial:
            case AdType.Reward:
            case AdType.RewardContinue:
            case AdType.SecondaryReward: return GAAdType.RewardedVideo;
            case AdType.RewardedInterStitial:
            default: return GAAdType.Undefined;
        }
    }

    public static void LogAdRevenue(string adUnitId, string adFormat, double revenue, string currency)
    {
        // Workaround for older SDKs: Log as a Design event
        // Format: "AdRevenue:[Network]:[Format]:[Placement]"
        string eventName = $"AdRevenue:AdMob:{adFormat}:{adUnitId}";

        // Design events take a float value
        GameAnalytics.NewDesignEvent(eventName, (float)revenue);
    }

    public static void LogDesignEvent(string eventName)
    {
        try
        {
#if UNITY_EDITOR || PRODUCTION_BUILD_OFF
            Debug.Log($"{TAG} Design Event -> {eventName}");
#endif

            GameAnalytics.NewDesignEvent(eventName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"{TAG} Design Event Failed: {eventName}\n{ex}");
        }
    }

    public static void LogDesignEvent(string eventName, float value)
    {
        try
        {
#if UNITY_EDITOR || PRODUCTION_BUILD_OFF
            Debug.Log($"{TAG} Design Event -> {eventName} : {value}");
#endif

            GameAnalytics.NewDesignEvent(eventName, value);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"{TAG} Design Event Failed: {eventName}\n{ex}");
        }
    }

    public static void LogResourceSink(string currency, float amount, string itemType, string itemId)
    {
        try
        {
#if UNITY_EDITOR || PRODUCTION_BUILD_OFF
            Debug.Log(
                $"{TAG} Resource Sink -> {amount} {currency}, {itemType}:{itemId}"
            );
#endif

            GameAnalytics.NewResourceEvent(
                GAResourceFlowType.Sink,
                currency,
                amount,
                itemType,
                itemId
            );
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"{TAG} Resource Event Failed\n{ex}");
        }
    }
}