using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using static UnityEngine.Networking.UnityWebRequest;

public class GameManager : MonoBehaviour
{
    [Header("dynamicLoadings-dont touch||or assign")]
    public CameraFollow cameraFollow;
    public SlingShot slingShot;
    public GameObject rewardBall;

    [Header("Level_BallsCount")]
    public GameObject ballPrefab;
    public Transform birdsgroup;

    public int[] Level_ballCount;
    public bool[] Give3Stars;
    Boolean isReward;


    [Header("int declarations")]
    public int currentBirdIndex;

    [Header("GameRelative Declarations(UI,ingameelements)")]
    public GameObject gameOverPanel;
    public GameObject SkipLevelBtn, _coinRef, gameFailed, rewardCanvas, levelup, menu, retry, failRetry, failMenu, star0, pauseBtn, replayBtn, tutorAnim, tutorOkBtn, gamePausePanel, ingamePausePanel;
    public Transform LevelsParent, spawnPoint, coinSpace, reachPoint;

    [Header("Text declarations")]
    public Text levelNo;
    public Text levelNoFail;

    [HideInInspector]
    public Text rewardtext;
    //[HideInInspector]
    public GameState gameState;

    private List<GameObject> bricks;
    public List<GameObject> birds;
    private List<GameObject> pigs;
    private float timer = 0;
    private float timermax = 0;

    public Text totalcoinsatWin;
    public Sprite[] BottleSkins;

    public static int coinsEarned = 0;
    public static GameManager Instance;

    int intialbirdcount = 0;

    public float startTime = 0;
    public float levelPlayTime = 0;

    private DateTime SessionStart, SessionEnd;

    private readonly string LevelSkippedKey = "Level_param_Skipped";
    private readonly string LevelExtraBallKey = "Level_param_ExtraBall";

    void Start()
    {

        Instance = this;
        Debug.Log("Start World " + WorldSelectionHandler.worldNumb + "Level" + Global.CurrentLeveltoPlay);
        startTime = Time.time;
        Debug.Log("W" + WorldSelectionHandler.worldNumb + "L" + Global.CurrentLeveltoPlay + " Start Time =" + startTime);
        //GetComponent<SpriteRenderer>().sprite = BottleSkins[PlayerPrefs.GetInt("BottleSelected", 0)];
        SessionStart = DateTime.UtcNow;
        levelNo.text = "" + (Global.CurrentLeveltoPlay + 1);
        levelNoFail.text = "" + (Global.CurrentLeveltoPlay + 1);
        ballCounts = PlayerPrefsX.GetIntArray("ballPowerCounts", 2, 6);
        SetCountTexts();
        ShowBallsCount();
        //TODO: 
        if (!AdManager.onlyOnce)
        {
            //if (!(AdManager._instance.levelPlayInterstitial != null && AdManager._instance.levelPlayInterstitial.IsAdReady()))
            //{
            //}

           // if (AdManager._instance.levelPlayrewardBasedVideo == null || !AdManager._instance.levelPlayrewardBasedVideo.IsAdReady())
            //}
            //{   

                //if (Global.isIntersitialsEnabled)
                //{
                //    AdManager._instance.RequestInterstitial();
                //}
            if (Global.isRewaredAdsEnabled)
            {
                AdManager._instance.RequestRewardBasedVideo(AdType.Reward);
                AdManager._instance.RequestRewardBasedVideo(AdType.RewardContinue);
                AdManager._instance.RequestRewardedInterstitial(AdType.RewardedInterStitial);
            }
            AdManager.onlyOnce = true;
        }

        // AdManager._instance.showbanner();

        SkipLevelBtn.SetActive(false);
        //if (Global.noOfTries>= Global.retryCount &&((AdManager._instance.rewardBasedVideo.IsLoaded() || (AdManager._instance.unityRewardReady && AdManager._instance.enableUnityAds))))

        if (Global.noOfTries >= Global.retryCount && 
            ((AdManager._instance.adMobNetworkHandler!=null && 
            AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo!=null && 
            AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo.CanShowAd())
            ||
            (AdManager._instance.levelPlayNetworkHandler!=null &&
            AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo!=null &&
            AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo.IsAdReady())))
        {
            SkipLevelBtn.SetActive(true);
        }
        else
        {
            SkipLevelBtn.SetActive(false);
        }
        // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelStart, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, "W"+WorldSelectionHandler.worldSelected+"_L" + Global.CurrentLeveltoPlay));
        // Debug.Log((WorldSelectionHandler.worldSelected + 1) + " " + (Global.CurrentLeveltoPlay + 1) + "-Started");
        Analytics.CustomEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Started ", new Dictionary<string, object>
                                            {
                                                { "LEVEL", Global.retryCount },

                                         });
        rewardtext = rewardCanvas.transform.GetChild(0).gameObject.GetComponent<Text>();
    }


    public void pause()
    {
        //AudioManager.Instance.ClickSound.Play ();
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(4, 0);
        }
        //Time.timeScale = 0;
        gameState = GameState.Pause;
        ingamePausePanel.SetActive(false);
        gamePausePanel.SetActive(true);

    }
    public void resume()
    {
        //AudioManager.Instance.ClickSound.Play ();
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(4, 0);
        }
        //Time.timeScale = 1;
        if (FindObjectOfType<timer>() != null)
        {
            FindObjectOfType<timer>().StartGame();
        }
        gameState = GameState.Playing;
        gamePausePanel.SetActive(false);
        ingamePausePanel.SetActive(true);
    }

    public void doneSelected()
    {
        //gamePausePanel.SetActive(true);
        tutorAnim.SetActive(false);
        AnimateBirdToSlingshot();
        // Debug.LogError("TUTORIAL " + slingShot.slingShootState);
    }

    public void Load()
    {
        AdManager._instance.ShowLoadingPanel();
        Global.tutorialDisplaye = true;
        Global.noOfTries = Global.noOfTries + 1;
        //ClickSound.Play ();
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(4, 0);
        }
        if (!gameOverPanel.activeInHierarchy && !gameFailed.activeInHierarchy)
        {
            currentAdDisplayTime = Time.time;
            if ((currentAdDisplayTime - AdManager._instance.lastAdDisplayTime) > AdManager._instance.levelReloadAdDuration)
            {
                AdManager._instance.ShowCommonInterstitial((result)=>
                    { 
                        if(result)
                        {
                            AdManager._instance.DelayOnShowAds();
                        }
                });
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    float currentAdDisplayTime;
    public void NextLevel()
    {
        if(InternetValidator.Instance)
        {
            if(!InternetValidator.Instance.canProceedToNextLevel)
            {
                CheckNoInterNetPopup();
                return;
            }
        }       

        currentAdDisplayTime = Time.time;
        //Debug.Log("currentAdDisplayTime " + currentAdDisplayTime + " lastAdDisplayTime " + AdManager._instance.lastAdDisplayTime + "Global.backFillAdGapToContinue" + Global.backFillAdGapToContinue);
        if (((currentAdDisplayTime - AdManager._instance.lastAdDisplayTime) > Global.backFillAdGapToContinue)
            && (AdManager._instance.adMobNetworkHandler!=null && 
                AdManager._instance.adMobNetworkHandler.adMobRewardedInterstitial != null &&
                AdManager._instance.adMobNetworkHandler.adMobRewardedInterstitial.CanShowAd()))
            {
            AdManager._instance.rewardTypeToUnlock = RewardType.continuegame;
            if (rewardtext)
            {
                rewardtext.text = "Continue game?";
            }
            rewardCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("IXD " + WorldSelectionHandler.worldSelected+"_"+ Global.CurrentLeveltoPlay);
            donextlevelcall();
        }
    }

    public void donextlevelcall()
    {
        AdManager._instance.ShowLoadingPanel();
        SessionStart = DateTime.UtcNow;
        //ClickSound.Play();
        Global.noOfTries = 0;
        if (Global.CurrentLeveltoPlay < (WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected] - 1))
        {
            Global.CurrentLeveltoPlay += 1;
        }
        else
        {
            WorldSelectionHandler.worldSelected += 1;
            Global.CurrentLeveltoPlay = 0;
        }
        SceneManager.LoadScene("GamePlay_W" + WorldSelectionHandler.worldSelected.ToString() + "_" + (int)(Global.CurrentLeveltoPlay / 5));
        //SceneManager.LoadScene("GamePlay_New_5");
        //SceneManager.LoadScene("GamePlay_W" + WorldSelectionHandler.worldSelected.ToString());

    }
    public void GoBack()
    {
        //ClickSound.Play ();
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(4, 0);
        }
        if (!gameOverPanel.activeInHierarchy && !gameFailed.activeInHierarchy)
        {
            AdManager._instance.ShowCommonInterstitial();
        }
        Global.noOfTries = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelection");
    }
    public void GoBackToWorldSel()
    {
        GoBack();
    }

    bool rewardUsed = false;
    void Awake()
    {
        Debug.Log("Awake World " + WorldSelectionHandler.worldNumb + "Level" + Global.CurrentLeveltoPlay);

        if (AdManager._instance != null)
            AdManager._instance.HideLoadingPanel();
        rewardUsed = false;

        CoinsEffect.Stop();
        //levelON
        int levelactive = 0;
        if (Global.CurrentLeveltoPlay > (((int)(Global.CurrentLeveltoPlay / 5)) * 5))
        {
            levelactive = (Global.CurrentLeveltoPlay - (((int)(Global.CurrentLeveltoPlay / 5)) * 5));
        }
        for (int i = 0; i < LevelsParent.childCount; i++)
        {
            if (i == levelactive)
            {
                LevelsParent.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                LevelsParent.GetChild(i).gameObject.SetActive(false);
                Destroy(LevelsParent.GetChild(i).gameObject);
            }
        }
        if (WorldSelectionHandler.worldSelected == 0 && Global.CurrentLeveltoPlay == 0)
        {
            tutorAnim.SetActive(true);
        }
        /*for (int i = 0; i < LevelsParent.childCount; i++)
        {
            if (i == Global.CurrentLeveltoPlay)
            {
                LevelsParent.GetChild(i).gameObject.SetActive(true);
            }
            else 
            {
                LevelsParent.GetChild(i).gameObject.SetActive(false);
                Destroy(LevelsParent.GetChild(i).gameObject);
            }

        }*/
        //cameraAccess
        if (cameraFollow == null)
        {
            cameraFollow = FindObjectOfType<CameraFollow>();
        }
        else
        {
            return;
        }
        //SlingAccess
        if (slingShot == null)
        {
            slingShot = FindObjectOfType<SlingShot>();
            cameraFollow.GetComponent<Reset>().slingShot = slingShot;
            cameraFollow.GetComponent<CameraMove>().slingShot = slingShot;
        }
        if (slingShot != null)
        {
            slingShot.enabled = false;
            slingShot.slingShootLineRenderer1.enabled = false;
            slingShot.slingShootLineRenderer2.enabled = false;
            slingShot.slingBeltRenderer.enabled = false;
        }
        //BirdsGeneration
        //birdsgroup.transform.position = Camera.main.ScreenToWorldPoint(ballCountTxt.rectTransform.transform.position);
        for (int i = 0; i < Level_ballCount[Global.CurrentLeveltoPlay]; i++)
        {
            GameObject G = Instantiate(ballPrefab, birdsgroup);
            G.name = "Ball" + (i + 1);
        }
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        birds = new List<GameObject>();
        intialbirdcount = birdsgroup.childCount;

        for (int i = 0; i < birdsgroup.childCount; i++)
        {
            birdsgroup.GetChild(i).transform.localPosition = Vector3.zero;
            if (i == (birdsgroup.childCount - 1))
            {
                birdsgroup.GetChild(i).gameObject.SetActive(false);
                rewardBall = birdsgroup.GetChild(i).gameObject;
            }
            else
            {
                birds.Add(birdsgroup.GetChild(i).gameObject);
            }
        }

        object[] anArray = birds.ToArray();

        if (null != anArray && anArray.Length > 0)
        {

            for (int i = 0; i < anArray.Length; i++)
            {
                birds[i].GetComponent<CircleCollider2D>().enabled = false;
                birds[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("pig"));
        Global.target = pigs.Count;
        Global.birdCount = 0;
        Global.TotalbirdCount = birds.Count;
        gameState = GameState.Start;
    }

    //public int[] leveltarget;
    GameObject[] FindObsWithTag(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObs, CompareObNames);
        return foundObs;
    }

    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    void OnEnable()
    {

        if (slingShot != null && cameraFollow != null)
        {
            slingShot.birdThrown += SlingShotBirdThrown;
        }
        InternetValidator.Instance.OnInterNetCheck += CheckNoInterNetPopup;
        AdManager.OnIngameAdClosed += ShowNoAdsButton;
    }

    private void OnDisable()
    {
        InternetValidator.Instance.OnInterNetCheck -= CheckNoInterNetPopup;
        if(InappManager.Instance)
        {
            InappManager.Instance.HideNoAdsButton();
        }
        AdManager.OnIngameAdClosed -= ShowNoAdsButton;

    }

    public void Update()
    {
        try
        {
            if (Global.count >= Global.target && gameOverPanel.activeSelf)
            {
                if (retry.activeSelf)
                {
                    if (!levelup.activeSelf) 
                    {
                        levelup.SetActive(true);
                    }
                }
            }

            // if (isReward) {
            // 	gameOverPanel.SetActive (false);
            // }

            if (gameOverPanel.activeSelf)
            {
                slingShot.SetSlingshotLinerenderersActive(false);
                AdManager._instance.hidebanner();
            }
            if (gameFailed.activeSelf)
            {
                AdManager._instance.hidebanner();
            }
            if (!gameOverPanel.activeSelf && !gameFailed.activeSelf && Global.CurrentLeveltoPlay >= 4 && AdManager._instance.enableBanner)
            {
                AdManager._instance.showbanner();
            }


            switch (gameState)
            {

                case GameState.Start:
                    if (Global.tutorialDisplaye)
                    {
                        tutorAnim.SetActive(false);
                        tutorOkBtn.SetActive(false);
                        pauseBtn.SetActive(true);
                        replayBtn.SetActive(true);
                    }
                    AnimateBirdToSlingshot();
                    Debug.Log("GAMESTART " + slingShot.slingShootState);
                    break;

                case GameState.Playing:
                    if (slingShot != null)
                    {
                        if (slingShot.slingShootState == SlingshotState.BirdFlying &&
                             (BricksBirdsPigsStoppedMoving() || Time.time - slingShot.timeSinceThrown > 3f))
                        {
                            slingShot.enabled = false;

                            slingShot.slingShootLineRenderer1.enabled = false;
                            slingShot.slingShootLineRenderer2.enabled = false;
                            slingShot.slingBeltRenderer.enabled = false;

                            if (cameraFollow != null)
                            {
                                Debug.Log("ANimCamToStartCallBefore " + cameraFollow);
                                AnimateCameraToStartPosition();
                                Debug.Log("ANimCamToStartCall " + cameraFollow);
                            }
                            gameState = GameState.BirdMovingToSlingshot;
                        }
                    }

                    break;

                case GameState.Won:
                case GameState.Lost:
                case GameState.Reward_Video_Started:
                case GameState.RewardState_Success:
                    break;
            }
        }
        catch (Exception excp) { }
    }


    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            ingamevideosuccess();
        }
    }

    public void ingamevideosuccess()
    {
        try
        {
            if (AdManager._instance != null)
            {
                switch (gameState)
                {
                    case GameState.Reward_Video_Completed:
                        if (AdManager._instance.rewardTypeToUnlock == RewardType.extraball)
                        {
                            if (rewardCanvas.activeInHierarchy)
                            {
                                if (!AdManager._instance.rewardedvideosuccess)
                                {
                                    gameState = GameState.Lost;
                                    OnGameFail();
                                }
                                else
                                {
                                    ingamePausePanel.SetActive(true);
                                    rewardCanvas.SetActive(false);
                                    gameState = GameState.RewardState_Success;
                                    AnimateBirdToSlingshot();
                                    Debug.Log("REWARDVIDEOCOMPLETE " + slingShot.slingShootState);
                                    try
                                    {
                                        //if (FirebaseEvents.instance != null)
                                        //{
                                        //    FirebaseEvents.instance.LogFirebaseEvent("RewardVideoSuccess", "Revive", "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                                        //}

                                        if (AdManager._instance)
                                        {
                                            string levelNum = "W" + WorldSelectionHandler.worldNumb + "_L" + Global.CurrentLeveltoPlay;
                                            string targetKey = LevelExtraBallKey.Replace("param", levelNum);
                                            AdManager._instance.FireBaseActions(targetKey,"ExtraBall","success");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //
                                    }
                                }
                            }
                        }
                        else if (AdManager._instance.rewardTypeToUnlock == RewardType.skiplevel)
                        {
                            if (AdManager._instance.rewardedvideosuccess)
                            {
                                SkipLevelBtn.SetActive(false);
                                PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + (Global.CurrentLeveltoPlay), 1);
                                try
                                {
                                    //if (FirebaseEvents.instance != null)
                                    //{
                                    //    FirebaseEvents.instance.LogFirebaseEvent("RewardVideoSuccess", "SkipLevel", "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                                    //}

                                    if(AdManager._instance)
                                    {
                                        string levelNum = "W" + WorldSelectionHandler.worldNumb + "_L" + Global.CurrentLeveltoPlay;
                                        string targetKey = LevelSkippedKey.Replace("param", levelNum);
                                        AdManager._instance.FireBaseActions(targetKey, "skiplevel", "success");
                                    }
                                }
                                catch (Exception e)
                                {
                                    //
                                }
                                AdManager._instance.rewardTypeToUnlock = RewardType.None;
                                donextlevelcall();
                            }
                        }
                        else if (AdManager._instance.rewardTypeToUnlock == RewardType.continuegame)
                        {
                            if (AdManager._instance.rewardedvideosuccess)
                            {                                
                                rewardCanvas.SetActive(false);
                                try
                                {
                                    //if (FirebaseEvents.instance != null)
                                    //{
                                    //    FirebaseEvents.instance.LogFirebaseEvent("RewardVideoSuccess", "continuegame", "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                                    //}
                                }
                                catch (Exception e)
                                {
                                    //
                                }
                                AdManager._instance.rewardTypeToUnlock = RewardType.None;
                                Invoke("donextlevelcall",0.25f);
                            }
                        }
                        break;
                }

                if(AdManager._instance.rewardedvideosuccess)
                {
                    AdManager._instance.rewardedvideosuccess = false;
                }
            }


        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_ingamevideosuccess", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void AnimateBirdToSlingshot()
    {
        try
        {

            if (Global.count >= Global.target)
            {
                if (slingShot != null)
                {
                    slingShot.enabled = false;
                }
                if (isReward)
                {
                    rewardBall.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    birds[currentBirdIndex].GetComponent<SpriteRenderer>().enabled = false;
                }
                if (slingShot != null)
                {
                    slingShot.slingShootLineRenderer1.enabled = false;
                    slingShot.slingShootLineRenderer2.enabled = false;
                    slingShot.slingBeltRenderer.enabled = false;
                }
                return;
            }
            GameObject ball = null;
            if (gameState == GameState.RewardState_Success)
            {
                ball = rewardBall;
                rewardBall.SetActive(true);
                isReward = true;
                gameOverPanel.SetActive(false);
                gameState = GameState.Playing;
                //Invoke("enableSling",0.25f);
            }
            else
            {
                if (gameState == GameState.Won || gameState == GameState.Lost)
                {
                    return;
                }
                ball = birds[currentBirdIndex];
            }
            if (ball != null)
            {
                slingShot.birdToThrow = ball;
            }
            if (slingShot != null)
            {
                gameState = GameState.BirdMovingToSlingshot;
                ball.GetComponent<SpriteRenderer>().enabled = true;
                //ballCreationEffect.Play();
                ball.transform.positionTo(Vector2.Distance(ball.transform.position / 10,
                    slingShot.birdWaitPosition.position) / 10,
                    slingShot.birdWaitPosition.position).
                setOnCompleteHandler((x) =>
                {
                    x.complete();
                    x.destroy();
                    Global.isBottleCollission = false;
                    gameState = GameState.Playing;
                    if (isReward)
                    {
                        rewardBall.GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else
                    {
                        birds[currentBirdIndex].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    slingShot.enabled = true;
                    slingShot.slingShootState = SlingshotState.Idle;
                    Debug.Log("GAMEMANAGER   CALL  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$   " + slingShot.slingShootState);
                    /*if (ball.transform.position == slingShot.birdWaitPosition.position)
                     {
                         if (!Waited(1))
                             return;
                     }*/
                });
            }
            float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.startingPosition) / 10f;
            if (duration == 0.0f)
                duration = 0.1f;
            Camera.main.transform.positionTo(duration,
                cameraFollow.startingPosition).
            setOnCompleteHandler((x) => {
                //nothing
            });
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "AnimateBtoSS", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void needextraballactivate()
    {
        if (AdManager._instance != null)
        {
            AdManager._instance.rewardTypeToUnlock = RewardType.extraball;     

            AdManager._instance.ShowRewardedVideo(result =>
            {
                // Debug.Log("asdf Reward status " + result);
                if (result)
                {
                    gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = true;
                    ingamevideosuccess();
                }
            },AdType.RewardContinue);

        }
    }

    public void SkiplevelRewardActivate()
    {
        if (AdManager._instance != null)
        {
            AdManager._instance.rewardTypeToUnlock = RewardType.skiplevel;

            AdManager._instance.ShowRewardedVideo(result =>
            {
                // Debug.Log("asdf Reward status " + result);
                if (result)
                {
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = true;
                    ingamevideosuccess();
                }
            },AdType.Reward);

        }
    }
    public GameObject getBall()
    {
        if (gameState == GameState.RewardState_Success)
        {
            return rewardBall;
        }
        else
            return birds[currentBirdIndex];
    }

    void OnDestroy()
    {
        //AdManager._instance.hidebanner();
    }
    private bool Waited(float seconds)
    {
        timermax = seconds;
        timer += Time.deltaTime;
        if (timer >= timermax)
        { return true; }
        return false;
    }
    void enableSling()
    {
        if (slingShot != null)
        {
            slingShot.slingShootLineRenderer1.enabled = true;
            slingShot.slingShootLineRenderer2.enabled = true;
            slingShot.slingBeltRenderer.enabled = true;
            slingShot.enabled = true;
        }
    }

    bool BricksBirdsPigsStoppedMoving()
    {

        foreach (var item in bricks.Union(birds).Union(pigs))
        {
            if (item != null)
            {
                return false;
            }
        }
        return true;
    }

    private bool AllPigsAreDestroyed()
    {
        return pigs.All(x => x == null);
    }
    bool flag = true;
    public void resetFlag()
    {
        flag = true;
    }
    private void AnimateCameraToStartPosition()
    {
        try
        {
            if (!flag)
            {
                return;
            }
            Invoke("resetFlag", 2f);
            flag = false;
            if (slingShot != null)
            {
                slingShot._ballType = SlingShot.BallType.normal;
                float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.startingPosition) / 10f;
                if (duration == 0.0f)
                    duration = 0.1f;

                if (AllPigsAreDestroyed())
                {
                    gameState = GameState.Won;
                    Invoke("checkResult", 0.25f);
                }
                else if (birdsgroup.childCount <= 1)
                {
                    //Debug.LogError("imhere");
                    gameState = GameState.Lost;
                    Global.isBottleCollission = false;
                    Invoke("checkBottleCollision", 3f);
                }

                Camera.main.transform.positionTo(duration,
                    cameraFollow.startingPosition).
                setOnCompleteHandler((x) =>
                {
                    cameraFollow.isFollowing = false;
                    if (AllPigsAreDestroyed())
                    {
                        gameState = GameState.Won;
                        Invoke("checkResult", 0.25f);
                    }
                    else if (birdsgroup.childCount <= 0)
                    {
                        //Debug.LogError("imhere");
                        gameState = GameState.Lost;
                        Global.isBottleCollission = false;
                        Invoke("checkBottleCollision", 3f);
                    }
                    else
                    {
                        if (slingShot.birdToThrow != null)
                        {
                            if (slingShot.birdToThrow != null && !slingShot.birdToThrow.transform.IsChildOf(birdsgroup))
                            {
                                if (slingShot.birdToThrow != null)
                                {
                                    Destroy(slingShot.birdToThrow);

                                }
                            }
                        }

                        currentBirdIndex = (intialbirdcount - birdsgroup.childCount);
                        AnimateBirdToSlingshot();
                        Debug.Log("From animate camera to START POSITION*****************    " + slingShot.slingShootState);
                        ShowBallsCount();
                    }
                    StartCoroutine(CheckBottlsRemaining());
                });
                slingShot._ballType = SlingShot.BallType.normal;
                slingShot.ChangeDisplayBallSkin();
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "AnimateCameratoStartPos", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void OnGameFail()
    {
        try
        {
            if (!gameFailed.activeSelf)
            {
                // Debug.LogError("EndGameWon World" + WorldSelectionHandler.worldNumb + "L" + Global.CurrentLeveltoPlay);
                //AdManager._instance.ShowGameFailInterstitial();
                //playTime = Time.time - startTime;
                // Debug.LogError("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay + "Playtime = " + playTime);
                // Firebase.Analytics.FirebaseAnalytics.LogEvent("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay, "Playtime", playTime);
                //Analytics.CustomEvent("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay + "Playtime = ", new Dictionary<string, object>
                //                                {
                //                                    { "LEVEL", ""+playTime },

                //                             });
                Invoke("DelayShowLevelCompleteAd", 1f);
                gameFailed.SetActive(true);
                pauseBtn.SetActive(false);
                replayBtn.SetActive(false);
                rewardCanvas.SetActive(false);
                //AudioManager.Instance.LevFail.Play();
                if (SoundsHandler.Instance != null)
                {
                    if (!SoundManager.IsMuted())
                    {
                        SoundsHandler.Instance.PlaySource1Clip(1, 0);
                    }
                }
                GOTeleHook();

                // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay));

                //Firebase.Analytics.FirebaseAnalytics.LogEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Fail ");

                Analytics.CustomEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Failed ", new Dictionary<string, object>
                                            {
                                                { "LEVEL", Global.retryCount },

                                         });
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_onGameFail", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void DelayShowLevelFailAd()
    {
        if (AdManager._instance != null)
        {            
            AdManager._instance.ShowGameFailInterstitial();
            //AdManager._instance.ShowFBInterstitial();
        }
    }

    public void GOTeleHook()
    {
        try
        {
            if (Global.CurrentLeveltoPlay < 10 && WorldSelectionHandler.worldSelected < 2)
            {
                SessionEnd = DateTime.UtcNow;
                TimeSpan SessionDuration = (SessionEnd - SessionStart);
                string status = "";
                int TD = SessionDuration.Seconds;
                if (TD > 50)
                {
                    status = "Grt50";
                }
                else if (TD > 40 && TD <= 50)
                {
                    status = "Grt40Less50";
                }
                else if (TD > 30 && TD <= 40)
                {
                    status = "Grt30Less40";
                }
                else if (TD > 20 && TD <= 30)
                {
                    status = "Grt20Less30";
                }
                else if (TD > 10 && TD <= 20)
                {
                    status = "Grt10Less20";
                }
                else
                {
                    status = "Lessthan10";
                }
                try
                {
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("S" + (WorldSelectionHandler.worldSelected + 1).ToString() + "_L" + (Global.CurrentLeveltoPlay + 1).ToString() + "_SD_" + status);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            if (Global.CurrentLeveltoPlay == 9 && WorldSelectionHandler.worldSelected == 0)
            {
                string status = "";
                if (Time.realtimeSinceStartup > 500)
                {
                    status = "MoreThan_9Mins";
                }
                else
                {
                    status = "LessThan_9Mins";
                }
                try
                {
                    if (FirebaseEvents.instance != null)
                    {
                        FirebaseEvents.instance.LogFirebaseEvent("GSD_" + status);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_gotelehook", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    IEnumerator CheckBottlsRemaining()
    {
        yield return new WaitForSeconds(2);
        Pig[] bottles = GameObject.FindObjectsOfType<Pig>();
        try
        {
            Debug.Log("bottles pending.. 1" + bottles.Length);
            if (bottles.Length <= 0)
            {
                ShowNewLevelComplete();
                yield break;
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_CBR", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
        yield return new WaitForSeconds(1);
        try
        {
            bottles = GameObject.FindObjectsOfType<Pig>();
            Debug.Log("bottles pending.. 2 " + bottles.Length);
            if (bottles.Length <= 0)
            {
                ShowNewLevelComplete();
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_cbr", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    private void checkBottleCollision()
    {
        try
        {
            Debug.Log("Birds count  " + birdsgroup.childCount);

            if (!Global.isBottleCollission)
            {
                //if (!rewardCanvas.activeSelf && (Global.target - Global.count) <= 2 && !Global.rewardUsed)
                if (!rewardCanvas.activeSelf && (Global.target - Global.count) <= 2 && !rewardUsed)
                {
                    ingamePausePanel.SetActive(false);
                    if (rewardtext)
                    {
                        rewardtext.text = "Need Extra Ball?";
                    }
                    rewardUsed = true;

                    //AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
                    //rewardCanvas.SetActive(true);

                    if ((AdManager._instance.adMobNetworkHandler!=null &&
                        AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo!=null &&
                        AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo.CanShowAd()) 
                        || (AdManager._instance.levelPlayNetworkHandler!=null &&
                            AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo!=null &&
                            AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo.IsAdReady()))
                    {
                        AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
                        rewardCanvas.SetActive(true);
                    }
                    else 
                    {
                    OnGameFail();
                    }

                }
                else if (birdsgroup.childCount <= 1)
                {
                    OnGameFail();
                }
            }
            else if (birdsgroup.childCount <= 1)
            {
                if (!gameOverPanel.activeSelf)
                    OnGameFail();
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_cbc", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    private void SlingShotBirdThrown()
    {
        if (isReward)
        {
            isReward = false;
            cameraFollow.birdToFollow = rewardBall.transform;
        }
        else
        {
            cameraFollow.birdToFollow = birds[currentBirdIndex].transform;
        }
        cameraFollow.isFollowing = true;
    }


    void checkResult()
    {
        if (isReward)
        {
            return;
        }
        if (Global.count >= Global.target)
        {
            showLevelComplete();
        }
    }

    void showLevelComplete()
    {
        try
        {
            if (gameOverPanel.activeSelf)
                return;

            gameOverPanel.SetActive(true);
            //AudioManager.Instance.LevClear.Play();

            pauseBtn.SetActive(false);
            replayBtn.SetActive(false);
            if (Global.CurrentLeveltoPlay < (WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected] - 1))
            {
                LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay);
                LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay + 1);
            }

            /*if (Give3Stars[Global.CurrentLeveltoPlay])
            {
                PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 3);
                stars[2].SetActive(true);
                Debug.LogError("Entered 3 star default");
            }
            else
            {*/

            //Debug.LogError("Skipped 3 star default");
            /* if (birds.Count - Global.birdCount > 1 && PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 3)
             {
                 PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 3);
             }
             else if (birds.Count - Global.birdCount > 0 && PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 2)
             {
                 PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 2);
             }
             else if (PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 1)
             {
                 PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 1);
             }*/
            if (birds.Count - Global.birdCount > 1)
            {
                stars[2].SetActive(true);
            }
            else if (birds.Count - Global.birdCount > 0)
            {
                stars[1].SetActive(true);
            }
            else
            {
                stars[0].SetActive(true);
            }
            // }
            //AdManager._instance.ShowGameWinInterstitial();
            CallAdInPigScript();

           
            // GOTeleHook();

            //Firebase.Analytics.FirebaseAnalytics.LogEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Finish ");

            // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelUp, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay));
            Analytics.CustomEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Finish ", new Dictionary<string, object>
                                            {
                                                { "LEVEL", Global.retryCount },

                                         });
            //Uncomment for coin animation
            //StartCoroutine(ShowCoinsAnimation());
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_SLC", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public float playTime;

    void ShowNewLevelComplete()
    {
        try
        {
            if (gameOverPanel.activeSelf)
                return;


            gameOverPanel.SetActive(true);
            //AudioManager.Instance.LevClear.Play();


            pauseBtn.SetActive(false);
            replayBtn.SetActive(false);
            //Debug.Log("unlock next lvl " + Global.CurrentLeveltoPlay);
            if (Global.CurrentLeveltoPlay < (WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected] - 1))
            {
                LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay);
                LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay + 1);
            }
            if ((Global.TotalbirdCount - Global.birdCount > 1 && PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 3))
            {
                PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 3);
            }
            else if (Global.TotalbirdCount - Global.birdCount > 0 && PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 2)
            {
                PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 2);
            }
            else if (PlayerPrefs.GetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay) < 1)
            {
                PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 1);
            }

            if ((Global.TotalbirdCount - (Global.birdCount)) > 1)
            {
                NewShowStars(3);
            }
            else if ((Global.TotalbirdCount - (Global.birdCount)) > 0)
            {
                NewShowStars(2);
            }
            else
            {
                NewShowStars(1);
            }
            //AdManager._instance.ShowGameWinInterstitial();
            CallAdInPigScript();

            // GOTeleHook();
            // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelUp, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay));
            Analytics.CustomEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Finish ", new Dictionary<string, object>
                                            {
                                                { "LEVEL", Global.retryCount },

                                         });
            //Uncomment for coin animation
            //StartCoroutine(ShowCoinsAnimation());
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GM_snlc", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void DelayShowLevelCompleteAd()
    {
        try
        {
            if (AdManager._instance != null)
            {
                AdManager._instance.ShowGameWinInterstitial();               
                // AdManager._instance.ShowFBInterstitial();                
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GameManager", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void CallAdInPigScript()
    {
       Invoke("DelayShowLevelCompleteAd", 1.75f);
    }

    public void ShowNoAdsButton()
    {
        if (InappManager.Instance && (gameOverPanel.activeSelf))
        {
            if (Global.CurrentLeveltoPlay > 0 && (Global.CurrentLeveltoPlay % (InappManager.Instance.PremiumPopUpIteration - 1) == 0))
            {
                InappManager.Instance.ShowNoAdsButton();
            }
        }
    }
    public delegate void coinsUpdated();
    public static event coinsUpdated coinsUpdatedEvent;
    public static void AddCoins()
    {
        if (coinsUpdatedEvent != null)
            coinsUpdatedEvent();
    }
    public static void DeductCoins(int coinsToDeduct)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - coinsToDeduct);
        if (coinsUpdatedEvent != null)
            coinsUpdatedEvent();
    }

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt("coins");
    }

    int[] powerPrices = new int[] { 100, 100, 100, 100, 100 };
    public void AddPowerBalls(int index)
    {
        Debug.Log("coins have " + GetCoins() + " powerPrice is " + powerPrices[index]);
        if (GetCoins() >= powerPrices[index])
        {
            DeductCoins(powerPrices[index]);
            ballCounts[index]++;
            PlayerPrefsX.SetIntArray("ballPowerCounts", ballCounts);
            SetCountTexts();
        }
        else
        {
            Debug.Log("Dont  have enough coins");
        }
    }



    [SerializeField] GameObject[] stars;
    public void ShowStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
            stars[i].transform.DOLocalMoveY(1000, .5f).From().SetDelay((i * .3f) + 1.5f).SetEase(Ease.OutBounce);
            stars[i].transform.DOScale(0, .5f).From().SetDelay((i * .4f) + 1.5f).SetEase(Ease.OutBounce);
        }
    }

    public void NewShowStars(int count)
    {
        // Debug.LogError("EndGameWon World" + WorldSelectionHandler.worldNumb + "L" + Global.CurrentLeveltoPlay);
        // playTime = Time.time - startTime;
        // Debug.LogError("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay + "Playtime = " + playTime);
        // Firebase.Analytics.FirebaseAnalytics.LogEvent("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay, "Playtime", playTime);
        //Analytics.CustomEvent("W" + WorldSelectionHandler.worldNumb + " L" + Global.CurrentLeveltoPlay + "Playtime = ", new Dictionary<string, object>
        //                                    {
        //                                        { "LEVEL", ""+playTime },

        //                                 });

        try
        {
            GOTeleHook();
            for (int i = 0; i < count; i++)
            {
                stars[i].SetActive(true);
                stars[i].transform.DOLocalMoveY(500, 1.2f).From().SetDelay((i * .3f) + .5f).SetEase(Ease.OutBounce);
                stars[i].transform.DOScale(0, 1.2f).From().SetDelay((i * .5f) + .5f).SetEase(Ease.OutCirc).OnStart(PlayStarSound);
                // SoundsHandler.Instance.PlaySource2Clip(3, 0);
                Debug.Log("NEWSHOW STARRRRRRRRRRSSSSSSSSSSSRSRRSRWSRSRRSRSRSRSRSR");
                // SoundManager.PlaySFX("Click", false, (i * 0.4f) + 0.5f);
            }
            // PlayStarSound(count);
            int coinsEarned = PlayerPrefs.GetInt("coins");
            GameManager.Instance.totalcoinsatWin.text = coinsEarned.ToString();
            int currentStars = LevelSelectionHandler.levelStars[Global.CurrentLeveltoPlay];
            if (currentStars < count)
                StartCoroutine(ShowCoinsAnimation(count, currentStars));


            LevelSelectionHandler.SetStarsOfLevel(Global.CurrentLeveltoPlay, WorldSelectionHandler.worldSelected, count);
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GameManager", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public ParticleSystem CoinsEffect;

    public IEnumerator ShowCoinsAnimation(int t, int s)
    {
        int coinsEarned = PlayerPrefs.GetInt("coins");
        GameManager.Instance.totalcoinsatWin.text = coinsEarned.ToString();
        yield return new WaitForSeconds(2.0f);
        t = (t - s);
        coinsEarned = PlayerPrefs.GetInt("coins") + (t * 100);

        CoinsEffect.Play();
        GameManager.Instance.totalcoinsatWin.text = coinsEarned.ToString();

        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (t * 100));
        GameManager.AddCoins();
        //int j = 0;
        //while (j < 5)
        //{
        //    GameObject _coin = Instantiate(GameManager.Instance._coinRef, GameManager.Instance.spawnPoint.localPosition, Quaternion.identity) as GameObject;
        //    _coin.transform.SetParent(GameManager.Instance.coinSpace);
        //    _coin.transform.localScale = Vector3.one;
        //    _coin.transform.localPosition = GameManager.Instance.spawnPoint.localPosition;
        //    _coin.transform.localScale = Vector3.zero;
        //    yield return new WaitForSeconds((j) * .1f);
        //    _coin.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutBounce);
        //    _coin.GetComponent<CoinAnimation>().StartCoroutine(
        //    _coin.GetComponent<CoinAnimation>().Animate(.5f, GameManager.Instance.reachPoint));
        //    j++;
        //}


        yield return null;
    }

    void PlayStarSound()
    {
        if (!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(3, 0);

            //if (count == 2)
            //    SoundsHandler.Instance.PlaySource1Clip(6, 1.0f);
            //else
            //{
            //    SoundsHandler.Instance.PlaySource1Clip(6, 1.0f);
            //    SoundsHandler.Instance.PlaySource2Clip(7, 1.2f);
            //}

        }


    }

    [SerializeField] Transform[] ballCountTxts;
    public static int[] ballCounts = new int[] { 2, 2, 2, 2, 2, -1 };
    public bool ChangeBallCounts(int index)
    {
        if (ballCounts[index] <= 0)
            return false;

        ballCounts[index]--;
        PlayerPrefsX.SetIntArray("ballPowerCounts", ballCounts);
        SetCountTexts();
        return true;
    }
    void SetCountTexts()
    {
        for (int i = 0; i < ballCountTxts.Length; i++)
        {
            ballCountTxts[i].GetComponentInChildren<Text>().text = ballCounts[i] + "";
            ballCountTxts[i].GetComponent<Button>().interactable = ballCounts[i] != 0;
        }
    }

    public ParticleSystem ballCreationEffect;
    [SerializeField] Text ballCountTxt;
    void ShowBallsCount()
    {
        int i = ((birds.Count - 1) - currentBirdIndex);
        if (i >= 0)
        {
            ballCountTxt.text = i.ToString();
        }
        else
        {
            ballCountTxt.text = "0";
        }
    }
    //public GameObject Store;
    public void StorePanel()
    {
        try
        {
            if (AdManager._instance != null)
            {
                if (FindObjectOfType<StoreManager>().gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    //Store.SetActive(false);
                    AdManager._instance.HideStorePanel();
                }
                else
                {
                    //Store.SetActive(true);
                    AdManager._instance.ShowStorePanel();
                    StoreManager._instance.ScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    StoreManager._instance.ScrollContent2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    try
                    {
                        if (FirebaseEvents.instance != null)
                        {
                            FirebaseEvents.instance.LogFirebaseEvent("StoreBtn_clicked_FromLevels");
                        }
                    }
                    catch (Exception e)
                    {
                        //
                    }
                }
                GameManager.Instance.totalcoinsatWin.text = PlayerPrefs.GetInt("coins").ToString();
                FindObjectOfType<StoreManager>().CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "GameManager", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void CheckNoInterNetPopup(bool status=false)
    {
        if (InternetValidator.Instance != null)
        {
            if (gameOverPanel.activeSelf )
            {
                InternetValidator.Instance.CheckNoInterNetPopup();
            }
        }
    }
    void CheckPreimumPopUP()
    {
        if (InappManager.Instance != null)
        {
            if (gameOverPanel.activeSelf)
            {
                InappManager.Instance.CheckPremiumPopup();
            }
        }
    }
} // GameManager