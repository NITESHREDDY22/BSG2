using System;

public interface IAdViewListener
{
	void OnAdLoaded ();

	void OnAdLoadFailed (Exception error);

    void OnAdImpression ();

    void OnAdClick ();
}