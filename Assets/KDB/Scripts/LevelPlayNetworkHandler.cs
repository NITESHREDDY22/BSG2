using com.unity3d.mediation;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using System.Collections;
using GoogleMobileAds.Common;

public class LevelPlayNetworkHandler : MonoBehaviour
{
    public LevelPlayInterstitialAd levelPlayInterstitial, levelPlayLaunchInterstitial;
    public LevelPlayRewardedAd levelPlayrewardBasedVideo;

    private string interstitialAdUnitId = "z8axy0332hnr585z";
    private string rewardAdUnitId = "hgncqhneupu7bppt";
    private string LaunchinterstitialAdUnitId = "68ozdvrgw2w8rw44";
    private Action<bool> callback = null;

    private int adDelayTimer = 60;

    private AdConfig adConfig;
    public class AdItem
    {
        public string AdID;
        public LevelPlayInterstitialAd Interstitial;
        public bool isAdReady;
        public bool isAdRequested;

        public LevelPlayRewardedAd levelPlayrewardBasedVideo;

        public AdItem(string AdID, LevelPlayInterstitialAd Interstitial, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.Interstitial = Interstitial;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }

        public AdItem(string AdID, LevelPlayRewardedAd levelPlayrewardBasedVideo, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.levelPlayrewardBasedVideo = levelPlayrewardBasedVideo;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }
    }

    private Dictionary<AdType, AdItem> keyValuePairs = new Dictionary<AdType, AdItem>();

    public void Initialize()
    {
        if(!keyValuePairs.ContainsKey(AdType.Launch))
            keyValuePairs.Add(AdType.Launch, new AdItem(LaunchinterstitialAdUnitId, levelPlayLaunchInterstitial, false,false));

        if (!keyValuePairs.ContainsKey(AdType.Interstital))
            keyValuePairs.Add(AdType.Interstital, new AdItem(interstitialAdUnitId, levelPlayInterstitial, false, false));

        if (!keyValuePairs.ContainsKey(AdType.Reward))
            keyValuePairs.Add(AdType.Reward, new AdItem(rewardAdUnitId, levelPlayrewardBasedVideo, false, false));
    }

    public void SetAdConfig(AdConfig adConfig)
    {
        this.adConfig=adConfig;
        SetInterStitalId();
        SetLaunchInterStitalId();
        SetRewardId();
    }
    public void SetLaunchInterStitalId()
    {
        this.LaunchinterstitialAdUnitId = GetAdUnitId(AdType.Launch);
    }

    public void SetInterStitalId()
    {       
        this.interstitialAdUnitId = GetAdUnitId(AdType.Interstital);
    }

    public void SetRewardId()
    {
        this.rewardAdUnitId = GetAdUnitId(AdType.Reward);
    }

    string GetAdUnitId(AdType adType) 
    {
        AdUnitConfig adUnitConfig= adConfig.adConfigs.Find(x=>x.AdType == adType);  
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
        //Debug.Log("Asdf Level RequestLaunchInterstitial 11111");

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        if (item == null || string.IsNullOrEmpty(item.AdID) || item.isAdRequested)
            return;
        //Debug.Log("Asdf Level RequestLaunchInterstitial 2222");

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (!item.isAdReady)
            {
                //Debug.Log("Asdf Level RequestLaunchInterstitial 3333");
                //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunchRequested : AdContent.levelPlayInterstitalRequested, AdMode.Requested, SuccessStatus.Success);

                if (item.Interstitial != null)
                {
                    item.Interstitial.DestroyAd();
                    item.Interstitial = null;
                    item.Interstitial = new LevelPlayInterstitialAd(item.AdID);
                    if (adType == AdType.Launch)
                    {
                        levelPlayLaunchInterstitial = item.Interstitial;
                    }
                    if (adType == AdType.Interstital)
                    {
                        levelPlayInterstitial = item.Interstitial;
                    }
                    //B Debug.Log("LevelPlay Request interstital called");

                    item.isAdRequested = true;
                    item.Interstitial.LoadAd();
                    item.Interstitial.OnAdLoadFailed += (x) =>
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {

                            item.isAdRequested = false;
                            InterstitialOnAdLoadFailedEvent(x, adType);
                            //Debug.Log("Asdf Level RequestLaunchInterstitial 4444");
                            //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunchRequested : AdContent.levelPlayInterstitalRequested, AdMode.Loaded, SuccessStatus.Failed);

                        });
                    };
                    item.Interstitial.OnAdClosed += (x) =>
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {

                            item.isAdRequested = false;
                            item.isAdReady = false;
                            InterstitialOnAdClosedEvent(x);
                            if (adType != AdType.Launch)
                            {
                                RequestInterstitial(adType);
                            }
                            FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunchShown : AdContent.levelPlayInterstitalshown, AdMode.Shown, SuccessStatus.Success);
                        });

                    };
                    item.Interstitial.OnAdLoaded += (x) =>
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            item.isAdReady = true;
                            InterstitialOnAdLoadedEvent(x);
                            //Debug.Log("Asdf Level RequestLaunchInterstitial 55555");

                            FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunchRequested : AdContent.levelPlayInterstitalRequested, AdMode.Loaded, SuccessStatus.Success);

                        });

                    };
                }
            }
        });
    }

    public void ShowInterstitialAd(AdType adType, Action<bool> callBack = null)
    {


        //Debug.Log("Asdf Level ShowInterstitialAd 1111");

        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;

        }
        //Debug.Log("Asdf Level ShowInterstitialAd 22222");

        if (item != null && item.Interstitial!= null && item.Interstitial.IsAdReady())
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                item.Interstitial.ShowAd(); 
            });
        //Debug.Log("Asdf Level ShowInterstitialAd 3333");

        }
        else
        {
            //Debug.Log("Asdf Level ShowInterstitialAd 44444");

            //B Debug.Log("level ad can't shown ");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                //TODO : FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunchShown : AdContent.levelPlayInterstitalshown, AdMode.Shown, SuccessStatus.Failed);
            });

        }
    }

    public void RequestRewardBasedVideo(AdType adType)
    {
        AdItem item = null;
        //Debug.Log("Asdf Level RequestLaunchInterstitial 11111");

        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;
        }
        if (item == null || string.IsNullOrEmpty(item.AdID) || item.isAdRequested)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {

            if (!item.isAdReady && item.levelPlayrewardBasedVideo != null)
            {
                item.levelPlayrewardBasedVideo.DestroyAd();
                item.levelPlayrewardBasedVideo = null;
                item.levelPlayrewardBasedVideo = new LevelPlayRewardedAd(rewardAdUnitId);
                levelPlayrewardBasedVideo = item.levelPlayrewardBasedVideo;

                item.levelPlayrewardBasedVideo.LoadAd();
                item.levelPlayrewardBasedVideo.OnAdClosed += (x) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        OnAdClosedMethod(x, item, adType);
                    });

                };
                item.levelPlayrewardBasedVideo.OnAdLoaded += (x) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        OnAdLoadedMethod(x, item);

                    });
                };
                item.levelPlayrewardBasedVideo.OnAdLoadFailed += (x) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        OnAdLoadFailedMethod(x, item, adType);

                    });

                };
                item.levelPlayrewardBasedVideo.OnAdRewarded += (x, y) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        OnAdRewardedMethod(x, y, item);
                    });
                };
            }
        });
    }

    private void  OnAdRewardedMethod(LevelPlayAdInfo info, LevelPlayReward reward,AdItem item)
    {
        item.isAdRequested = false;
        item.isAdReady = false;
        callback?.Invoke(true);
    }

    private void OnAdLoadFailedMethod(LevelPlayAdError error,AdItem adItem,AdType adType)
    {
        //should add delay      
        //TODO : FireBaseActions(AdContent.levelPlayRewardRequested , AdMode.Loaded, SuccessStatus.Failed);
        adItem.isAdReady = false;
        adItem.isAdRequested = false;
        //B Debug.Log("LevelPlay RequestWithDelay Reward called");
        RequestWithDelay(adDelayTimer, () =>
        {
            RequestRewardBasedVideo(adType);
        });
    }  

    private void OnAdLoadedMethod(LevelPlayAdInfo info,AdItem item)
    {
        //Debug.Log("Asdf RequestRewardBasedVideo..levelPlay2222");
        item.isAdReady= true;
        //TODO : FireBaseActions(AdContent.levelPlayRewardRequested, AdMode.Loaded, SuccessStatus.Success);

        //throw new NotImplementedException();
    }

    private void OnAdClosedMethod(LevelPlayAdInfo info,AdItem item,AdType adType)
    {
        callback?.Invoke(true);
        //B Debug.Log("LevelPlay Request Reward called");
        item.isAdReady= false;
        item.isAdRequested= false;
        RequestRewardBasedVideo(adType);
        //FireBaseActions(AdContent.levelPlayRewardShown, AdMode.Shown, SuccessStatus.Success);
        //throw new NotImplementedException();
    }

    public void ShowRewardBasedVideo(Action<bool> callback=null,AdType adType=AdType.Reward)
    {
#if UNITY_EDITOR
        callback?.Invoke(true);
#endif

        AdItem item = null;
        if (keyValuePairs.TryGetValue(adType, out AdItem adItem))
        {
            item = adItem;

        }
        if (item!=null && item.levelPlayrewardBasedVideo != null && item.levelPlayrewardBasedVideo.IsAdReady())
        {           
            try
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {                    
                    this.callback = callback;
                    item.levelPlayrewardBasedVideo.ShowAd();
                });
            }
            catch (Exception e)
            { }
        }
    }

    public void EnableAds()
    {
        levelPlayInterstitial = new LevelPlayInterstitialAd(interstitialAdUnitId);
        levelPlayLaunchInterstitial = new LevelPlayInterstitialAd(LaunchinterstitialAdUnitId);
        levelPlayrewardBasedVideo = new LevelPlayRewardedAd(rewardAdUnitId);

        Initialize();
       
        // Register to Interstitial events
        //levelPlayInterstitial.OnAdLoaded += InterstitialOnAdLoadedEvent;
        //levelPlayInterstitial.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        //levelPlayInterstitial.OnAdClosed += InterstitialOnAdClosedEvent;
        //interstitial.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        //interstitial.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        //interstitial.OnAdClicked += InterstitialOnAdClickedEvent;
        //interstitial.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
        //rewardBasedVideo.OnAdDisplayed += OnAdDisplayedMethod;
        //rewardBasedVideo.OnAdDisplayFailed += OnAdDisplayFailedMethod;
        //rewardBasedVideo.OnAdClicked += OnAdClickedMethod;
    }
  

    private void InterstitialOnAdClosedEvent(LevelPlayAdInfo info)
    {
        //throw new NotImplementedException();
    }

    private void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error,AdType adType)
    {
        //throw new NotImplementedException();
        //B Debug.Log("LevelPlay RequestWithDelay interstital called");
        if (adType != AdType.Launch)
        {
            RequestWithDelay(adDelayTimer, () =>
            {

                RequestInterstitial(AdType.Interstital);
            });
        }
    }

    private void InterstitialOnAdLoadedEvent(LevelPlayAdInfo info)
    {
        //throw new NotImplementedException();
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
