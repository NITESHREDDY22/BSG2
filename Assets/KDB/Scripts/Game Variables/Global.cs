using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static int count = 0;
    public static int birdCount = 0;
    public static int TotalbirdCount = 0;
    public static int target = 0;
    public static ArrayList botList = new ArrayList();
    public static ArrayList brokenBotList = new ArrayList();
    public static bool rewardUsed = false;
    public static bool tutorialDisplaye = false;
    public static int retryCount =3;

    public static bool isBottleCollission = false;

    public static int noOfTries = 0;
    public static bool backFillAds = true;
    public static float BirdColliderRadiusNormal = .25f;//0.86f;
    public static float BirdColliderRadiusBig = .25f;//4.50f;
    public static float BirdColliderRadiusZero = 0.0f;

    public static int CurrentLeveltoPlay = 0;
    public static float backFillAdGapToContinue = 20f;
    public static int TotalStarsAchivedWorld1 = 0;
    public static int TotalStarsAchivedWorld2 = 0;
    public static int TotalStarsAchivedWorld3 = 0;
    public static int TotalStarsAchivedWorld4 = 0;
    public static int TotalStarsAchivedWorld5 = 0;

    public static int World2ReqStars = 50;
    public static int World3ReqStars = 30;
    public static int World4ReqStars = 20;
    public static int World5ReqStars = 20;

    public static bool firstLoad = true;

    public static bool loadedFromServer = false;

    public static bool isBannerEnabled = false;
    public static bool isIntersitialsEnabled = false;
    public static bool isRewaredAdsEnabled = false;
    public static bool isNativeAdsEnabled = false;
}
