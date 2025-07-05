using System;

public interface IRewardedListener
{
	void OnRewardedLoaded ();

    void OnRewardedLoadFailed (Exception error);

    void OnRewardedClick ();
    
    void OnRewardedOpened ();

    void OnRewardedClosed ();

    void onReward ();
}