using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyBidInterstitialAdFactory {

	public static HyBidInterstitialAd createInterstitialAd(MonoBehaviour parent) 
	{
		HyBidInterstitialAd interstitial;

		#if UNITY_EDITOR

		interstitial = parent.gameObject.AddComponent<HyBidEditorInterstitialAd> ();

		#elif UNITY_ANDROID

		interstitial = parent.gameObject.AddComponent<HyBidAndroidInterstitialAd> ();

		#elif UNITY_IOS

		interstitial = parent.gameObject.AddComponent<HyBidIOSInterstitialAd> ();

		#endif

		return interstitial;
		}
}