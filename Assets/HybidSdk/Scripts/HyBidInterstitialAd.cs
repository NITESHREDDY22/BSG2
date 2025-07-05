using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HyBidInterstitialAd : MonoBehaviour
{
    public string appToken;
	public string placement;
	protected string adID = "";

	protected IInterstitialListener interstitialListener;

    public IInterstitialListener InterstitialListener {
		get {
			return this.interstitialListener;
		}
		set {
			this.interstitialListener = value;
		}
	}

    public string AdId {
		get {
			return this.adID;
		}
		set {
			this.adID = value;
		}
	}

    public abstract void Load ();
	public abstract void Show ();
	public abstract void Hide ();

    protected virtual void OnHyBidInterstitialLoaded (string message)
	{
		if (this.interstitialListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.interstitialListener.OnInterstitialLoaded ();
		}
	}

	protected virtual void OnHyBidInterstitialLoadFailed (string message)
	{
		if (this.interstitialListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.interstitialListener.OnInterstitialLoadFailed (new Exception ("Ad failed to load."));
		}
	}

	protected virtual void OnHyBidInterstitialImpression (string message)
	{
		if (this.interstitialListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.interstitialListener.OnInterstitialImpression ();
		}
	}

	protected virtual void OnHyBidInterstitialClick (string message)
	{
		if (this.interstitialListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.interstitialListener.OnInterstitialClick ();
		}
	}

	protected virtual void OnHyBidInterstitialDismissed (string message)
	{
		if (this.interstitialListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.interstitialListener.OnInterstitialDismissed ();
		}
	}
}