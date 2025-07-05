using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyBidAdViewFactory
{
    public static HyBidAdView createHyBidAdView(MonoBehaviour parent) 
	{
		HyBidAdView banner;

		#if UNITY_EDITOR

		banner = parent.gameObject.AddComponent<HyBidEditorBanner> ();

		#elif UNITY_ANDROID

		banner = parent.gameObject.AddComponent<HyBidAndroidBanner> ();

		#elif UNITY_IOS

		banner = parent.gameObject.AddComponent<HyBidIOSBanner> ();

		#endif

		return banner;
	}
}