using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialNative : MonoBehaviour, IInterstitialListener
{
	private HyBidInterstitialAd interstitial;

	private bool isInterstitialLoading;

	public string appToken;
	public string placement;

	[SerializeField]
	private Button _buttonLoadInterstitial;

	// Use this for initialization
	void Start ()
	{
		interstitial = HyBidInterstitialAdFactory.createInterstitialAd (this);

		interstitial.appToken = appToken;
		interstitial.placement = placement;
		interstitial.InterstitialListener = this;
		_buttonLoadInterstitial.onClick.AddListener (RequestInterstitial);
	}

	private void RequestInterstitial ()
	{
		if (interstitial != null && !isInterstitialLoading) {
			isInterstitialLoading = true;
			interstitial.Load();
		}
	}

	// Interstitial Listeners
	public void OnInterstitialLoaded ()
	{
		isInterstitialLoading = false;
		interstitial.Show ();
	}

	public void OnInterstitialLoadFailed (Exception error)
	{
		isInterstitialLoading = false;
		// Handle error
	}

	public void OnInterstitialImpression ()
	{
		// Handle Impression
	}

	public void OnInterstitialClick ()
	{
		// Handle click
	}

	public void OnInterstitialDismissed ()
	{
		// Handle dismissed
	}
	
}