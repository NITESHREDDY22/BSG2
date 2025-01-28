using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public enum AdType
{
    Launch,
    Interstital
}

public enum AdContent
{
    AdMobLaunch,
    AdMobInterstital,
    AdMobReward,
    LevelPlayLaunch,
    levelPlayInterstital,
    levelPlayReward,

}

public enum AdMode
{
    Requested,
    Loaded,
    Shown
}

public enum SuccessStatus
{
    Success,
    Failed
}

public class AdMobNetworkHandler
{

    public InterstitialAd adMobInterstitial, adMobLaunchInterstitial;
    public RewardedAd adMobRewardBasedVideo;
    public bool adMobInterstitialReady, adMobLaunchInterstitialReady;
    public bool adMobRewardBasedVideoReady,isAdmobRewardRequested;

    private bool isAdMobInitialized;
    private Action<AdType> adMobLoadFailedAction;
    private Action<AdType> OnAdClosedAction;

    private string adMobInterstitialId;
    private string adMobLaunchinterStitialId;
    private string adMobRewardBasedVideoId;

    private int adDelayTimer = 60;

    public class AdItem
    {
        public string AdID;
        public InterstitialAd Interstitial;
        public bool isAdReady;
        public bool isAdRequested;

        public AdItem(string AdID, InterstitialAd Interstitial, bool isAdReady,bool isAdRequested)
        {
            this.AdID = AdID;
            this.Interstitial = Interstitial;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }
    }

    private Dictionary<AdType, AdItem> keyValuePairs = new Dictionary<AdType, AdItem>();

    private Action<bool> callBack=null;

    private AdManager adManager;

    public void InitAdManager(AdManager adManager)
    {
        this.adManager = adManager;
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
    }
    public void Initialize(bool isAdMobInitialized)
    {
        this.isAdMobInitialized = isAdMobInitialized;
        adMobLoadFailedAction = OnAdLoadFailed;
        OnAdClosedAction = OnAdClosed;
    }

    public void SetLaunchInterStitalId(string adMobLaunchinterStitialId)
    {
#if UNITY_EDITOR
        adMobLaunchinterStitialId = string.Empty;
        #endif

        this.adMobLaunchinterStitialId = adMobLaunchinterStitialId;
    }

    public void SetInterStitalId(string adMobInterstitialId)
    {
        #if UNITY_EDITOR
                adMobInterstitialId = string.Empty;
#endif
        this.adMobInterstitialId = adMobInterstitialId;
    }

    public void SetRewardId(string adMobRewardBasedVideoId)
    {
#if UNITY_EDITOR
        adMobRewardBasedVideoId = string.Empty;
#endif
        this.adMobRewardBasedVideoId = adMobRewardBasedVideoId;
    }

    public void RequestInterstitial(AdType adType)
    {       
        AdItem item =null;
        //Debug.Log("Asdf RequestLaunchInterstitial 1111"+ isAdMobInitialized);

        if (keyValuePairs.TryGetValue(adType,out AdItem adItem))
        {
            item = adItem;
        }
       // Debug.Log("Asdf RequestLaunchInterstitial 1111----"+item + " "+ item.AdID+ ""+ item.isAdRequested);

        if (item == null||  string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;

        //Debug.Log("Asdf RequestLaunchInterstitial 2222");

        if (!item.isAdReady)
        {
            if (item.Interstitial != null)
            {
                item.Interstitial.Destroy();
                item.Interstitial = null;
                item.isAdRequested = true;
                //Debug.Log("Asdf RequestLaunchInterstitial 3333");

                FireBaseActions(adType==AdType.Launch? AdContent.AdMobLaunch: AdContent.AdMobInterstital, AdMode.Requested, SuccessStatus.Success);
               
            }
            InterstitialAd.Load(item.AdID, new AdRequest(),
                    (InterstitialAd ad, LoadAdError loadAdError) =>
                    {
                        if (loadAdError != null)
                        {
                            Debug.Log("Interstitial ad failed to load with error: " +
                            loadAdError.GetMessage());
                            adMobLoadFailedAction?.Invoke(adType);
                            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunch : AdContent.AdMobInterstital, AdMode.Requested, SuccessStatus.Failed);
                            //Debug.Log("Asdf RequestLaunchInterstitial 44444");
                            return;
                        }
                        else if (ad == null)
                        {
                            Debug.Log("Interstitial ad failed to load.");
                            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunch : AdContent.AdMobInterstital, AdMode.Requested, SuccessStatus.Failed);
                            //Debug.Log("Asdf RequestLaunchInterstitial 55555");

                            adMobLoadFailedAction?.Invoke(adType);
                            return;
                        }

                        //Debug.Log("Asdf RequestLaunchInterstitial 666666");

                        Debug.Log("Interstitial ad loaded.");
                        item.Interstitial = ad;
                        item.Interstitial.OnAdFullScreenContentClosed += () => {OnAdClosed(adType);};
                        item.isAdReady = true;
                        FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunch : AdContent.AdMobInterstital, AdMode.Loaded, SuccessStatus.Success);

                    });
        }
    }

    public void ShowInterstitialAd(AdType adType, Action<bool> callBack = null)
    {
        //Debug.Log("Asdf ShowInterstitialAd 0000");

        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        //Debug.Log("Asdf ShowInterstitialAd 11111");

        if (item.Interstitial != null && item.Interstitial.CanShowAd())
        {
            //Debug.Log("Asdf ShowInterstitialAd 22222");

            item.Interstitial.Show();
            item.isAdReady = false;
            item.isAdRequested = false;
            callBack?.Invoke(true);
            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunch : AdContent.AdMobInterstital, AdMode.Shown, SuccessStatus.Success);

        }
        else
        {
            //Debug.Log("Asdf ShowInterstitialAd 33333");

            Debug.Log("Interstitial ad cannot be shown.");
            callBack?.Invoke(false);
            adMobLoadFailedAction?.Invoke(adType);
            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunch : AdContent.AdMobInterstital, AdMode.Shown, SuccessStatus.Failed);

        }
    } 

   void OnAdLoadFailed(AdType adType)
    {
        switch (adType)
        {
            case AdType.Launch:
                var item = keyValuePairs[adType];
                if(item!=null)
                {
                    item.isAdRequested = false;
                    item.isAdReady = false;
                }
                break;

            case AdType.Interstital:
                var item1 = keyValuePairs[adType];
                if (item1 != null)
                {
                    item1.isAdRequested = false;
                    item1.isAdReady = false;
                }
                adManager.RequestWithDelay(adDelayTimer, () =>
                {
                   // Debug.Log("RequestWithDelay Interstitial ad cannot be shown.");

                    RequestInterstitial(adType);
                });
                break;
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
                break;
        }
    }

    public void RequestRewardBasedVideo()
    {

        if (isAdmobRewardRequested)
            return;


        if (!adMobRewardBasedVideoReady && isAdMobInitialized)
        {

            if (adMobRewardBasedVideo != null)
            {
                adMobRewardBasedVideo.Destroy();
                adMobRewardBasedVideo = null;
                isAdmobRewardRequested = true;
                FireBaseActions( AdContent.AdMobReward, AdMode.Requested, SuccessStatus.Success);

            }

            RewardedAd.Load(adMobRewardBasedVideoId, new AdRequest(),
               (RewardedAd ad, LoadAdError loadError) =>
               {
                   if (loadError != null)
                   {
                       isAdmobRewardRequested = false;
                       adMobRewardBasedVideoReady = false;
                       RequestAgain();                      
                       Debug.Log("asdf Admob Rewarded ad failed to load with error: " +
                         loadError.GetMessage());
                       FireBaseActions(AdContent.AdMobReward, AdMode.Requested, SuccessStatus.Failed);

                       return;
                   }
                   else if (ad == null)
                   {
                       adMobRewardBasedVideoReady = false;
                       isAdmobRewardRequested = false;

                       RequestAgain();
                       Debug.Log("asdf Admob  Rewarded ad failed to load.");
                       FireBaseActions(AdContent.AdMobReward, AdMode.Requested, SuccessStatus.Failed);

                       return;
                   }

                   Debug.Log("asdf Admob  Rewarded ad loaded.");
                   adMobRewardBasedVideo = ad;
                   adMobRewardBasedVideoReady = true;
                   
                   ad.OnAdFullScreenContentClosed += () =>
                   {
                       Debug.Log("asdf Admob  Rewarded Ad full screen content closed.");

                       // Reload the ad so that we can show another as soon as possible.
                       adMobRewardBasedVideoReady = false;
                       isAdmobRewardRequested = false;
                       RequestRewardBasedVideo();
                       this.callBack?.Invoke(true);
                       FireBaseActions(AdContent.AdMobReward, AdMode.Shown, SuccessStatus.Success);

                   };
                   // Raised when the ad failed to open full screen content.
                   ad.OnAdFullScreenContentFailed += async (AdError error) =>
                   {
                       Debug.LogError("asdf Admob  Rewarded ad failed to open full screen content " +
                                      "with error : " + error);

                       // Reload the ad so that we can show another as soon as possible.

                       //Debug.Log("OnAdFullScreenContentFailed. should add delay request");
                       adMobRewardBasedVideoReady = false;
                       isAdmobRewardRequested = false;
                       this.callBack?.Invoke(false);
                       RequestAgain();
                       FireBaseActions(AdContent.AdMobReward, AdMode.Shown, SuccessStatus.Failed);

                   };
                   FireBaseActions(AdContent.AdMobReward, AdMode.Loaded, SuccessStatus.Success);

               });
        }       
    }

    void RequestAgain()
    {
        adMobRewardBasedVideoReady = false;
        adManager.RequestWithDelay(adDelayTimer, () =>
        {
            //Debug.Log("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");

            RequestRewardBasedVideo();
        });
    }   

    public void ShowAdmobRewardedVideo(Action<bool> callBack)
    {
        this.callBack = callBack;
        if (adMobRewardBasedVideo != null && adMobRewardBasedVideo.CanShowAd())
        {
            adMobRewardBasedVideo.Show((Reward reward) =>
            {
                if (reward != null)
                {
                    Debug.Log("asdf Admob  Rewarded ad granted a reward: ");                 
                }               
            });
        }  
        else
        {
            callBack?.Invoke(false);
            FireBaseActions(AdContent.AdMobReward, AdMode.Shown, SuccessStatus.Failed);

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
        }
        catch (Exception e) 
        {
            
        }
    }


}
