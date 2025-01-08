using System.IO;
using System;
using UnityEngine;

/// <summary>
	/// Decoding GIF example.
	/// </summary>
	public class LocalGIFLoad : MonoBehaviour
	{
		public AnimatedImage AnimatedImage;
        public String LargeSample = "Assets/GifAssets/PowerGif/Examples/Samples/Large.gif";
		public String CTA_android_Url;
		public String CTA_ios_Url;

		public void Awake()
		{
			var bytes = File.ReadAllBytes(LargeSample);
			var gif = Gif.Decode(bytes);
			AnimatedImage.Play(gif);
		}

		public void CTAFunc()
		{
            #if UNITY_ANDROID || UNITY_EDITOR
			            Application.OpenURL(CTA_android_Url);
            #elif UNITY_IOS
                        Application.OpenURL(CTA_ios_Url);
            #endif
		}
	}