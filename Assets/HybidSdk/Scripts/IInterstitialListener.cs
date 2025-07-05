using System;

public interface IInterstitialListener
{
	void OnInterstitialLoaded ();

	void OnInterstitialLoadFailed (Exception error);

    void OnInterstitialImpression ();

    void OnInterstitialClick ();

    void OnInterstitialDismissed ();
}