using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class HybidNetworkHandler : MonoBehaviour
{
    public HybidAdInterstitial hybidAdInterstitial;
    public HybidLaunchlaunchInterstitial hybidAdLaunchInterstitial;
    public HybidAdReward hybidAdReward;
    public HybidAdBanner hybidAdBanner;

    private AdConfig adConfig;

    private DateTime timeSinceGameLoaded;
    private DateTime _expireTime;
    private bool isInitilized=false;
    private void Start()
    {
        timeSinceGameLoaded = DateTime.UtcNow;
    }

    public void Initialize()
    {
        
        hybidAdInterstitial.Initialize();
       //hybidAdLaunchInterstitial.Initialize();
       hybidAdReward.Initialize();
        //hybidAdBanner.Initialize();
        isInitilized = true;

    }

    public void RequestInterstitial()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdInterstitial != null)
            {
                hybidAdInterstitial.RequestInterstitial();
            }
        });
    }

    public void ShowInterstitial(Action<bool> callback)
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdInterstitial != null)
            {
               hybidAdInterstitial.ShowInterstitial(callback);
            }
        });
    }

    public void RequestLaunchInterstitial()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdLaunchInterstitial != null)
            {
                //hybidAdLaunchInterstitial.RequestlaunchInterstitial();
            }
        });
    }

    public void ShowLaunchInterstitial(Action<bool> callback)
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdLaunchInterstitial != null)
            {
                //hybidAdLaunchInterstitial.ShowlaunchInterstitial(callback);
            }
        });
    }

    public void RequestRewardAd()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdReward != null)
            {
                hybidAdReward.RequestRewarded();
            }
        });
    }

    public void ShowRewardAd(Action<bool> callback)
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdReward != null)
            {
                hybidAdReward.ShowRewardAd(callback);
            }
        });
    }


    public void RequestBannerAd()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdBanner != null)
            {
                //hybidAdBanner.RequestBanner();
            }
        });
    }

    public void ShowBannerAd()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdBanner != null)
            {
                //hybidAdBanner.ShowBanner();
            }
        });
    }

    public void HideBanner()
    {
        if (!isInitilized)
            return;

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (hybidAdBanner != null)
            {
                //hybidAdBanner.HideBanner();
            }
        });
    }
}
*/