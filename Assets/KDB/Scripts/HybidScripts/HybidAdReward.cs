using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class HybidAdReward : MonoBehaviour, IRewardedListener
{
    
    private HyBidRewardedAd rewarded;
    private bool isRewardedLoading;
    public string appToken;
    public string placement;
    Action<bool> callback;

    // Use this for initialization
    public void Initialize()
    {
        rewarded = HyBidRewardedAdFactory.createRewardedAd(this);
        rewarded.appToken = appToken;
        rewarded.placement = placement;
        rewarded.RewardedListener = this;      
    }

    public void RequestRewarded()
    {
        if (rewarded != null && !isRewardedLoading)
        {
            rewarded.Load();
        }
    }

    // Rewarded Listeners
    public void OnRewardedLoaded()
    {
        isRewardedLoading = true;
        Debug.LogError("OnRewardedLoaded" + (isRewardedLoading));


    }

    public void ShowRewardAd(Action<bool> callback)
    {
        this.callback = callback;
        if (isRewardedLoading)
        {
            rewarded.Show();
        }
        else
        {
            callback?.Invoke(false);
        }
    }
    public void OnRewardedLoadFailed(Exception error)
    {
        isRewardedLoading = false;
        Debug.LogError("OnRewardedLoadFailed" + (error));

        // Handle error
    }

    public void OnRewardedOpened()
    {
        // Handle opened
    }

    public void OnRewardedClick()
    {
        // Handle click
    }

    public void OnRewardedClosed()
    {
        // Handle closed
        isRewardedLoading = false;
    }

    public void onReward()
    {
        // Handle onReward
        this.callback?.Invoke(true);
    }
    
}
*/