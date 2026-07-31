using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System;
using UnityEngine.Analytics;
using DG.Tweening;

public class Pig : MonoBehaviour
{

    private AudioSource audioSource;
    private float changeSpriteHealth;
    static GameManager gameMngr;
    private Explodable _explodable;
    private System.Boolean firstTime = true;

    public float health = 150f;
    public AudioClip broke;
    [SerializeField]
    public int count;

    int bottleSoundCount = 10;
    //public Sprite[] BottleSkins;

    void Start()
    {
        try
        {
            Debug.Log($"BottleSelected:{PlayerPrefs.GetInt("BottleSelected", 0)}");
            if (PlayerPrefs.GetInt("BottleSelected", 0) > 0)
            {
                GetComponent<SpriteRenderer>().sprite = GameManager.Instance.BottleSkins[PlayerPrefs.GetInt("BottleSelected", 0)];
            }
            Invoke("startSound", 2f);
            gameMngr = GameManager.Instance;
            GetComponent<AudioSource>().playOnAwake = false;
            GetComponent<AudioSource>().clip = broke;
            _explodable = GetComponent<Explodable>();
            audioSource = GetComponent<AudioSource>();
            changeSpriteHealth = health - 30f;
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void startSound()
    {
        bottleSoundCount = 0;
    }

    void explodebottle()
    {
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
        Global.brokenBotList.Add(_explodable);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        try
        {
            if (target.gameObject.tag == "water")
            {
                gameMngr.gameState = GameState.Lost;
                gameMngr.OnGameFail();
            }
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    bool first = true;
    void OnCollisionEnter2D(Collision2D target)
    {
        try
        {

            if (target.gameObject.tag == "pig" && bottleSoundCount < 2)
            {
                bottleSoundCount++;
                if (SoundsHandler.Instance != null)
                {
                    if (!SoundManager.IsMuted())
                    {
                        SoundsHandler.Instance.PlaySource2Clip(0, 0);
                    }
                }
            }

            if (target.gameObject.tag == "Bird" && first)
            {
                if (SoundsHandler.Instance != null)
                {
                    if (!SoundManager.IsMuted())
                    {
                        SoundsHandler.Instance.PlaySource2Clip(1, 0);
                    }
                }
                first = false;
            }


            if (target.gameObject.tag == "ground")
            {
                //audioSource.Play();

                if (firstTime)
                {
                    if (!SoundManager.IsMuted())
                    {
                        audioSource.Play();
                    }
                    firstTime = false;
                }
                if (!Global.botList.Contains(gameObject))
                {
                    Global.count = Global.count + 1;
                    Debug.Log($"bottle count:{Global.count},targetBottles:{Global.target}.{gameMngr.gameOverPanel.activeSelf}");

                    Global.isBottleCollission = true;
                    Global.botList.Add(gameObject);
                    MultiSetHandler.OnBottlesBreak?.Invoke();

                    if (Global.count >= Global.target && !gameMngr.gameOverPanel.activeSelf)
                    {
                        Debug.Log("Level complete");
                        gameMngr.gameState = GameState.Won;
                        /* gameMngr.gameOverPanel.GetComponent<UIBase>().bottomElements.Delay = 2.75f; */
                        gameMngr.gameOverPanel.SetActive(true);
                        //level won audio
                        if (SoundsHandler.Instance != null)
                        {
                            if (!SoundManager.IsMuted())
                            {
                                SoundsHandler.Instance.PlaySource1Clip(0, 0);
                            }
                        }
                        gameMngr.pauseBtn.SetActive(false);
                        gameMngr.replayBtn.SetActive(false);
                        Debug.Log("unlock next lvl " + Global.CurrentLeveltoPlay);
                        Debug.Log(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "-Finish");
                        Debug.Log($"IsFirebaseReady{FirebaseEvents.IsFirebaseReady}");
                        try
                        {
                            if (FirebaseEvents.IsFirebaseReady)
                            {
                                Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelComplete_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                            }
                        }
                        catch
                        {
                            Debug.Log("Firebase event LevelComplete logging failed");
                        }
                        AnalyticsManager.LogEvent(GameAnalyticsSDK.GAProgressionStatus.Complete);

                        try
                        {
                            Analytics.CustomEvent(WorldSelectionHandler.worldSelected + " " + Global.CurrentLeveltoPlay + "- Finish ", new Dictionary<string, object>
                                            {
                                                { "LEVEL", Global.retryCount },

                                         });
                        }
                        catch
                        {
                            Debug.Log("Custom event LevelComplete logging failed");
                        }

                        if (Global.CurrentLeveltoPlay < (WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected] - 1))
                        {
                            LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay);
                            LevelSelectionHandler.UnlockLevel(Global.CurrentLeveltoPlay + 1);
                        }
                        else
                        {
                            gameMngr.levelNo.text = (Global.worldNames[WorldSelectionHandler.worldSelected]+" levels CLEARED").ToUpper();
                            if(gameMngr.gameOverFrame)gameMngr.gameOverFrame.SetActive(false);
                            if (WorldSelectionHandler.worldSelected == 4)
                            {
                                Debug.Log("All LEVELS CLEARED");
                                gameMngr.levelNo.text = "All Levels Cleared";
                                gameMngr.retry.GetComponent<Button>().interactable = false;
                                gameMngr.menu.GetComponent<Button>().interactable = false;
                                gameMngr.levelup.GetComponent<Button>().interactable = false;
                                int totalStars = Global.TotalStarsAchivedWorld1 + Global.TotalStarsAchivedWorld2 + Global.TotalStarsAchivedWorld3 + Global.TotalStarsAchivedWorld4 + Global.TotalStarsAchivedWorld5;
                                Debug.Log($"[Show GameCompletionPopup]totalStars:{totalStars},getNatureStars:{Global.TotalStarsAchivedWorld1},getDesertStars:{Global.TotalStarsAchivedWorld2},getSnowStars:{Global.TotalStarsAchivedWorld3},getVolcanoStars:{Global.TotalStarsAchivedWorld4},getSpaceStars:{Global.TotalStarsAchivedWorld5}");
                                if (GameCompletionPopup.Instance != null)
                                {
                                    gameMngr?.StartCoroutine(gameMngr.AnimatePanelsTransition(totalStars));
                                }
                            }
                        }

                        Debug.Log($"Give Stars {gameMngr.Give3Stars[Global.CurrentLeveltoPlay]}");
                        //Block for 3 stars logic -- Added newly
                        if (gameMngr.Give3Stars[Global.CurrentLeveltoPlay])
                        {
                            //PlayerPrefs.SetInt("W" + WorldSelectionHandler.worldNumb + "level" + Global.CurrentLeveltoPlay, 3);
                            gameMngr.NewShowStars(3);

                        }
                        else
                        {
                            Debug.Log($"Give Stars {Global.TotalbirdCount - (Global.birdCount)}");
                            if ((Global.TotalbirdCount - (Global.birdCount)) > 1)
                            {
                                gameMngr.NewShowStars(3);
                            }
                            else if ((Global.TotalbirdCount - (Global.birdCount)) > 0)
                            {
                                gameMngr.NewShowStars(2);
                            }
                            else
                            {
                                gameMngr.NewShowStars(1);
                            }

                        }

                        //StartCoroutine( DelayShowLevelCompleteAd());

                        //AdManager._instance.ShowGameWinInterstitial();
                        gameMngr.CallAdInPigScript();                       

                        if (SingularEvents.instance!=null)
                        {
                            SingularEvents.instance.SendLevelCompleteEvent(WorldSelectionHandler.worldSelected, Global.CurrentLeveltoPlay);
                            SingularEvents.instance.SendAchieventmentEvent();
                        }

                        if (AdManager._instance)
                            AdManager._instance.HidebannerAd();

                        if(CustomAdManager.Instance)CustomAdManager.Instance.ShowBanner();

                        //Uncomment for coin animation
                        //GameObject.FindObjectOfType<GameManager>().StartCoroutine(GameObject.FindObjectOfType<GameManager>().ShowCoinsAnimation());
                    }

                    if (null != _explodable)
                    {
                        try
                        {
                            Invoke("explodebottle", 0.5f);
                        }
                        catch (Exception exp)
                        {
                        }
                    }
                    //Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelUp, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay));
                    //FirebaseEvents.instance.LogFirebaseEvent("LevelComplete_"+ "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                    //Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelComplete_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay);
                }
            }
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    private IEnumerator DelayShowLevelCompleteAd()
    {
        yield return new WaitForSeconds(1.75f);
        try
        {
            Debug.LogError("admanager ...DelayShowLevelCompleteAd");


            if (AdManager._instance != null)
            {
                Debug.LogError("admanager ...");
                if (!AdManager._instance.isLaunchAdShown && AdManager._instance.adMobNetworkHandler.adMobLaunchInterstitial.CanShowAd())
                {
                    Debug.LogError("admanager ...isLaunchAdShown");

                    AdManager._instance.ShowLaunchInterstitial();
                }
                else
                {
                    Debug.LogError("admanager ...isLaunchAdShown else");

                    AdManager._instance.ShowGameWinInterstitial();
                }
                InternetValidator.Instance.CheckNoInterNetPopup();

                //AdManager._instance.ShowFBInterstitial();
            }
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public  bool bottlesfound = false;
    IEnumerator FindAllBottlesMoved()
    {
        bottlesfound = true;
        yield return new WaitForSeconds(.3f);
        try
        {
            Pig[] bottles = GameObject.FindObjectsOfType<Pig>();
            int totaleffected = 0;
            foreach (Pig bot in bottles)
            {
                if (bot.GetComponent<Rigidbody2D>().velocity.magnitude > 1)
                    totaleffected++;
            }

            /*if (totaleffected >= bottles.Length)
            {
                Time.timeScale = .75f;
                Debug.Log("totaleffected");
            }

            yield return new WaitForSeconds(0.25f);

            if (totaleffected >= bottles.Length)
            {
                Time.timeScale = .5f;
                Debug.Log("totaleffected");
            }

            yield return new WaitForSeconds(0.25f);
            Time.timeScale = 1f;
            */
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
    bool hitonce = false;


    void Update()
    {
        try
        {
            //_explodable.deleteFragments ();
            if (Global.count >= Global.target && gameMngr.gameOverPanel.activeSelf)
            {
                if (GameManager.Instance.retry.activeSelf)
                {
                    if (!GameManager.Instance.levelup.activeSelf)
                    {
                        GameManager.Instance.levelup.SetActive(true);
                    }
                }

            }
        }
        catch (Exception exp)
        {
            try
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
                        FirebaseEvents.instance.LogFirebaseEvent("Exception", "Pig", exp.Message + "at " + errorline);
                    }
                }
                catch (Exception e)
                {
                    //
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }


} // Pig