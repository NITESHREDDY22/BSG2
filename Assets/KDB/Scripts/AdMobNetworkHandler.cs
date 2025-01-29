using GoogleMobileAds.Api;
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
    RewardContinue
}

public enum AdContent
{
    AdMobLaunchRequested,
    AdMobInterstitalRequested,
    AdMobRewardRequested,
    AdMobContinueRewardRequested,

    AdMobLaunchShown,
    AdMobInterstitalShown,
    AdMobRewardShown,
    AdMobContinueRewardShown,


    LevelPlayLaunchRequested,
    levelPlayInterstitalRequested,
    levelPlayRewardRequested,


    LevelPlayLaunchShown,
    levelPlayInterstitalshown,
    levelPlayRewardShown,
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

public class AdMobNetworkHandler :MonoBehaviour
{

    public InterstitialAd adMobInterstitial, adMobLaunchInterstitial;
    public RewardedAd adMobRewardBasedVideo;
    public RewardedAd adMobContinueRewardBasedVideo;

    private bool isAdMobInitialized;
    private Action<AdType> OnAdClosedAction;

    private string adMobInterstitialId;
    private string adMobLaunchinterStitialId;
    private string adMobRewardBasedVideoId;
    private string adMobContinueRewardBasedVideoId;

    private int adDelayTimer = 60;

    public class AdItem
    {
        public string AdID;
        public InterstitialAd Interstitial;
        public bool isAdReady;
        public bool isAdRequested;
        public RewardedAd RewardedAd;
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
    }

    private Dictionary<AdType, AdItem> keyValuePairs = new Dictionary<AdType, AdItem>();

    private Action<bool> callBack=null;

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

        if (!keyValuePairs.ContainsKey(AdType.RewardContinue))
        {
            keyValuePairs.Add(AdType.RewardContinue, new AdItem(adMobContinueRewardBasedVideoId, adMobContinueRewardBasedVideo, false, false));
        }
    }
    public void Initialize(bool isAdMobInitialized)
    {
        this.isAdMobInitialized = isAdMobInitialized;       
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

    public void SetContinueRewardId(string adMobContinueRewardBasedVideoId)
    {
#if UNITY_EDITOR
        adMobContinueRewardBasedVideoId = string.Empty;
#endif
        this.adMobContinueRewardBasedVideoId = adMobContinueRewardBasedVideoId;
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

                FireBaseActions(adType==AdType.Launch? AdContent.AdMobLaunchRequested: AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Success);               
            }
            Debug.Log("Admob Request InterStital called"+adType);

            InterstitialAd.Load(item.AdID, new AdRequest(),
                    (InterstitialAd ad, LoadAdError loadAdError) =>
                    {
                        if (loadAdError != null)
                        {
                            //B Debug.Log("Interstitial ad failed to load with error: " + loadAdError.GetMessage()+adType);
                            OnAdLoadFailed(adType);
                            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Failed);
                            //Debug.Log("Asdf RequestLaunchInterstitial 44444");
                            return;
                        }
                        else if (ad == null)
                        {
                            //B Debug.Log("Interstitial ad failed to load." + adType);
                            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Requested, SuccessStatus.Failed);
                            //Debug.Log("Asdf RequestLaunchInterstitial 55555");
                            OnAdLoadFailed(adType);
                            return;
                        }

                        //Debug.Log("Asdf RequestLaunchInterstitial 666666");

                        Debug.Log("Interstitial ad loaded." + adType);
                        item.Interstitial = ad;
                        item.isAdReady = true;
                        item.Interstitial.OnAdFullScreenContentClosed += () => {OnAdClosed(adType);};

                        if (adType == AdType.Launch)
                        {
                            adMobLaunchInterstitial = ad;
                        }
                        if(adType==AdType.Interstital)
                        {
                            adMobInterstitial = ad;
                        }
                        FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchRequested : AdContent.AdMobInterstitalRequested, AdMode.Loaded, SuccessStatus.Success);

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
            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Success);

        }
        else
        {
            //Debug.Log("Asdf ShowInterstitialAd 33333");

            Debug.Log("Interstitial ad cannot be shown.");
            callBack?.Invoke(false);
            OnAdLoadFailed(adType);
            FireBaseActions(adType == AdType.Launch ? AdContent.AdMobLaunchShown : AdContent.AdMobInterstitalShown, AdMode.Shown, SuccessStatus.Failed);

        }
    }

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
            //B Debug.Log("Admob RequestWithDelay InterStital called" + adType);
            RequestWithDelay(adDelayTimer, () =>
            {
                // Debug.Log("RequestWithDelay Interstitial ad cannot be shown.");
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
                break;
        }
    }

    public void RequestRewardBasedVideo(AdType adType=AdType.Reward)
    {

         AdItem item = null;

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }

        if (item == null || string.IsNullOrEmpty(item.AdID) || !isAdMobInitialized || item.isAdRequested)
            return;


        if (!item.isAdReady)
        {
            if (item.RewardedAd != null)
            {
                item.RewardedAd.Destroy();
                item.RewardedAd = null;
                item.isAdRequested = true;
                FireBaseActions(adType==AdType.Reward? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Requested, SuccessStatus.Success);

            }

            //B Debug.Log("Admob Request Reward called");

            RewardedAd.Load(item.AdID, new AdRequest(),
               (RewardedAd ad, LoadAdError loadError) =>
               {
                   if (loadError != null)
                   {
                       item.isAdRequested = false;
                       item.isAdReady= false;
                       RequestAgain(adType);
                       //B Debug.Log("asdf Admob Rewarded ad failed to load with error: " +loadError.GetMessage()+adType);
                       FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Requested, SuccessStatus.Failed);

                       return;
                   }
                   else if (ad == null)
                   {
                       item.isAdRequested = false;
                       item.isAdReady = false;
                       RequestAgain(adType);

                       //B Debug.Log("asdf Admob  Rewarded ad failed to load." + adType);
                       FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Requested, SuccessStatus.Failed);

                       return;
                   }

                   //B Debug.Log("asdf Admob  Rewarded ad loaded."+adType);
                   item.RewardedAd= ad;
                   item.isAdReady= true;

                   if(adType==AdType.Reward)
                   {
                       adMobRewardBasedVideo = ad;
                   }
                   if(adType==AdType.RewardContinue)
                   {
                       adMobContinueRewardBasedVideo = ad;
                   }

                   ad.OnAdFullScreenContentClosed += () =>
                   {
                       //B Debug.Log("asdf Admob  Rewarded Ad full screen content closed.");

                       // Reload the ad so that we can show another as soon as possible.
                       item.isAdReady = false;
                       item.isAdRequested = false;
                       RequestRewardBasedVideo(adType);
                       this.callBack?.Invoke(true);
                       FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Success);

                   };
                   // Raised when the ad failed to open full screen content.
                   ad.OnAdFullScreenContentFailed += async (AdError error) =>
                   {
                       Debug.LogError("asdf Admob  Rewarded ad failed to open full screen content " +
                                      "with error : " + error);

                       // Reload the ad so that we can show another as soon as possible.

                       //Debug.Log("OnAdFullScreenContentFailed. should add delay request");
                       item.isAdReady= false;
                       item.isAdRequested= false;
                       this.callBack?.Invoke(false);
                       RequestAgain(adType);
                       FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Failed);

                   };
                   FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardRequested : AdContent.AdMobContinueRewardRequested, AdMode.Loaded, SuccessStatus.Success);

               });
        }
        /*
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

            Debug.Log("Admob Request Reward called");

            RewardedAd.Load(adMobRewardBasedVideoId, new AdRequest(),
               (RewardedAd ad, LoadAdError loadError) =>
               {
                   if (loadError != null)
                   {
                       isAdmobRewardRequested = false;
                       adMobRewardBasedVideoReady = false;
                       RequestAgain(adType);                      
                       Debug.Log("asdf Admob Rewarded ad failed to load with error: " +
                         loadError.GetMessage());
                       FireBaseActions(AdContent.AdMobReward, AdMode.Requested, SuccessStatus.Failed);

                       return;
                   }
                   else if (ad == null)
                   {
                       isAdmobRewardRequested = false;
                       adMobRewardBasedVideoReady = false;
                       RequestAgain(adType);

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
        */
    }

    void RequestAgain(AdType adType)
    {
        //B Debug.Log("Admob RequestWithDelay reward called");       
        RequestWithDelay(adDelayTimer, () =>
        {
            //Debug.Log("RequestWithDelay RequestRewardBasedVideo ad cannot be shown.");

            RequestRewardBasedVideo(adType);
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
            item.RewardedAd.Show((Reward reward) =>
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
            FireBaseActions(adType == AdType.Reward ? AdContent.AdMobRewardShown : AdContent.AdMobContinueRewardShown, AdMode.Shown, SuccessStatus.Failed);
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


    void RequestWithDelay(float timer, Action callback)
    {
        StopCoroutine(RequestDelay(timer, callback));
        StartCoroutine(RequestDelay(timer, callback));
    }
    IEnumerator RequestDelay(float timer, Action callBack)
    {
        yield return new WaitForSeconds(timer);
        callBack?.Invoke();
    }
}
