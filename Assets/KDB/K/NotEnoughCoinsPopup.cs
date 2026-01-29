using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotEnoughCoinsPopup : MonoBehaviour
{
    public static NotEnoughCoinsPopup Instance;

    [Header("Popup Root")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField]private Text currentCoinsTxt,coinsToBuyTxt,reqCoinsTxt;
    [SerializeField]private GameObject infoPopup,optionsPopup,coinsInfoPopup,coinStatus;

    [Header("Callbacks")]
    public UnityEvent OnOpenCallback;
    public UnityEvent OnCloseCallback;
    public UnityEvent OnIAPSuccessCallback;
    public UnityEvent OnIAPFailedCallback;
    public UnityEvent OnWatchAdCallback;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
    }

    // -----------------------
    //   OPEN / CLOSE POPUP
    // -----------------------
    public void Open()
    {
        reqCoinsTxt.text = ""+(Global.finalReloadCoins-GameManager.GetCoins());
        popupPanel.SetActive(true);
        ShowNotEnoughCoinsPopup();
        OnOpenCallback?.Invoke();
        if (AdManager._instance)
            AdManager._instance.HidebannerAd();

        Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_Open_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay
        );
        Debug.Log("NotEnoughCoins_Open_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
    }

    public void Close()
    {
        popupPanel.SetActive(false);
        OnCloseCallback?.Invoke();
        if (AdManager._instance)
            AdManager._instance.ShowbannerAd();

        //CloseFirebaseLog();
    }

    public void CloseFirebaseLog()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_Close_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay
    );
    Debug.Log("NotEnoughCoins_Close_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
    }

    public bool IsActive()
    {
        return popupPanel.activeSelf;
    }


    // -----------------------
    //   DIRECT IAP HANDLING
    // -----------------------
    /// <summary>
    /// Called when user clicks the BUY COINS (IAP) button.
    /// </summary>
    public void BuyCoins_IAP()
    {
        Debug.Log("[NotEnoughCoinsPopup] Starting IAP purchase...");

        // Call your IAP system
        // You will replace this with your actual IAP call
        /* IAPManager.Instance.BuyCoins(
            onSuccess: () =>
            {
                Debug.Log("[NotEnoughCoinsPopup] IAP success!");
                OnIAPSuccessCallback?.Invoke();
                Close();
            },
            onFailed: () =>
            {
                Debug.LogWarning("[NotEnoughCoinsPopup] IAP failed!");
                OnIAPFailedCallback?.Invoke();
            }); */
            Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_buyCoins_click_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
            Debug.Log("NotEnoughCoins_buyCoins_click_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
    }


    // -----------------------
    //   WATCH AD HANDLING
    // -----------------------
    public void WatchAdSuccess()
    {
        //OnWatchAdCallback?.Invoke();
        //Close();
        //GameManager.Instance.ReloadLevelNoInterstitial();
        
        Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_VideoSuccess_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay
        );
        Debug.Log("NotEnoughCoins_VideoSuccess_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
        Invoke(nameof(Close),2.5f);
        if (CoinFlyAnimator.Instance)
        {
            CoinFlyAnimator.Instance.Play(reqCoinsTxt.GetComponent<RectTransform>(), coinStatus.GetComponent<RectTransform>(),spawnParent, () =>
            {
                Debug.Log("ShowCoinsAnimation complete");
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 300);
                GameManager.AddCoins();
            });
        } 
    }

    public  void WatchAdToRetryLevel()
    {
        Debug.LogError("WatchAdToRetryLevel CLICKED");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_Video_Click_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay
        );
        Debug.Log("NotEnoughCoins_Video_Click_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
        AdManager._instance.rewardTypeToUnlock = RewardType.retryLevel;
        AdManager._instance.ShowRewardedVideo(result=>
                {
                    if(result)
                    {
                        GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                        AdManager._instance.rewardedvideosuccess = true;
                        WatchAdSuccess();
                    }
                },AdType.Reward);

                

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
        /* infoPopup.SetActive(true);
        optionsPopup.SetActive(false);
        coinsInfoPopup.SetActive(false); */
        GetCoins();
    }

/// Coin Toast
    public FloatingCoinToast popupPrefab;
    public Transform spawnParent;

    public void ShowCoinDeduction(Vector3 worldPosition, int amount)
    {
        if(amount<=0)return;
        var popup = Instantiate(popupPrefab, spawnParent);
        RectTransform rt = popup.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnParent.GetComponent<RectTransform>().rect.center;
        //popup.transform.position = worldPosition;
        popup.Play("-" + amount);
    }
}
