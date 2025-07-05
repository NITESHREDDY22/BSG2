using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyBidRewardedAdFactory {

	public static HyBidRewardedAd createRewardedAd(MonoBehaviour parent) 
	{
		HyBidRewardedAd rewarded;

		#if UNITY_EDITOR

		rewarded = parent.gameObject.AddComponent<HyBidEditorRewardedAd> ();

		#elif UNITY_ANDROID

		rewarded = parent.gameObject.AddComponent<HyBidAndroidRewardedAd> ();

		#elif UNITY_IOS

		rewarded = parent.gameObject.AddComponent<HyBidIOSRewardedAd> ();

		#endif

		return rewarded;
		}
}