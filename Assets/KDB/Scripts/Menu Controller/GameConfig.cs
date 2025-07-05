

[System.Serializable]
public class GameConfig {

    public int adGap1;
    public int adGap2;
    public int adGap3;
    public int adGap4;
	public float const1;

    public int retryCount1;
    public int retryCount2;
    public int retryCount3;
    public int retryCount4;
    public bool backFillAds,useAnalytics,useIAP;
    public int bannerLevel;
    public int backFillAdGap;
    public int backFillAdGapToContinue;
    public bool isNativeAdsEnabled;
    public int GOFAdInterval;
    public int GOWAdInterval;
    public int World2ReqStars=150;
    public int World3ReqStars=60;
    public int World4ReqStars=120;
    public int World5ReqStars=30;
    public int InternetMandtoryLevel=30;
    public int PremiumPopUpInterval=3;
    public int FIRST_LVLS_SET_AD_GAP=2;
    public int SECOND_LVLS_SET_AD_GAP = 3;
    public string Fullversion = "W1_50";
    public bool showLaunchAd = false;
    public bool isAppOpenAdEnabled = false;
    public bool isSingularEnabled = false;

    public int InterstitialAdGap = 60;
    public int showBannerFrom = 4;

    public bool isBannerEnabled;
    public bool isRewaredAdsEnabled;
    public bool isIntersitialsEnabled;
    public int adRetryTime = 60;
}
