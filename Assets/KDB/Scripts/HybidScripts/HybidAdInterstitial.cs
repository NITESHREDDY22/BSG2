using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybidAdInterstitial : MonoBehaviour, IInterstitialListener
{
    
   private HyBidInterstitialAd interstitial;   

    private bool isInterstitialLoading;
    public string appToken;
    public string placement;
    private Action<bool> callback;
    public void Initialize()
    {
        Debug.LogError("Initialize 000");
        interstitial = HyBidInterstitialAdFactory.createInterstitialAd(this);
        Debug.LogError("Initialize 111");
        interstitial.appToken = appToken;
        Debug.LogError("Initialize 222");
        interstitial.placement = placement;
        Debug.LogError("Initialize 333");
        interstitial.InterstitialListener = this; 
        Debug.LogError("Initialize 444");
    }

    public void RequestInterstitial()
    {
        if (interstitial != null && !isInterstitialLoading)
        {
            interstitial.Load();
        }
    }

    public void ShowInterstitial(Action<bool> callback)
    {
        this.callback= callback;
        if (isInterstitialLoading)
        {
            interstitial.Show();
        }
        else
        {
            callback?.Invoke(false);
        }

    }

    public void OnLoadFinished()
    {
        isInterstitialLoading = true;
        Debug.LogError("OnLoadFinished" + (this.name));
    }

    public void OnLoadFailed(Exception error)
    {
        // Handle error
        isInterstitialLoading = false;
        Debug.LogError("OnLoadFailed" + (error));

    }

    public void OnImpressionTracked()
    {
        // Handle Impression
    }

    public void OnClickTracked()
    {
        // Handle Click
    }

    public void OnShown()
    {
        // Handle interstitial show
        callback?.Invoke(true);
    }

    public void OnHidden()
    {
        // Handle interstitial hide
    }

    //OLD
    public void OnInterstitialClick()
    {

    }

    public void OnInterstitialDismissed()
    {
       

    }

    public void OnInterstitialImpression()
    {

    }

    public void OnInterstitialLoaded()
    {
    }

    public void OnInterstitialLoadFailed(Exception error)
    {
    }
   
}
