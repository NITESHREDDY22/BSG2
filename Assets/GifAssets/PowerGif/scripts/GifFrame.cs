using SimpleGif.Enums;
using UnityEngine;


	/// <summary>
	/// Texture + delay + disposal method
	/// </summary>
	public class GifFrame
	{
		public Texture2D Texture;
		public float Delay;
		public DisposalMethod DisposalMethod;

		public GifFrame(Texture2D texture, float delay, DisposalMethod disposalMethod = DisposalMethod.RestoreToBackgroundColor)
		{
			Texture = texture;
			Delay = delay;
			DisposalMethod = disposalMethod;
		}
	}