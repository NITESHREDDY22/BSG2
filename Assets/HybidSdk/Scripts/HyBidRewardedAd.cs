using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HyBidRewardedAd : MonoBehaviour
{
    public string appToken;
	public string placement;
	protected string adID = "";

	protected IRewardedListener rewardedListener;

    public IRewardedListener RewardedListener {
		get {
			return this.rewardedListener;
		}
		set {
			this.rewardedListener = value;
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

    protected virtual void OnHyBidRewardedLoaded (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.OnRewardedLoaded ();
		}
	}

	protected virtual void OnHyBidRewardedLoadFailed (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.OnRewardedLoadFailed (new Exception ("Ad failed to load."));
		}
	}

	protected virtual void OnHyBidRewardedOpened (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.OnRewardedOpened ();
		}
	}

	protected virtual void OnHyBidRewardedClick (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.OnRewardedClick ();
		}
	}

	protected virtual void OnHyBidRewardedClosed (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.OnRewardedClosed ();
		}
	}

    protected virtual void OnHyBidRewardedReward (string message)
	{
		if (this.rewardedListener != null && adID.Equals(message, StringComparison.Ordinal)) {
			this.rewardedListener.onReward ();
		}
	}
}