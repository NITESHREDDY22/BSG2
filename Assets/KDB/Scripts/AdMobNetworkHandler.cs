using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public enum AdType
{
    Launch,
    Interstital,
    Reward,
    RewardContinue,
    RewardedInterStitial,
    SecondaryInterstitial,
    BannerAd,
    AppOpenAd,
    SecondaryReward,

}

public enum AdContent
{
    AdMobLaunchRequested,
    AdMobInterstitalRequested,
    AdMobRewardRequested,
    AdMobContinueRewardRequested,
    AdMobRewardedInterstitialRequested,

    AdMobLaunchAdLoaded,
    AdMobInterstitalAdLoaded,
    AdMobRewardAdLoaded,    
    AdMobRewardedInterstitialAdLoaded,

    AdMobLaunchLoadFailed,
    AdMobInterstitalLoadFailed,
    AdMobRewardLoadFailed,
    AdMobRewardedInterstitialLoadFailed,

    AdMobLaunchShown,
    AdMobInterstitalShown,
    AdMobRewardShown,
    AdMobContinueRewardShown,
    AdMobRewardedInterstitialShown,

    AdmobInterstitialImpression,
    AdmobRewardImpression,
    AdmobRewardInterstitialImpression,


    AdmobInterstitialClicked,
    AdmobRewardClicked,
    AdmobRewardInterstitialClicked,




    LevelPlayLaunchRequested,
    levelPlayInterstitalRequested,
    levelPlayRewardRequested,

    LevelPlayLaunchAdLoaded,
    levelPlayInterstitalAdLoaded,
    levelPlayRewardAdLoaded,

    LevelPlayLaunchLoadFailed,
    levelPlayInterstitalAdLoadFailed,
    levelPlayRewardLoadFailed,

    LevelPlayLaunchShown,
    levelPlayInterstitalshown,
    levelPlayRewardShown,  

    levelPlayInterstitialClicked,
    levelPlayRewardClicked,

    SessionAdsBefore3Mins,
    SessionAdsBefore6Mins,
    SessionAdsBefore9Mins,
    SessionAdsafter9Mins,

    SessionAdClicksBefore3Mins,
    SessionAdClicksBefore6Mins,
    SessionAdClicksBefore9Mins,
    SessionAdClicksafter9Mins,

    AdmobBannerLoadFailed,
    AdmobAppopenAdLoadFailed,

    AdmobAppopenSplashShown,
    AdmobAppopenAppForeGroundShown

}

public enum AdMode
{
    Requested,
    Loaded,
    Shown,
    Impression,
    Clicked
}

public enum SuccessStatus
{
    Success,
    Failed
}

public class AdMobNetworkHandler :MonoBehaviour
{

    public InterstitialAd adMobInterstitial, adMobLaunchInterstitial;
    public RewardedInterstitialAd adMobRewardedInterstitial;
    public RewardedAd adMobRewardBasedVideo;
    public RewardedAd adMobContinueRewardBasedVideo;
    public BannerView bannerView;
    public AppOpenAd appOpenAd;


    private bool isAdMobInitialized;
    private Action<AdType> OnAdClosedAction;

    private string adMobInterstitialId;
    private string adMobLaunchinterStitialId;
    private string adMobRewardBasedVideoId;
    private string adMobContinueRewardBasedVideoId;
    private string adMobRewardedInterstitialVideoId;
    private string adMobBannerAdId;
    private string adappOpenAdId;




    private int adDelayTimer = Global.adRetryTime;

    public class AdItem
    {
        public string AdID;
        public InterstitialAd Interstitial;
        public bool isAdReady;
        public bool isAdRequested;
        public RewardedAd RewardedAd;
        public RewardedInterstitialAd adMobRewardedInterstitial;
        public BannerView bannerView;
        public AppOpenAd appOpenAd;
        public AdItem(string AdID, InterstitialAd Interstitial, bool isAdReady,bool isAdRequested)
        {
            this.AdID = AdID;
            this.Interstitial = Interstitial;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }

        public AdItem(string AdID, RewardedAd RewardedAd, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.RewardedAd = RewardedAd;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }

        public AdItem(string AdID, RewardedInterstitialAd adMobRewardedInterstitial, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.adMobRewardedInterstitial = adMobRewardedInterstitial;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }

        public AdItem(string AdID, BannerView bannerView, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.bannerView = bannerView;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }
        public AdItem(string AdID, AppOpenAd appOpenAd, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.appOpenAd = appOpenAd;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }
    }

    private Dictionary<AdType, AdItem> keyValuePairs = new Dictionary<AdType, AdItem>();

    private Action<bool> callBack=null;
    private Action<bool> rewardedInterStitialcallBack = null;
    public Action<bool> rewardedInterStitialrequestcallBack = null;
    public Action<bool> rewardedrequestcallBack = null;
    public bool isInterstitialLoaded;
    [SerializeField] DailyLoginHandler dailyLoginHandler;


    private AdConfig adConfig;

    private DateTime timeSinceGameLoaded;
    private DateTime _expireTime;

    private void Start()
    {
        timeSinceGameLoaded = DateTime.UtcNow;
    } 
    public void Init()
    {
        if (!keyValuePairs.ContainsKey(AdType.Launch))
        {
            keyValuePairs.Add(AdType.Launch, new AdItem(adMobLaunchinterStitialId, adMobLaunchInterstitial, false, false));
        }
        if (!keyValuePairs.ContainsKey(AdType.Interstital))
        {
            keyValuePairs.Add(AdType.Interstital, new AdItem(adMobInterstitialId, adMobInterstitial, false, false));
        }

        if (!keyValuePairs.ContainsKey(AdType.Reward))
        {
            keyValuePairs.Add(AdType.Reward, new AdItem(adMobRewardBasedVideoId, adMobContinueRewardBasedVideo, false, false));
        }

        if (!keyValuePairs.ContainsKey(AdType.RewardedInterStitial))
        {
            keyValuePairs.Add(AdType.RewardedInterStitial, new AdItem(adMobRewardedInterstitialVideoId, adMobRewardedInterstitial, false, false));
        }

        if (!keyValuePairs.ContainsKey(AdType.BannerAd))
        {
            keyValuePairs.Add(AdType.BannerAd, new AdItem(adMobBannerAdId, bannerView, false, false));
        }

        if (!keyValuePairs.ContainsKey(AdType.AppOpenAd))
        {
            keyValuePairs.Add(AdType.AppOpenAd, new AdItem(adappOpenAdId, appOpenAd, false, false));
        }
    }
    public void Initialize(bool isAdMobInitialized)
    {
        this.isAdMobInitialized = isAdMobInitialized;       
        OnAdClosedAction = OnAdClosed;

    }

    public void SetAdConfig(AdConfig adConfig)
    {
        this.adConfig = adConfig;
        SetInterStitalId();
        SetLaunchInterStitalId();
        SetRewardId();        
        SetRewardedInterstitalId();
        SetBannerlId();
        SeAppOpenAdId();
    }
    public void SetLaunchInterStitalId()
    {

        this.adMobLaunchinterStitialId = GetAdUnitId(AdType.Launch);
    }

    public void SetInterStitalId(string adMobInterstitialId="")
    {   
       this.adMobInterstitialId = string.IsNullOrEmpty(adMobInterstitialId)? GetAdUnitId(AdType.Interstital):adMobInterstitialId;

        if(!string.IsNullOrEmpty(adMobInterstitialId))
        {
            if (keyValuePairs.ContainsKey(AdType.Interstital))
            {
                keyValuePairs[AdType.Interstital].AdID = this.adMobInterstitialId;
            }
        }
    }

    public void SetRewardId()
    {
        this.adMobRewardBasedVideoId = GetAdUnitId(AdType.Reward); ;
        
    }

    public void SetRewardId(string AdId)
    {
        this.adMobRewardBasedVideoId = AdId ;
        if (!string.IsNullOrEmpty(adMobRewardBasedVideoId))
        {
            if (keyValuePairs.ContainsKey(AdType.Reward))
            {
                keyValuePairs[AdType.Reward].AdID = this.adMobRewardBasedVideoId;
            }
        }
    }

    public void SetRewardedInterstitalId()
    {
        this.adMobRewardedInterstitialVideoId = GetAdUnitId(AdType.RewardedInterStitial);
    }
    public void SetBannerlId()
    {
        this.adMobBannerAdId = GetAdUnitId(AdType.BannerAd);
    }

    public void SeAppOpenAdId()
    {
        this.adappOpenAdId = GetAdUnitId(AdType.AppOpenAd);
    }

    string GetAdUnitId(AdType adType)
    {
        AdUnitConfig adUnitConfig = adConfig.adConfigs.Find(x => x.AdType == adType);
        string adUnitId = string.Empty;

        if (adUnitConfig != null)
        {
            adUnitId = adUnitConfig.AdUnitId;
#if UNITY_EDITOR
            adUnitId = string.Empty;
#endif
            if (adUnitConfig.ActiveStatus == ActiveStatus.Deactive)
            {
                adUnitId = string.Empty;
            }
            return adUnitId;
        }

        return adUnitId;
    }

    public void RequestInterstitial(AdType adType)
    {
        AdItem item = null;
        //Debug.LogError("Asdf RequestLaunchInterstitial 1111"+ isAdMobInitialized);

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        // Debug.LogError("Asdf RequestLaunchInterstitial 1111----"+item + " "+ item.AdID+ ""+ item.isAdRequested);

        if (GameConstants.GetNoAdsStatus)
            return;

        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested )
            return;

        //B Debug.LogError("Asdf RequestLaunchInterstitial 2222  ===="+item.AdID+ " type=== "+adType);
        
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                if (item.Interstitial != null)
                {
                    item.Interstitial.Destroy();
                    item.Interstitial = null;
                    item.isAdRequested = true;
                    //Debug.LogError("Asdf RequestLaunchInterstitial 3333");

                    FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Success);
                }
                Debug.LogError("Admob Request InterStital called  " + adType + "ID "+adItem.AdID);
                InterstitialAd.Load(item.AdID, new AdRequest(),
                        (InterstitialAd ad, LoadAdError loadAdError) =>
                        {
                            if (loadAdError != null)
                            {
                                Debug.LogError("RequestInterstitial ad." +adItem.AdID);
                                Debug.LogError("Interstitial ad failed to load with error: " + loadAdError.GetMessage()+adType);
                                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                {
                                    OnAdLoadFailed(adType);
                                    FireBaseActions(adType == AdType.Launch ? 
                                        AdContent.AdMobLaunchLoadFailed : 
                                        AdContent.AdMobInterstitalLoadFailed,
                                        AdMode.Requested, SuccessStatus.Failed);
                                    //Debug.LogError("Asdf RequestLaunchInterstitial 44444");
                                    Debug.LogError("RequestInterstitial ad." + loadAdError.GetMessage()+" "+ adItem.AdID);
                                    if(adType==AdType.Interstital)
                                    {
                                        isInterstitialLoaded = false;
                                    }

                                });
                                return;
                            }
                            else if (ad == null)
                            {
                                Debug.LogError("Interstitial ad failed to load." + adType);
                                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                {
                                    FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchLoadFailed : AdContent.AdMobInterstitalLoadFailed, 
                                        AdMode.Requested, SuccessStatus.Failed);
                                    //Debug.LogError("Asdf RequestLaunchInterstitial 55555");
                                      OnAdLoadFailed(adType);
                                    Debug.LogError("RequestInterstitial ad. FAILED" +  adItem.AdID);

                                    if (adType == AdType.Interstital)
                                    {
                                        isInterstitialLoaded = false;
                                    }

                                });
                                return;
                            }

                            //Debug.LogError("Asdf RequestLaunchInterstitial 666666");

                            //B Debug.LogError("Interstitial ad loaded." + adType + "  "+item.AdID);
                            if (!item.isAdReady)
                            {
                                item.Interstitial = ad;
                                item.isAdReady = true;
                                item.Interstitial.OnAdFullScreenContentClosed += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        OnAdClosed(adType);
                                    });
                                };

                                if (adType == AdType.Launch)
                                {
                                    adMobLaunchInterstitial = ad;
                                }
                                if (adType == AdType.Interstital)
                                {
                                    adMobInterstitial = ad;

                                    isInterstitialLoaded = true;
                                }

                                item.Interstitial.OnAdImpressionRecorded += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        OnAdImpression(adType);
                                    });
                                };

                                item.Interstitial.OnAdClicked += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        OnAdClicked(adType);
                                    });
                                };

                                FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchAdLoaded : AdContent.AdMobInterstitalAdLoaded,
                                    AdMode.Loaded, SuccessStatus.Success);

                                Debug.LogError("RequestInterstitial ad. success" + adType + "ID " + adItem.AdID);

                                item.Interstitial.OnAdPaid += (AdValue adValue) =>
                                {
                                    float revenue = adValue.Value / 1_000_000f; // Convert micros to dollars
                                    string currency = adValue.CurrencyCode;

                                    // Check if revenue is positive and currency is valid
                                    if (revenue > 0 && !string.IsNullOrEmpty(currency))
                                    {
                                        // Construct and send the Singular AdMon Event
                                        //TODO 
                                        
                                        Singular.SingularAdData data = new Singular.SingularAdData(
                                            "Admob",
                                            currency,
                                            revenue
                                        );
                                        Singular.SingularSDK.AdRevenue(data);
                                        
                                        // Log the revenue data for debugging purposes
                                        //Debug.Log($"Ad Revenue reported to Singular: {data}");
                                    }
                                    else
                                    {
                                        Debug.LogError($"Invalid ad revenue data: revenue = {revenue}, currency = {currency}");
                                    }
                                };
                            }

                        });
            }
        });
    }

    public void ShowInterstitialAd(AdType adType, Action<bool> callBack = null)
    {
        //Debug.LogError("Asdf ShowInterstitialAd 0000");
        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        //Debug.LogError("Asdf ShowInterstitialAd 11111");

        if (GameConstants.GetNoAdsStatus)
            return;


        if (item.Interstitial != null && item.Interstitial.CanShowAd())
        {
            Debug.LogError("Asdf ShowInterstitialAd " + adType);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.Interstitial.Show();
                item.isAdReady = false;
                item.isAdRequested = false;
                callBack?.Invoke(true);
                FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Success);

            });
        }
        else
        {
            //Debug.LogError("Asdf ShowInterstitialAd 33333");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.LogError("ShowInterstitialAd. failed" + adType);

                callBack?.Invoke(false);
                OnAdLoadFailed(adType);
                //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Failed);

            });

        }
    }


    public void StopPreviousCoroutine()
    {
        if (insterstitalCoroutine != null)
        {
            StopCoroutine(insterstitalCoroutine);
        }
    }

    private Coroutine insterstitalCoroutine;
    void OnAdLoadFailed(AdType adType)
    {
      
        var item = keyValuePairs[adType];
        if (item != null)
        {
            item.isAdRequested = false;
            item.isAdReady = false;
        }
        if (adType != AdType.Launch)
        {
            //B Debug.LogError("Admob RequestWithDelay InterStital called" + adType);
           
        }
        else if(adType==AdType.Interstital)
        {
             Debug.LogError("RequestWithDelay Interstitial $$$$$$$$.");
             RequestWithDelay(adDelayTimer, () =>
            {
                RequestInterstitial(adType);
            },
            (x)=>
            {
                insterstitalCoroutine = x;
            });
        }
        else
        {
            RequestWithDelay(adDelayTimer, () =>
            {
                // Debug.LogError("RequestWithDelay Interstitial ad cannot be shown.");
                RequestInterstitial(adType);
            });
        }

    }

    void OnAdClosed(AdType adType)
    {
        switch (adType)
        {
            case AdType.Launch:
                break;

            case AdType.Interstital:
                RequestInterstitial(AdType.Interstital);
                AdManager.OnIngameAdClosed?.Invoke();
                break;
        }
    }

    public void RequestRewardBasedVideo(AdType adType = AdType.Reward)
    {

        AdItem item = null;

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                if (item.RewardedAd != null)
                {
                    item.RewardedAd.Destroy();
                    item.RewardedAd = null;
                    item.isAdRequested = true;
                   //TODO : FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Requested, SuccessStatus.Success);

                }

                Debug.LogError("Admob Request Reward called"+ adItem.AdID);

                RewardedAd.Load(item.AdID, new AdRequest(),
                   (RewardedAd ad, LoadAdError loadError) =>
                   {
                       if (loadError != null)
                       {
                           MobileAdsEventExecutor.ExecuteInUpdate(() =>
                           {
                               item.isAdRequested = false;
                               item.isAdReady = false;
                               RequestAgain(adType);
                               Debug.LogError("asdf Admob Rewarded ad failed to load with error: "+ item.AdID +" ..." + loadError.GetMessage()+adType);
                              

                               FireBaseActions( AdContent.AdMobRewardLoadFailed, AdMode.Requested, SuccessStatus.Failed);
                               if (adType == AdType.Reward)
                                   this.rewardedrequestcallBack?.Invoke(false);

                           });
                           return;
                       }
                       else if (ad == null)
                       {
                           MobileAdsEventExecutor.ExecuteInUpdate(() =>
                           {
                               item.isAdRequested = false;
                               item.isAdReady = false;
                                RequestAgain(adType);
                               Debug.LogError("asdf Admob  Rewarded ad failed to load." + adType+ " ..." + item.AdID );
                               FireBaseActions( AdContent.AdMobRewardLoadFailed, AdMode.Requested, SuccessStatus.Failed);
                               if (adType == AdType.Reward)
                                   this.rewardedrequestcallBack?.Invoke(false);

                           });
                           return;
                       }

                        Debug.LogError("asdf Admob  Rewarded ad loaded. "+adItem.AdID);
                       if (!item.isAdReady)
                       {
                           item.RewardedAd = ad;
                           item.isAdReady = true;

                           if (adType == AdType.Reward)
                           {
                               adMobRewardBasedVideo = ad;
                               this.rewardedrequestcallBack?.Invoke(true);

                           }
                           if (adType == AdType.RewardContinue)
                           {
                               adMobContinueRewardBasedVideo = ad;
                           }

                           ad.OnAdFullScreenContentClosed += () =>
                           {
                               //B Debug.LogError("asdf Admob  Rewarded Ad full screen content closed.");

                               // Reload the ad so that we can show another as soon as possible.

                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {

                                   item.isAdReady = false;
                                   item.isAdRequested = false;
                                   RequestRewardBasedVideo(adType);
                                   this.callBack?.Invoke(false);
                                   //FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Success);

                               });
                           };
                           // Raised when the ad failed to open full screen content.
                           ad.OnAdFullScreenContentFailed += async (AdError error) =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   Debug.LogError("asdf Admob  Rewarded ad failed to open full screen content " +
                                                  "with error : " + error);
                                   // Reload the ad so that we can show another as soon as possible.

                                   //Debug.LogError("OnAdFullScreenContentFailed. should add delay request");
                                   item.isAdReady = false;
                                   item.isAdRequested = false;
                                   this.callBack?.Invoke(false);
                                   RequestAgain(adType);
                                  // FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Failed);

                               });

                           };

                           ad.OnAdImpressionRecorded += () =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   OnAdImpression(adType);
                               });
                           };

                           ad.OnAdClicked += () =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   OnAdClicked(adType);
                               });
                           };
                           FireBaseActions( AdContent.AdMobRewardAdLoaded, AdMode.Loaded, SuccessStatus.Success);

                           ad.OnAdPaid += (AdValue adValue) =>
                           {
                               float revenue = adValue.Value / 1_000_000f; // Convert micros to dollars
                               string currency = adValue.CurrencyCode;

                               // Check if revenue is positive and currency is valid
                               if (revenue > 0 && !string.IsNullOrEmpty(currency))
                               {
                                   //TODO 
                                   // Construct and send the Singular AdMon Event
                                   
                                   Singular.SingularAdData data = new Singular.SingularAdData(
                                       "Admob",
                                       currency,
                                       revenue
                                   );
                                   Singular.SingularSDK.AdRevenue(data);

                                   // Log the revenue data for debugging purposes
                                   Debug.Log($"Ad Revenue reported to Singular: {data}");
                                   
                               }
                               else
                               {
                                   Debug.LogError($"Invalid ad revenue data: revenue = {revenue}, currency = {currency}");
                               }
                           };
                       }
                   });
            }
        });
    }
    public void ShowAdmobRewardedVideo(Action<bool> callBack,AdType adType=AdType.Reward)
    {

#if UNITY_EDITOR
        callBack?.Invoke(true);
#endif

        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }

        this.callBack = callBack;
        if (item!=null && item.RewardedAd!= null && item.RewardedAd.CanShowAd())
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {                
                item.RewardedAd.Show((Reward reward) =>
                {
                    if (reward != null)
                    {
                        Debug.LogError("asdf Admob  Rewarded ad granted a reward: "+adItem.AdID);
                        this.callBack?.Invoke(true);
                    }
                });
            });
        }  
        else
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                callBack?.Invoke(false);
                Debug.LogError("ShowAdmobRewardedVideo. failed" + adItem.AdID);

            });
           // FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Failed);
        }
    }

    public void RequestRewardInterstitial(AdType adType = AdType.RewardedInterStitial,Action<bool> callback=null)
    {

        AdItem item = null;

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;

        //Debug.LogError("Admob Request RequestRewardInterstitial called" + adType);       
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                if (item.adMobRewardedInterstitial != null)
                {
                    item.adMobRewardedInterstitial.Destroy();
                    item.adMobRewardedInterstitial = null;
                    item.isAdRequested = true;
                    //TODO : FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Requested, SuccessStatus.Success);

                }

                 Debug.LogError("Admob Request RequestRewardInterstitial called" + adType);

                RewardedInterstitialAd.Load(item.AdID, new AdRequest(),
                   (RewardedInterstitialAd ad, LoadAdError loadError) =>
                   {
                       if (loadError != null)
                       {
                           MobileAdsEventExecutor.ExecuteInUpdate(() =>
                           {
                               item.isAdRequested = false;
                               item.isAdReady = false;
                               RequestWithDelay(adDelayTimer, () =>
                               {
                                   //Debug.LogError("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");
                                     RequestRewardInterstitial(adType);
                               });
                               this.rewardedInterStitialrequestcallBack?.Invoke(false);
                               Debug.LogError("asdf Admob RequestRewardInterstitial ad failed to load with error: " + loadError.GetMessage() + adType);
                                FireBaseActions( AdContent.AdMobRewardedInterstitialLoadFailed, AdMode.Requested, SuccessStatus.Failed);

                           });
                           return;
                       }
                       else if (ad == null)
                       {
                           MobileAdsEventExecutor.ExecuteInUpdate(() =>
                           {
                               item.isAdRequested = false;
                               item.isAdReady = false;
                               RequestWithDelay(adDelayTimer, () =>
                               {
                                   //Debug.LogError("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");
                                   RequestRewardInterstitial(adType);
                               });
                                this.rewardedInterStitialrequestcallBack?.Invoke(false);
                                Debug.LogError("asdf Admob  RequestRewardInterstitial ad failed to load." + adType);
                               FireBaseActions(AdContent.AdMobRewardedInterstitialLoadFailed, AdMode.Requested, SuccessStatus.Failed);

                           });
                           return;
                       }

                       //B Debug.LogError("asdf Admob  RequestRewardInterstitial ad loaded. " + adType);
                       if (!item.isAdReady)
                       {
                           item.adMobRewardedInterstitial = ad;
                           item.isAdReady = true;
                           this.rewardedInterStitialrequestcallBack?.Invoke(true);

                           if (adType == AdType.RewardedInterStitial)
                           {
                               adMobRewardedInterstitial = ad;
                           }                         

                           ad.OnAdFullScreenContentClosed += () =>
                           {
                               //B Debug.LogError("asdf Admob  Rewarded Ad full screen content closed.");

                               // Reload the ad so that we can show another as soon as possible.
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   this.rewardedInterStitialcallBack?.Invoke(false);
                                   item.isAdReady = false;
                                   item.isAdRequested = false;
                                   RequestRewardInterstitial(adType);                                 
                                   //FireBaseActions( AdContent.AdMobRewardedInterstitialShown, AdMode.Shown, SuccessStatus.Success);

                               });
                           };
                           // Raised when the ad failed to open full screen content.
                           ad.OnAdFullScreenContentFailed += async (AdError error) =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   Debug.LogError("asdf Admob  RequestRewardInterstitial ad failed to open full screen content " +
                                                  "with error : " + error);
                                   // Reload the ad so that we can show another as soon as possible.

                                   //Debug.LogError("OnAdFullScreenContentFailed. should add delay request");
                                   item.isAdReady = false;
                                   item.isAdRequested = false; 
                                   this.rewardedInterStitialcallBack?.Invoke(false);
                                   RequestWithDelay(adDelayTimer, () =>
                                   {
                                       //Debug.LogError("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");
                                       RequestRewardInterstitial(adType);
                                   });
                                   //TODO :FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Failed);

                               });

                           };

                           ad.OnAdImpressionRecorded += () =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   OnAdImpression(adType);
                               });
                           };

                           ad.OnAdClicked += () =>
                           {
                               MobileAdsEventExecutor.ExecuteInUpdate(() =>
                               {
                                   OnAdClicked(adType);
                               });
                           };
                           FireBaseActions(AdContent.AdMobRewardedInterstitialRequested, AdMode.Loaded, SuccessStatus.Success);
                           Debug.LogError("RequestRewardInterstitial." + adItem.AdID);

                           ad.OnAdPaid += (AdValue adValue) =>
                           {
                               float revenue = adValue.Value / 1_000_000f; // Convert micros to dollars
                               string currency = adValue.CurrencyCode;

                               // Check if revenue is positive and currency is valid
                               if (revenue > 0 && !string.IsNullOrEmpty(currency))
                               {
                                   //TODO 
                                   // Construct and send the Singular AdMon Event

                                   Singular.SingularAdData data = new Singular.SingularAdData(
                                       "Admob",
                                       currency,
                                       revenue
                                   );
                                   Singular.SingularSDK.AdRevenue(data);

                                   // Log the revenue data for debugging purposes
                                   Debug.Log($"Ad Revenue reported to Singular: {data}");

                               }
                               else
                               {
                                   Debug.LogError($"Invalid ad revenue data: revenue = {revenue}, currency = {currency}");
                               }
                           };

                       }
                   });
            }
        });
    }
    public void ShowRewardInterstitial(Action<bool> callBack, AdType adType = AdType.RewardedInterStitial)
    {

#if UNITY_EDITOR
        callBack?.Invoke(true);
#endif

        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
       this.rewardedInterStitialcallBack = callBack;
        if (item != null && item.adMobRewardedInterstitial != null && item.adMobRewardedInterstitial.CanShowAd())
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.adMobRewardedInterstitial.Show((Reward reward) =>
                {
                    if (reward != null)
                    {
                        Debug.LogError("asdf Admob  RequestRewardInterstitial ad granted a reward: " + adItem.AdID);
                        callBack?.Invoke(true);
                        AdManager.OnIngameAdClosed?.Invoke();
                    }
                });
            });
        }
        else
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                callBack?.Invoke(false);
                Debug.LogError("ShowRewardInterstitial failed." + adItem.AdID);

            });
           // FireBaseActions( AdContent.AdMobRewardedInterstitialShown, AdMode.Shown, SuccessStatus.Failed);
        }
    }
    void RequestAgain(AdType adType)
    {
        //B Debug.LogError("Admob RequestWithDelay reward called");       
        RequestWithDelay(adDelayTimer, () =>
        {
            //Debug.LogError("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");
            RequestRewardBasedVideo(adType);
        });
    }



    int count = 0;

    private bool isShowBannerAdCalled = false;


    public void CreateBannerView()
    {
        AdItem item = null;
        //Debug.LogError("Asdf RequestLaunchInterstitial 1111"+ isAdMobInitialized);

        if (keyValuePairs.TryGetValue(AdType.BannerAd, out AdItem adItem))
        {
            item = adItem;
        }
        // Debug.LogError("Asdf RequestLaunchInterstitial 1111----"+item + " "+ item.AdID+ ""+ item.isAdRequested);

        if (GameConstants.GetNoAdsStatus)
            return;

        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized )
            return;

        //B Debug.LogError("Asdf RequestLaunchInterstitial 2222  ===="+item.AdID+ " type=== "+adType);
        if (bannerView != null)
        {
            DestroyAd();
        }
        //Normal banner ad
        // Create a 320x50 banner at top of the screen
        //bannerView = new BannerView(item.AdID, AdSize.Banner, AdPosition.BottomLeft);


        //Adaptive Banner Ad
        // [START create_anchored_adaptive_banner_view]
        // Get the device safe width in density-independent pixels.
        
        int deviceWidth =  (int)(MobileAds.Utils.GetDeviceSafeWidth()*1f);

       // float width = Screen.width * 0.5f; // 80% of screen width
       // int deviceWidth = Mathf.RoundToInt(width / Screen.dpi * 160); // Convert px to dp if needed
        // Define the anchored adaptive ad size.
        AdSize adaptiveSize =AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);
        // Create an anchored adaptive banner view.
        bannerView = new BannerView(item.AdID, adaptiveSize, AdPosition.Bottom);
        // [END create_anchored_adaptive_banner_view]
        

        bannerView.OnBannerAdLoaded += () =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.isAdReady = true;
                item.bannerView = bannerView;
                //TODO 
                if (!isShowBannerAdCalled)
                    HideBannerView();
            });
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.LogError("RequestWithDelay OnBannerAdLoadFailed."+error);

                FireBaseActions(AdContent.AdmobBannerLoadFailed, AdMode.Requested, SuccessStatus.Failed);
                RequestWithDelay(adDelayTimer, () =>
                {
                   
                  RequestBannerView();
                    
                });
            });
        };

        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            float revenue = adValue.Value / 1_000_000f; // Convert micros to dollars
            string currency = adValue.CurrencyCode;

            // Check if revenue is positive and currency is valid
            if (revenue > 0 && !string.IsNullOrEmpty(currency))
            {
                // Construct and send the Singular AdMon Event
                //TODO 
                
                Singular.SingularAdData data = new Singular.SingularAdData(
                    "Admob",
                    currency,
                    revenue
                );
                Singular.SingularSDK.AdRevenue(data);

                // Log the revenue data for debugging purposes
                Debug.Log($"Ad Revenue reported to Singular: {data}");
                
            }
            else
            {
                Debug.LogError($"Invalid ad revenue data: revenue = {revenue}, currency = {currency}");
            }
        };
    }

    private void DestroyAd()
    {
        if (bannerView != null)
        {
            Debug.LogError("Destroying banner view.");
            bannerView.Destroy();
            bannerView = null;         
        }
    }

    public void RequestBannerView()
    {

        AdItem item = null;
        //Debug.LogError("Asdf RequestLaunchInterstitial 1111"+ isAdMobInitialized);

        if (keyValuePairs.TryGetValue(AdType.BannerAd, out AdItem adItem))
        {
            item = adItem;
        }
        // Debug.LogError("Asdf RequestLaunchInterstitial 1111----"+item + " "+ item.AdID+ ""+ item.isAdRequested);

        if (GameConstants.GetNoAdsStatus)
            return;

        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;

        //B Debug.LogError("Asdf RequestLaunchInterstitial 2222  ===="+item.AdID+ " type=== "+adType);


        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                if (bannerView == null)
                {
                    CreateBannerView();
                }

                if (item.bannerView != null)
                {
                    item.isAdRequested = true;
                    item.bannerView = null;
                    //Debug.LogError("Asdf RequestLaunchInterstitial 3333");

                    //FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Success);
                }
                //BDebug.LogError("Admob Request InterStital called  " + adType);

                var adRequest = new AdRequest();

                // send the request to load the ad.
                Debug.LogError("Loading banner ad.");
                bannerView.LoadAd(adRequest);
                Debug.LogError("Loading banner ad...00000");
               
            }
        });
    }

    public void ShowBannerAd()
    {
        AdItem item = null;
        if (keyValuePairs.TryGetValue(AdType.BannerAd, out AdItem adItem))
        {
            item = adItem;
        }
        //Debug.LogError("Asdf ShowInterstitialAd 11111");

        if (GameConstants.GetNoAdsStatus)
            return;


        if (item.bannerView != null && item.isAdReady)
        {
            //Debug.LogError("Asdf ShowInterstitialAd 22222");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.bannerView.Show();   
                isShowBannerAdCalled = true;
            });
        }
        else
        {
            //Debug.LogError("Asdf ShowInterstitialAd 33333");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                isShowBannerAdCalled = false;

                Debug.LogError("banner ad cannot be shown.");
                RequestWithDelay(adDelayTimer, () =>
                {
                    // Debug.LogError("RequestWithDelay Interstitial ad cannot be shown.");
                      RequestBannerView();
                });
                //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Failed);

            });

        }
    }

    public void HideBannerView()
    {

        AdItem item = null;
        if (keyValuePairs.TryGetValue(AdType.BannerAd, out AdItem adItem))
        {
            item = adItem;
        }
        if (item.bannerView != null )
        {
            item.bannerView.Hide();
            isShowBannerAdCalled = false;

        }
    }

    public void RequestAppOpenAd(Action<bool> appOpenAdLoadCallback=null)
    {
        AdItem item = null;
        //Debug.LogError("Asdf RequestLaunchInterstitial 1111"+ isAdMobInitialized);

        if (keyValuePairs.TryGetValue(AdType.AppOpenAd, out AdItem adItem))
        {
            item = adItem;
        }
        // Debug.LogError("Asdf RequestLaunchInterstitial 1111----"+item + " "+ item.AdID+ ""+ item.isAdRequested);

        if (GameConstants.GetNoAdsStatus)
            return;

        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;

        // Debug.LogError("Asdf RequestLaunchInterstitial 2222  ===="+item.AdID+ " type=== "+adType);


        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                if (item.appOpenAd != null)
                {
                    item.appOpenAd.Destroy();
                    item.appOpenAd = null;
                    item.isAdRequested = true;
                    //Debug.LogError("Asdf RequestAppOpenAd 3333");

                    //FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Success);
                }
                //BDebug.LogError("Admob Request InterStital called  " + adType);

                AppOpenAd.Load(item.AdID, new AdRequest(),
                        (AppOpenAd ad, LoadAdError loadAdError) =>
                        {
                            if (loadAdError != null)
                            {
                                Debug.LogError("RequestAppOpenAd ad failed to load with error: " + loadAdError.GetMessage()+" "+adItem.AdID);
                                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                {
                                    RequestWithDelay(adDelayTimer, () =>
                                    {
                                        //TODO :  RequestAppOpenAd();
                                    });
                                    FireBaseActions(AdContent.AdmobAppopenAdLoadFailed, AdMode.Requested, SuccessStatus.Failed);

                                    //Debug.LogError("Asdf RequestLaunchInterstitial 44444");
                                     appOpenAdLoadCallback?.Invoke(false);
                                });

                                return;
                            }
                            else if (ad == null)
                            {
                               Debug.LogError("Interstitial ad failed to load." + adItem.AdID);
                                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                {
                                    FireBaseActions(AdContent.AdmobAppopenAdLoadFailed, AdMode.Requested, SuccessStatus.Failed);

                                    RequestWithDelay(adDelayTimer, () =>
                                    {
                                        //TODO :  RequestAppOpenAd();
                                    });
                                    appOpenAdLoadCallback?.Invoke(false);

                                });
                                return;
                            }

                            //Debug.LogError("Asdf RequestLaunchInterstitial 666666");

                            //B Debug.LogError("Interstitial ad loaded." + adType + "  "+item.AdID);
                            if (!item.isAdReady)
                            {
                                item.appOpenAd = ad;
                                item.isAdReady = true;
                                item.appOpenAd.OnAdFullScreenContentClosed += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        //RequestAppOpenAd();
                                    });
                                };
                                appOpenAd = ad;
                                item.appOpenAd.OnAdImpressionRecorded += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                      
                                    });
                                };

                                item.appOpenAd.OnAdClicked += () =>
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                      
                                    });
                                };
                                _expireTime = DateTime.Now + TimeSpan.FromHours(4);
                                appOpenAdLoadCallback?.Invoke(true);

                                item.appOpenAd.OnAdPaid += (AdValue adValue) =>
                                {
                                    float revenue = adValue.Value / 1_000_000f; // Convert micros to dollars
                                    string currency = adValue.CurrencyCode;

                                    // Check if revenue is positive and currency is valid
                                    if (revenue > 0 && !string.IsNullOrEmpty(currency))
                                    {
                                        //TODO 
                                        
                                        // Construct and send the Singular AdMon Event
                                        Singular.SingularAdData data = new Singular.SingularAdData(
                                            "Admob",
                                            currency,
                                            revenue
                                        );
                                        Singular.SingularSDK.AdRevenue(data);

                                        // Log the revenue data for debugging purposes
                                        Debug.Log($"Ad Revenue reported to Singular: {data}");
                                        
                                    }
                                    else
                                    {
                                        Debug.LogError($"Invalid ad revenue data: revenue = {revenue}, currency = {currency}");
                                    }
                                };
                            }

                        });
            }
        });
    }

    public void ShowAppOpenAd(Action<bool> successCallback=null)
    {
        //Debug.LogError("Asdf ShowInterstitialAd 0000");
        AdItem item = null;
        if (keyValuePairs.TryGetValue(AdType.AppOpenAd, out AdItem adItem))
        {
            item = adItem;
        }
        //Debug.LogError("Asdf ShowInterstitialAd 11111");

        if (GameConstants.GetNoAdsStatus)
            return;


        if (item.appOpenAd != null && item.appOpenAd.CanShowAd())
        {
            //Debug.LogError("Asdf ShowInterstitialAd 22222");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.appOpenAd.Show();
                item.isAdReady = false;
                item.isAdRequested = false;
                successCallback?.Invoke(true);
              //  FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Success);

            });
        }
        else
        {
         
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.LogError("ShowAppOpenAd ad cannot be shown.");
                RequestWithDelay(adDelayTimer, () =>
                {
                    //RequestAppOpenAd();
                });
                //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Failed);

            });

        }
    }

    public bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null
                   && appOpenAd.CanShowAd()
                   && DateTime.Now < _expireTime;
        }
    }

    void OnAdImpression(AdType adType)
    {
        try
        {
            if (adType == AdType.Interstital)
            {
                FireBaseActions(AdContent.AdmobInterstitialImpression, AdMode.Impression, SuccessStatus.Success);
            }
            if (adType == AdType.Reward)
            {
                FireBaseActions(AdContent.AdmobRewardImpression, AdMode.Impression, SuccessStatus.Success);
            }
            if (adType == AdType.RewardedInterStitial)
            {
                FireBaseActions(AdContent.AdmobRewardInterstitialImpression, AdMode.Impression, SuccessStatus.Success);
            }
            //OnAdImpressionCallBack?.Invoke(adType.ToString(),  NetworkType.AdMob.ToString());
            if(dailyLoginHandler != null) 
            dailyLoginHandler.OnAdImpressionCallBack(adType.ToString(), NetworkType.AdMob.ToString());
        }
        catch (Exception ex)
        {

        }
    }

    void OnAdClicked(AdType adType)
    {
        try
        {
            OnAdClickEvents();

            OnAdClickSessionEvents();

            void OnAdClickEvents()
            {
                if (adType == AdType.Interstital)
                {
                    FireBaseActions(AdContent.AdmobInterstitialClicked, AdMode.Clicked, SuccessStatus.Success);
                }
                if (adType == AdType.Reward)
                {
                    FireBaseActions(AdContent.AdmobRewardClicked, AdMode.Clicked, SuccessStatus.Success);
                }
                if (adType == AdType.RewardedInterStitial)
                {
                    FireBaseActions(AdContent.AdmobRewardInterstitialClicked, AdMode.Clicked, SuccessStatus.Success);
                }
            }

            void OnAdClickSessionEvents()
            {

                if (FirebaseEvents.instance != null)
                {
                    double mins = (DateTime.UtcNow - timeSinceGameLoaded).TotalMinutes;
                    if (mins > 0 && mins <= 3)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdClicksBefore3Mins.ToString(), AdMode.Clicked.ToString(), adType.ToString());
                    }
                    else if (mins > 3 && mins <= 6)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdClicksBefore6Mins.ToString(), AdMode.Clicked.ToString(), adType.ToString());

                    }
                    else if (mins > 6 && mins <= 9)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdClicksBefore9Mins.ToString(), AdMode.Clicked.ToString(), adType.ToString());

                    }
                    else if (mins > 9)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdClicksafter9Mins.ToString(), AdMode.Clicked.ToString(), adType.ToString());
                    }
                }
            }

            //OnAdClickedCallBack?.Invoke(adType.ToString(), NetworkType.AdMob.ToString());
            if (dailyLoginHandler != null)
                dailyLoginHandler.OnAdClickedCallBack(adType.ToString(), NetworkType.AdMob.ToString());
        }
        catch (Exception ex)
        {

        }
    }

    public void FireBaseActions(AdContent adContent, AdMode adMode, SuccessStatus status)
    {
        try
        {
            if (FirebaseEvents.instance != null)
            {
                FirebaseEvents.instance.LogFirebaseEvent(adContent.ToString(), adMode.ToString(), status.ToString());
            }

            AdShownEvents();

            void AdShownEvents()
            {
                if (adMode == AdMode.Shown)
                {
                    double mins = (DateTime.UtcNow - timeSinceGameLoaded).TotalMinutes;


                    if (FirebaseEvents.instance != null)
                    {
                        if (mins > 0 && mins <= 3)
                        {
                            
                            FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdsBefore3Mins.ToString(), adMode.ToString(), status.ToString());
                        }
                        else if (mins > 3 && mins <= 6)
                        {
                           
                                FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdsBefore6Mins.ToString(), adMode.ToString(), status.ToString());

                        }
                        else if (mins > 6 && mins <= 9)
                        {
                           
                                FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdsBefore9Mins.ToString(), adMode.ToString(), status.ToString());

                        }
                        else if (mins > 9)
                        {
                            
                                FirebaseEvents.instance.LogFirebaseEvent(AdContent.SessionAdsafter9Mins.ToString(), adMode.ToString(), status.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception e) 
        {
            
        }
    }


    void RequestWithDelay(float timer, Action callback, Action<Coroutine> coroutineCallBack = null)
    {
        StopCoroutine(RequestDelay(timer, callback));
        Coroutine coroutine= StartCoroutine(RequestDelay(timer, callback));
        coroutineCallBack?.Invoke(coroutine);
    }
    IEnumerator RequestDelay(float timer, Action callBack)
    {
        yield return new WaitForSeconds(timer);
        callBack?.Invoke();
    }

    IEnumerator RunOnMainThread(Action callback)
    {
        yield return null;
        callback?.Invoke();
    }
}
