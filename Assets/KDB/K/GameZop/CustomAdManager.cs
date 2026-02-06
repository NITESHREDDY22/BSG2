using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomAdManager : MonoBehaviour
{
    public static CustomAdManager Instance { get; private set; }

    [Header("Global Toggle")]
    public static bool adsEnabled = true;

    public static bool showGameZopInterstitials = true;

    [Header("Common Probabilities (e.g., A=0.3, B=0.5. Sum <= 1.0)")]
    [Range(0f, 1f)] public float listAProb = 0.3f;
    [Range(0f, 1f)] public float listBProb = 0.5f;

    [Header("Data Source")]
    public AdData adData;
    
    [Header("Banner UI")]
    public CanvasGroup bannerCanvasGroup;
    public Image bannerDisplayImage;
    
    [Header("Interstitial UI")]
    public CanvasGroup interstitialCanvasGroup;
    public Image interstitialDisplayImage;

    private AdData.AdItem _activeBanner;
    private AdData.AdItem _activeInterstitial;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        HideAllAds();
    }

    void OnEnable()
    {
        AdManager.OnConfigLoaded += OnConfigLoaded;
    }
    void OnDisable()
    {
        AdManager.OnConfigLoaded -= OnConfigLoaded;
    }

    private void OnConfigLoaded(GameConfig config)
    { 
        listAProb = config.showGameAds;
        listBProb = config.showAstroAds;
        Debug.Log($"<color=cyan>[AdManager]</color> OnConfigLoaded showGameAds: {config.showGameAds}\n,showAstroAds:{config.showAstroAds}");
    }

    public void ShowBanner()
    {
        if (!CanShowAds()) return;

        var selected = DetermineWeightedItem(adData.bannerListA, adData.bannerListB, "Banner");
        if (selected != null)
        {
            _activeBanner = selected;
            bannerDisplayImage.sprite = _activeBanner.image;
            SetCanvasState(bannerCanvasGroup, true);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("GameZop_banner_"+selected.name + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
        }
        else HideBanner();
    }

    public void ShowInterstitial()
    {
        
        bool condition = AdManager._instance.adMobNetworkHandler != null && AdManager._instance.adMobNetworkHandler.adMobRewardedInterstitial != null &&
                AdManager._instance.adMobNetworkHandler.adMobRewardedInterstitial.CanShowAd();

        Debug.Log($"[Ads][GameZop]ShowInterstitial can show{condition}");
        if(condition)return;
        if (!CanShowAds()) return;
        if(!showGameZopInterstitials)return;

        var selected = DetermineWeightedItem(adData.interstitialListA, adData.interstitialListB, "Interstitial");
        if (selected != null)
        {
            _activeInterstitial = selected;
            interstitialDisplayImage.sprite = _activeInterstitial.image;
            SetCanvasState(interstitialCanvasGroup, true);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("GameZop_Interstitial_"+selected.name + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
        }
        else HideInterstitial();
    }

    // --- PROBABILITY & SELECTION LOGIC ---

    private AdData.AdItem DetermineWeightedItem(List<AdData.AdItem> listA, List<AdData.AdItem> listB, string label)
    {
        float roll = Random.value; // Returns 0.0 to 1.0
        
        // 1. Check List A: Ignored if probability is 0 OR list is empty
        bool canUseA = listAProb > 0 && listA != null && listA.Count > 0;
        if (canUseA && roll < listAProb)
        {
            Debug.Log($"<color=cyan>[{label}]</color> Selected: List A (Roll: {roll:F2} < {listAProb})");
            return GetRandom(listA);
        }

        // 2. Check List B: Ignored if probability is 0 OR list is empty
        // We check against the CUMULATIVE range (A + B)
        bool canUseB = listBProb > 0 && listB != null && listB.Count > 0;
        if (canUseB && roll < (listAProb + listBProb))
        {
            Debug.Log($"<color=cyan>[{label}]</color> Selected: List B (Roll: {roll:F2} < {(listAProb + listBProb):F2})");
            return GetRandom(listB);
        }

        // 3. Fallback: Both 0, Roll too high, or selected list was empty
        Debug.Log($"<color=white>[{label}]</color> Result: No Ad (Roll: {roll:F2})");
        return null;
    }

    private bool CanShowAds() => adsEnabled && adData != null;

    private AdData.AdItem GetRandom(List<AdData.AdItem> list) => list[Random.Range(0, list.Count)];

    // --- UI & UTILS ---

    public void HideBanner() => SetCanvasState(bannerCanvasGroup, false);
    public void HideInterstitial() => SetCanvasState(interstitialCanvasGroup, false);
    public void HideAllAds() { HideBanner(); HideInterstitial(); }

    private void SetCanvasState(CanvasGroup cg, bool show)
    {
        if (cg == null) return;
        cg.alpha = show ? 1 : 0;
        cg.blocksRaycasts = show;
        cg.interactable = show;
    }

    public void OnBannerClicked() => HandleUrl(_activeBanner?.url);
    public void OnInterstitialClicked() => HandleUrl(_activeInterstitial?.url);

    private void HandleUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return;
#if UNITY_ANDROID && !UNITY_EDITOR
        OpenChromeTab(url);
#else
        Application.OpenURL(url);
#endif
    Firebase.Analytics.FirebaseAnalytics.LogEvent("GameZop_OpenURL_url"+url + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
    }

    private void OpenChromeTab(string url)
    {
        try {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var builder = new AndroidJavaObject("androidx.browser.customtabs.CustomTabsIntent$Builder"))
            using (var intent = builder.Call<AndroidJavaObject>("build"))
            {
                var uriClass = new AndroidJavaClass("android.net.Uri");
                var uri = uriClass.CallStatic<AndroidJavaObject>("parse", url);
                intent.Call("launchUrl", activity, uri);
            }
        } catch { Application.OpenURL(url); }
    }
}