using com.unity3d.mediation;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class LevelPlayNetworkHandler 
{
    public LevelPlayInterstitialAd levelPlayInterstitial, levelPlayLaunchInterstitial;
    public LevelPlayRewardedAd levelPlayrewardBasedVideo;

    private string interstitialAdUnitId = "z8axy0332hnr585z";
    private string rewardAdUnitId = "hgncqhneupu7bppt";
    private string LaunchinterstitialAdUnitId = "68ozdvrgw2w8rw44";

    private bool islevelPlayrewardBasedVideo = false;
    private Action<bool> callback = null;

    private int adDelayTimer = 60;

    public class AdItem
    {
        public string AdID;
        public LevelPlayInterstitialAd Interstitial;
        public bool isAdReady;
        public bool isAdRequested;


        public AdItem(string AdID, LevelPlayInterstitialAd Interstitial, bool isAdReady, bool isAdRequested)
        {
            this.AdID = AdID;
            this.Interstitial = Interstitial;
            this.isAdReady = isAdReady;
            this.isAdRequested = isAdRequested;
        }
    }

    private Dictionary<AdType, AdItem> keyValuePairs = new Dictionary<AdType, AdItem>();

    private AdManager adManager;

    public void InitAdManager(AdManager adManager)
    {
        this.adManager= adManager;
    }

    public void Initialize()
    {
        if(!keyValuePairs.ContainsKey(AdType.Launch))
        keyValuePairs.Add(AdType.Launch, new AdItem(LaunchinterstitialAdUnitId, levelPlayLaunchInterstitial, false,false));
        if (!keyValuePairs.ContainsKey(AdType.Interstital))
            keyValuePairs.Add(AdType.Interstital, new AdItem(interstitialAdUnitId, levelPlayInterstitial, false, false));
    }

    public void SetLaunchInterStitalId(string LaunchinterstitialAdUnitId)
    {
#if UNITY_EDITOR
        LaunchinterstitialAdUnitId = string.Empty;
#endif
        this.LaunchinterstitialAdUnitId = LaunchinterstitialAdUnitId;
    }

    public void SetInterStitalId(string interstitialAdUnitId)
    {
#if UNITY_EDITOR
        interstitialAdUnitId = string.Empty;
#endif
        this.interstitialAdUnitId = interstitialAdUnitId;
    }

    public void SetRewardId(string rewardAdUnitId)
    {
#if UNITY_EDITOR
        rewardAdUnitId = string.Empty;
#endif
        this.rewardAdUnitId = rewardAdUnitId;
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


        FireBaseActions(adType==AdType.Launch? AdContent.LevelPlayLaunch :AdContent.levelPlayInterstital, AdMode.Requested, SuccessStatus.Success);

        if (!item.isAdReady)
        {
            //Debug.Log("Asdf Level RequestLaunchInterstitial 3333");

            if (item.Interstitial != null)
            {
                item.isAdRequested = true;
                item.Interstitial.LoadAd();
                item.Interstitial.OnAdLoadFailed += (x) =>
                {
                    item.isAdRequested = false;
                    InterstitialOnAdLoadFailedEvent(x);
                    //Debug.Log("Asdf Level RequestLaunchInterstitial 4444");

                    FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunch : AdContent.levelPlayInterstital, AdMode.Loaded, SuccessStatus.Failed);

                };
                item.Interstitial.OnAdClosed += (x) => 
                {
                    item.isAdRequested = false;
                    item.isAdReady = false;
                    InterstitialOnAdClosedEvent(x); 
                    if (adType != AdType.Launch)
                    {
                        RequestInterstitial(adType);
                    }
                    FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunch : AdContent.levelPlayInterstital, AdMode.Shown, SuccessStatus.Success);

                };
                item.Interstitial.OnAdLoaded += (x)=> 
                { 
                    item.isAdReady = true;
                    InterstitialOnAdLoadedEvent(x);
                    //Debug.Log("Asdf Level RequestLaunchInterstitial 55555");

                    FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunch : AdContent.levelPlayInterstital, AdMode.Loaded, SuccessStatus.Success);

                };
            }
        }
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
            item.Interstitial.ShowAd(); 
        //Debug.Log("Asdf Level ShowInterstitialAd 3333");

        }
        else
        {
            //Debug.Log("Asdf Level ShowInterstitialAd 44444");

            Debug.Log("level ad can't shown ");
            FireBaseActions(adType == AdType.Launch ? AdContent.LevelPlayLaunch : AdContent.levelPlayInterstital, AdMode.Shown, SuccessStatus.Failed);

        }
    }
    
    public void RequestRewardBasedVideo()
    {

        if (!islevelPlayrewardBasedVideo && levelPlayrewardBasedVideo!=null)
        {
           

            levelPlayrewardBasedVideo.LoadAd();
            levelPlayrewardBasedVideo.OnAdClosed += OnAdClosedMethod;
            levelPlayrewardBasedVideo.OnAdLoaded += OnAdLoadedMethod;
            levelPlayrewardBasedVideo.OnAdLoadFailed += OnAdLoadFailedMethod;
            levelPlayrewardBasedVideo.OnAdRewarded += OnAdRewardedMethod;

        }
    }

    private void  OnAdRewardedMethod(LevelPlayAdInfo info, LevelPlayReward reward)
    {        
        callback?.Invoke(true);
        islevelPlayrewardBasedVideo = false;       
    }

    private void OnAdLoadFailedMethod(LevelPlayAdError error)
    {
        //should add delay
        islevelPlayrewardBasedVideo = false;
        FireBaseActions(AdContent.levelPlayReward, AdMode.Loaded, SuccessStatus.Failed);
        adManager.RequestWithDelay(adDelayTimer, () =>
        {
            //Debug.Log("RequestWithDelay level RequestRewardBasedVideo");

            RequestRewardBasedVideo();
        });
        //throw new NotImplementedException();
    }

    private void OnAdLoadedMethod(LevelPlayAdInfo info)
    {
        //Debug.Log("Asdf RequestRewardBasedVideo..levelPlay2222");
        islevelPlayrewardBasedVideo = true;
        FireBaseActions(AdContent.levelPlayReward, AdMode.Loaded, SuccessStatus.Success);

        //throw new NotImplementedException();
    }

    private void OnAdClosedMethod(LevelPlayAdInfo info)
    {
        callback?.Invoke(true);
        islevelPlayrewardBasedVideo = false;
        RequestRewardBasedVideo();
        FireBaseActions(AdContent.levelPlayReward, AdMode.Shown, SuccessStatus.Success);

        //throw new NotImplementedException();
    }

    public void ShowRewardBasedVideo(Action<bool> callback=null)
    {
        if (levelPlayrewardBasedVideo!=null && levelPlayrewardBasedVideo.IsAdReady())
        {           
            try
            {
                this.callback = callback;
                levelPlayrewardBasedVideo.ShowAd();
            }
            catch (Exception e)
            { }
        }
    }

    public void EnableAds()
    {
        levelPlayInterstitial = new LevelPlayInterstitialAd(interstitialAdUnitId);
        levelPlayLaunchInterstitial = new LevelPlayInterstitialAd(LaunchinterstitialAdUnitId);
        // Register to Interstitial events
        //levelPlayInterstitial.OnAdLoaded += InterstitialOnAdLoadedEvent;
        //levelPlayInterstitial.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        //levelPlayInterstitial.OnAdClosed += InterstitialOnAdClosedEvent;
        //interstitial.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        //interstitial.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        //interstitial.OnAdClicked += InterstitialOnAdClickedEvent;
        //interstitial.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        levelPlayrewardBasedVideo = new LevelPlayRewardedAd(rewardAdUnitId);

       
        //rewardBasedVideo.OnAdDisplayed += OnAdDisplayedMethod;
        //rewardBasedVideo.OnAdDisplayFailed += OnAdDisplayFailedMethod;
        //rewardBasedVideo.OnAdClicked += OnAdClickedMethod;
    }
  

    private void InterstitialOnAdClosedEvent(LevelPlayAdInfo info)
    {
        //throw new NotImplementedException();
    }

    private void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        //throw new NotImplementedException();
    }

    private void InterstitialOnAdLoadedEvent(LevelPlayAdInfo info)
    {
        //throw new NotImplementedException();
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
