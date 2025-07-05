using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerNative : MonoBehaviour, IAdViewListener
{
	private HyBidAdView banner;

	public string appToken;
	public string placement;

	[SerializeField]
	private Button _buttonLoadTopBanner;

	[SerializeField]
	private Button _buttonLoadBottomBanner;

	[SerializeField]
	private Button _buttonHideBanner;

	// Use this for initialization
	void Start ()
	{
		banner = HyBidAdViewFactory.createHyBidAdView (this);

		banner.appToken = appToken;
		banner.placement = placement;
		banner.AdViewListener = this;
		_buttonLoadTopBanner.onClick.AddListener (delegate { RequestBanner(1); });
		_buttonLoadBottomBanner.onClick.AddListener (delegate { RequestBanner(2); });
		_buttonHideBanner.onClick.AddListener (HideBanner);
	}

	private void RequestBanner (int atPosition)
	{
		if (banner != null) {
			banner.Load (atPosition);
		}
	}

	private void HideBanner ()
	{
		if (banner != null) {
			banner.Hide ();
		}
	}

	public void OnAdLoaded ()
	{
	}

	public void OnAdLoadFailed (Exception error)
	{
		// Handle error
	}

    public void OnAdImpression ()
	{
		// Handle Impression
	}

    public void OnAdClick ()
	{
		// Handle Click
	}

}
