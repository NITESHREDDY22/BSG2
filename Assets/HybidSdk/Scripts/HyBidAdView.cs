using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HyBidAdView : MonoBehaviour
{
    public string appToken;
	public string placement;
	protected string adID = "";

	protected IAdViewListener adViewListener;

    public IAdViewListener AdViewListener {
		get {
			return this.adViewListener;
		}
		set {
			this.adViewListener = value;
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

	public abstract void Load (int position);
	public abstract void Hide ();

    public virtual void OnHyBidAdLoaded(string message){
        if (this.adViewListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.adViewListener.OnAdLoaded();
		}
    }

	public virtual void OnHyBidAdImpression(string message){
        if (this.adViewListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			 this.adViewListener.OnAdImpression();
		}
    }

	public virtual void OnHyBidAdClicked(string message){
        if (this.adViewListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			 this.adViewListener.OnAdClick();
		}
    }

	public virtual void OnHyBidAdError(string message){
        if (this.adViewListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.adViewListener.OnAdLoadFailed(new Exception("Failed to load Ad"));
		}
    }
}