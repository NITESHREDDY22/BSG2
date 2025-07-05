using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedNative : MonoBehaviour, IRewardedListener
{
	private HyBidRewardedAd rewarded;
	private bool isRewardedLoading;

	public string appToken;
	public string placement;

	[SerializeField]
	private Button _buttonLoadRewarded;

	// Use this for initialization
	void Start ()
	{
		rewarded = HyBidRewardedAdFactory.createRewardedAd (this);

		rewarded.appToken = appToken;
		rewarded.placement = placement;
		rewarded.RewardedListener = this;
		_buttonLoadRewarded.onClick.AddListener (RequestRewarded);
	}

	private void RequestRewarded ()
	{
		if (rewarded != null && !isRewardedLoading) {
			isRewardedLoading = true;
			rewarded.Load();
		}
	}

	// Rewarded Listeners
	public void OnRewardedLoaded ()
	{
		isRewardedLoading = false;
		rewarded.Show ();
	}

	public void OnRewardedLoadFailed (Exception error)
	{
		isRewardedLoading = false;
		// Handle error
	}

	public void OnRewardedOpened ()
	{
		// Handle opened
	}

	public void OnRewardedClick ()
	{
		// Handle click
	}

	public void OnRewardedClosed ()
	{
		// Handle closed
	}

	public void onReward ()
	{
		// Handle onReward
	}
	
}