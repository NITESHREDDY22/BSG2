using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotEnoughCoinsPopup : MonoBehaviour
{
    public static NotEnoughCoinsPopup Instance;

    [Header("Popup Root")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text currentCoinsTxt, coinsToBuyTxt, reqCoinsTxt, coinsInfoTxt;
    [SerializeField] private GameObject infoPopup, optionsPopup, coinsInfoPopup, coinStatus;

    [Header("UI Controls")]
    [SerializeField] private Button watchAdBtn; // <--- ADDED: Assign this in Inspector
    [SerializeField] private GameObject loadingSpinner; // <--- ADDED: Assign a spinning UI element here
    [SerializeField] private float adWaitTimeout = 7f;   // Time to wait for ad load (5-10s)
    private Coroutine _adWaitCoroutine;

    [Header("In-House Ad Settings")]
    [SerializeField] private InHouseAdController inHouseAd; 

    [Header("Callbacks")]
    public UnityEvent OnOpenCallback;
    public UnityEvent OnCloseCallback;
    public UnityEvent OnIAPSuccessCallback;
    public UnityEvent OnIAPFailedCallback;
    public UnityEvent OnWatchAdCallback;

    public static int rewardCoins = 500;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
        if(inHouseAd)inHouseAd.gameObject.SetActive(false);
        if(loadingSpinner) loadingSpinner.SetActive(false);
    }

    // -----------------------
    //   FIREBASE HELPER
    // -----------------------
    /* private void LogFirebase(string eventName)
    {
        // Maintains the exact naming convention: EventName_W1_L1
        string fullEventName = eventName + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay;

        // Firebase event names have a strict 40-character limit
        if (fullEventName.Length > 40)
        {
            // Trims characters from the start to keep only the last 40 characters
            fullEventName = fullEventName.Substring(fullEventName.Length - 40);
        }
        Debug.Log($"IsFirebaseReady{FirebaseEvents.IsFirebaseReady}");
        try
        {
        if (FirebaseEvents.IsFirebaseReady)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(fullEventName);
        }
        }
        catch
        {
            Debug.Log("Firebase event logging failed for event: " + fullEventName);
        }
        Debug.Log("[Firebase Log]: " + fullEventName);
    } */
    private void LogFirebase(string eventName)
    {
        int worldNumber = WorldSelectionHandler.worldSelected;
        int levelNumber = Global.CurrentLeveltoPlay;

        // Keep the Firebase event name length protection
        string trimmedEventName = eventName;

        if (trimmedEventName.Length > 40)
        {
            trimmedEventName = trimmedEventName.Substring(trimmedEventName.Length - 40);
        }

        Debug.Log($"IsFirebaseReady: {FirebaseEvents.IsFirebaseReady}");

        try
        {
            if (FirebaseEvents.IsFirebaseReady)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    trimmedEventName,
                    new Firebase.Analytics.Parameter("world", worldNumber),
                    new Firebase.Analytics.Parameter("level", levelNumber)
                );
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(
                $"Firebase event logging failed. Event={trimmedEventName}, World={worldNumber}, Level={levelNumber}\n{e}");
        }

        Debug.Log(
            $"[Firebase Log]: {trimmedEventName} | world={worldNumber} | level={levelNumber}");
    }

    // -----------------------
    //   OPEN / CLOSE POPUP
    // -----------------------
    public void Open()
    {
        if (watchAdBtn != null) watchAdBtn.interactable = true; 

        reqCoinsTxt.text = "" + (Global.finalReloadCoins - GameManager.GetCoins());
        coinsInfoTxt.text = "+" + rewardCoins.ToString();
        popupPanel.SetActive(true);
        ShowNotEnoughCoinsPopup();
        OnOpenCallback?.Invoke();
        
        if (AdManager._instance)
            AdManager._instance.HidebannerAd();

        LogFirebase("NotEnoughCoins_Open");
    }

    public void Close()
    {
        popupPanel.SetActive(false);
        OnCloseCallback?.Invoke();

        if (GameManager.Instance && (!GameManager.Instance.gameOverPanel.activeSelf && !GameManager.Instance.gameFailed.activeSelf))
        {
            if (AdManager._instance)
                AdManager._instance.ShowbannerAd();
        }
    }

    public void CloseFirebaseLog()
    {
        LogFirebase("NotEnoughCoins_Close");
    }

    public bool IsActive()
    {
        return popupPanel.activeSelf;
    }


    // -----------------------
    //   DIRECT IAP HANDLING
    // -----------------------
    public void BuyCoins_IAP()
    {
        Debug.Log("[NotEnoughCoinsPopup] Starting IAP purchase...");
        LogFirebase("NotEnoughCoins_buyCns_click");
    }


    // -----------------------
    //   WATCH AD HANDLING
    // -----------------------
    public void WatchAdSuccess(string adNetwork = "video")
    {
        #if UNITY_EDITOR
        //GameManager.Instance.gameState = GameState.Reward_Video_Completed;
        #endif
        Debug.Log($"WatchAdSuccess: User earned coins reward.{adNetwork}");
        LogFirebase("NotEnoughCoins_VidScss_"+adNetwork);
        if (watchAdBtn != null) watchAdBtn.interactable = false;
        
        if (CoinFlyAnimator.Instance)
        {
            CoinFlyAnimator.Instance.Play(coinsInfoTxt.GetComponent<RectTransform>(), coinStatus.GetComponent<RectTransform>(), spawnParent, () =>
            {
                Debug.Log("ShowCoinsAnimation complete");
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + rewardCoins);
                GameManager.AddCoins();
                Invoke(nameof(Close), 1f);
            });
        }
        else
        {
            // Fallback if animator is missing
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + rewardCoins);
            GameManager.AddCoins();
            Close();
        }
    }

    public void WatchAdToGetCoins()
    {
        if (watchAdBtn != null) watchAdBtn.interactable = false;
        LogFirebase("NotEnoughCoins_Vid_Clk");
        
        NetworkReachability reachability = Application.internetReachability;
        bool hasInternet = (reachability != NetworkReachability.NotReachable);

        if (!hasInternet)
        {
            LogFirebase("NotEnoughC_NoNet");
            ShowFallbackInHouse();
            return;
        }

        // Check if ad is already ready
        if (AdManager._instance != null && AdManager._instance.IsRewardedVideoAvailable())
        {
            LogFirebase("NotEnoughC_Ad_Prior");
            ShowActualAd();
        }
        else
        {
            // Ad not ready, start the "Request and Wait" flow
            if (_adWaitCoroutine != null) StopCoroutine(_adWaitCoroutine);
            _adWaitCoroutine = StartCoroutine(WaitAndShowAd());
        }
    }

    private IEnumerator WaitAndShowAd()
    {
        AdMobNetworkHandler.LastRewardLoadError = "Timeout_No_Response";
        LogFirebase("NotEnoughC_AdWait_Start");
        
        // Show Loading Spinner UI
        if (loadingSpinner) loadingSpinner.SetActive(true);

        // Force a request if one isn't already in flight
        if (AdManager._instance != null)
        {
            Debug.Log("Ad not ready. Requesting on-demand...");
            AdManager._instance.RequestRewardBasedVideo(AdType.Reward);
        }

        float timer = 0f;
        bool adFound = false;

        // Poll for the ad status until timeout
        while (timer < adWaitTimeout)
        {
            if (AdManager._instance != null && AdManager._instance.IsRewardedVideoAvailable())
            {
                adFound = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null; 
        }

        // Hide Spinner UI
        if (loadingSpinner) loadingSpinner.SetActive(false);

        if (adFound)
        {
            LogFirebase("NotEnoughC_Ad_OnDemand");
            ShowActualAd();
        }
        else
        {
            string reason = AdMobNetworkHandler.LastRewardLoadError;
            //LogFirebase("NotEnoughC_AdWait_Timeout");
            LogFirebase("NotEnoughC_Ad_Fail_"+ reason);
            Debug.LogWarning("Ad request timed out. Showing In-House fallback.");
            ShowFallbackInHouse();
        }
    }

    private void ShowActualAd()
    {
        AdManager._instance.rewardTypeToUnlock = RewardType.retryLevel;
        AdManager._instance.ShowRewardedVideo(result =>
        {
            if (result)
            {
                AdManager._instance.rewardedvideosuccess = true;
                WatchAdSuccess("admob");
            }
            else
            {
                if (watchAdBtn != null) watchAdBtn.interactable = true;
                LogFirebase("NotEnoughCoins_Vid_Skipped");
            }
        }, AdType.Reward);
    }

    private void ShowFallbackInHouse()
    {
        if (inHouseAd != null)
        {
            inHouseAd.OpenAndShow(() =>
            {
                if (watchAdBtn != null) watchAdBtn.interactable = true;
                LogFirebase("NotEnoughCoins_InHseAd_RwdGrantd");
                WatchAdSuccess("inhouse");
            });
        }
        else
        {
            if (watchAdBtn != null) watchAdBtn.interactable = true;
            Debug.LogError("InHouseAd reference missing!");
        }
    }
    public void OnDisable()
    {
        if (_adWaitCoroutine != null) StopCoroutine(_adWaitCoroutine);
        if (loadingSpinner) loadingSpinner.SetActive(false);
    }


    // -----------------------
    //   STATIC TRIGGER
    // -----------------------
    public static void Trigger()
    {
        if (Instance != null)
            Instance.Open();
        else
            Debug.LogWarning("[NotEnoughCoinsPopup] No instance found in scene.");
    }

    public void GetCoins()
    {
        infoPopup.SetActive(false);
        optionsPopup.SetActive(true);
        coinsInfoPopup.SetActive(true);
    }

    public void ShowNotEnoughCoinsPopup()
    {
        GetCoins();
    }

    // -----------------------
    //   COIN TOAST
    // -----------------------
    public FloatingCoinToast popupPrefab;
    public Transform spawnParent;

    public void ShowCoinDeduction(Vector3 worldPosition, int amount)
    {
        if (amount <= 0) return;
        var popup = Instantiate(popupPrefab, spawnParent);
        RectTransform rt = popup.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnParent.GetComponent<RectTransform>().rect.center;
        popup.Play("-" + amount);
    }
}