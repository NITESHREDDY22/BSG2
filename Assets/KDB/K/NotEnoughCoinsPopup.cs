using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotEnoughCoinsPopup : MonoBehaviour
{
    public static NotEnoughCoinsPopup Instance;

    [Header("Popup Root")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField]private Text currentCoinsTxt,coinsToBuyTxt,reqCoinsTxt;
    [SerializeField]private GameObject infoPopup,optionsPopup,coinsInfoPopup;

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
        reqCoinsTxt.text = ""+(Global.coinsToReload-GameManager.GetCoins());
        popupPanel.SetActive(true);
        ShowNotEnoughCoinsPopup();
        OnOpenCallback?.Invoke();
        if (AdManager._instance)
            AdManager._instance.HidebannerAd();
    }

    public void Close()
    {
        popupPanel.SetActive(false);
        OnCloseCallback?.Invoke();
        if (AdManager._instance)
            AdManager._instance.ShowbannerAd();
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
    }


    // -----------------------
    //   WATCH AD HANDLING
    // -----------------------
    public void WatchAdSuccess()
    {
        //OnWatchAdCallback?.Invoke();
        Close();
        GameManager.Instance.ReloadLevelNoInterstitial();
    }

    public  void WatchAdToRetryLevel()
    {
        Debug.LogError("WatchAdToRetryLevel CLICKED");
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
        infoPopup.SetActive(true);
        optionsPopup.SetActive(false);
        coinsInfoPopup.SetActive(false);
    }

/// Coin Toast
    public FloatingCoinToast popupPrefab;
    public Transform spawnParent;

    public void ShowCoinDeduction(Vector3 worldPosition, int amount)
    {
        var popup = Instantiate(popupPrefab, spawnParent);
        RectTransform rt = popup.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnParent.GetComponent<RectTransform>().rect.center;
        //popup.transform.position = worldPosition;
        popup.Play("-" + amount);
    }
}
