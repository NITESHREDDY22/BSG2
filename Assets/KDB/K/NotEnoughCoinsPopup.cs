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
    }

    // -----------------------
    //   FIREBASE HELPER
    // -----------------------
    private void LogFirebase(string eventName)
    {
        // Maintains the exact naming convention: EventName_W1_L1
        string fullEventName = eventName + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay;

        // Firebase event names have a strict 40-character limit
        if (fullEventName.Length > 40)
        {
            // Trims characters from the start to keep only the last 40 characters
            fullEventName = fullEventName.Substring(fullEventName.Length - 40);
        }

        Firebase.Analytics.FirebaseAnalytics.LogEvent(fullEventName);
        Debug.Log("[Firebase Log]: " + fullEventName);
    }

    // -----------------------
    //   OPEN / CLOSE POPUP
    // -----------------------
    public void Open()
    {
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
        
        if (AdManager._instance)
            AdManager._instance.ShowbannerAd();
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
        LogFirebase("NotEnoughCoins_VidScss_"+adNetwork);
        
        Invoke(nameof(Close), 2.5f);
        if (CoinFlyAnimator.Instance)
        {
            CoinFlyAnimator.Instance.Play(coinsInfoTxt.GetComponent<RectTransform>(), coinStatus.GetComponent<RectTransform>(), spawnParent, () =>
            {
                Debug.Log("ShowCoinsAnimation complete");
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + rewardCoins);
                GameManager.AddCoins();
            });
        }
    }

    public void WatchAdToGetCoins()
    {
        Debug.LogError("WatchAdToGetCoins CLICKED");
        LogFirebase("NotEnoughCoins_Vid_Clk");

        // Check availability before trying to show
        // Note: Replace 'IsRewardedVideoAvailable' with the actual check in your AdManager
        string netStatus = Application.internetReachability.ToString();
        LogFirebase("NotEnoughCoins_NetStat_" + netStatus);
        bool adReady = AdManager._instance != null && AdManager._instance.IsRewardedVideoAvailable();

        if (adReady)
        {
            AdManager._instance.rewardTypeToUnlock = RewardType.retryLevel;
            AdManager._instance.ShowRewardedVideo(result =>
            {
                if (result)
                {
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = true;
                    WatchAdSuccess("admob");
                }
                else
                {
                    // 2. Track Ad Skipped / Cancelled
                    LogFirebase("NotEnoughCoins_Vid_Skipped");
                    Debug.Log("User skipped the rewarded video.");
                }
            }, AdType.Reward);
        }
        else
        {
            /* // Triggered when no ad is available
            LogFirebase("NotEnoughCoins_NoAdAvl");
            ShowInHouseAd(); */
            LogFirebase("NotEnoughCoins_NoAdAvil");
            if (inHouseAd != null)
            {
                // Instead of SetActive here, we call the new combined method
                inHouseAd.OpenAndShow(() =>
                {
                    LogFirebase("NotEnoughCoins_InHseAd_RwdGrantd");
                    WatchAdSuccess("inhouse");
                });
            }
            else
            {
                Debug.LogError("InHouseAd reference is missing in Inspector!");
            }
        }
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