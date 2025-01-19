using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;


public class SpinWheel : MonoBehaviour
{
    public List<int> prize;
    public List<AnimationCurve> animationCurves;

    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    private int itemNumber;
    public GameObject FreeSpinBtn, WatchVideoSpinBtn, NoFreeSpinsAvailable;
    public bool _freeSpinAvailable = false;
    public AdManager _adManager;
    void Start()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;
        _adManager = FindObjectOfType<AdManager>();
        //////////
        ///Spin Availability
        try
        {
            if (!PlayerPrefs.HasKey("OldDate"))
            {
                PlayerPrefs.SetInt("OldDate", System.DateTime.UtcNow.DayOfYear);
                _freeSpinAvailable = true;
                PlayerPrefs.SetInt("FSUsed", 0);
                PlayerPrefs.SetInt("WVSpin", 0);
            }
            else
            {
                //Store the current time when it starts
                int DayofYear = System.DateTime.UtcNow.DayOfYear;
                int oldDate = PlayerPrefs.GetInt("OldDate");

                if (DayofYear != oldDate && DayofYear > oldDate)
                {
                    _freeSpinAvailable = true;
                    PlayerPrefs.SetInt("FSUsed", 0);
                    PlayerPrefs.SetInt("WVSpin", 0);
                    PlayerPrefs.SetInt("OldDate", System.DateTime.UtcNow.DayOfYear);
                }
                else if (DayofYear == oldDate)
                {
                    if ((PlayerPrefs.GetInt("FSUsed", 0) == 0))
                    {
                        _freeSpinAvailable = true;
                        PlayerPrefs.SetInt("FSUsed", 0);
                        PlayerPrefs.SetInt("WVSpin", 0);
                    }
                    else
                    {
                        _freeSpinAvailable = false;
                    }
                }
            }
            ///////////
            ///End
            if (_freeSpinAvailable)
            {
                NoFreeSpinsAvailable.SetActive(false);
                FreeSpinBtn.SetActive(true);
                WatchVideoSpinBtn.SetActive(false);
            }
            else
            {
                int DayofYear = System.DateTime.UtcNow.DayOfYear;
                int oldDate = PlayerPrefs.GetInt("OldDate");
                FreeSpinBtn.SetActive(false);
                if (DayofYear == oldDate)
                {
                    if (_adManager.rewardBasedVideo.IsAdReady())
                    {
                        MakeWVAvaialable();
                    }
                    else
                    {
                        NoFreeSpinsAvailable.SetActive(true);
                        WatchVideoSpinBtn.SetActive(false);
                    }
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
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "spinwheel_Start", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }
    public void MakeWVAvaialable()
    {
        if (PlayerPrefs.GetInt("WVSpin") == 0)
        {
            NoFreeSpinsAvailable.SetActive(false);
            WatchVideoSpinBtn.SetActive(true);
        }
        else
        {
            NoFreeSpinsAvailable.SetActive(true);
            WatchVideoSpinBtn.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spinning)
        {
            SpinIt();
        }
    }

    public void SpinIt()
    {
        randomTime = UnityEngine.Random.Range(3, 5);
        itemNumber = UnityEngine.Random.Range(0, prize.Count);
        float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
        StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
        //if (SoundsHandler.Instance) SoundsHandler.Instance.PlaySource2Clip(1, 0);
        //gameObject.AddComponent<AudioSource>().clip = SoundsHandler.Instance.source2clips[1];
        //gameObject.GetComponent<AudioSource>().Play();
        //gameObject.GetComponent<AudioSource>().loop = true;
    }
    public void WatchVideoSpinIt()
    {
        FirebaseEvents.instance.LogFirebaseEvent("watch_video_btn_clicked_spinpopup");

#if UNITY_EDITOR
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelSelection")
        {
            OnWatchVideoSuccess();
        }
#else
        FindObjectOfType<AdManager>().ShowRewardedVideo();
#endif


    }
    public void OnWatchVideoSuccess()
    {

        randomTime = UnityEngine.Random.Range(3, 5);
        itemNumber = UnityEngine.Random.Range(0, prize.Count);
        float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
        PlayerPrefs.SetInt("WVSpin", 1);
        StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
        //if (SoundsHandler.Instance) SoundsHandler.Instance.PlaySource2Clip(1, 0);
        //gameObject.AddComponent<AudioSource>().clip = SoundsHandler.Instance.source2clips[1];
        //gameObject.GetComponent<AudioSource>().Play();
        //gameObject.GetComponent<AudioSource>().loop = true;
    }
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;
        //spinwheel events
        FirebaseEvents.instance.LogFirebaseEvent("Spinclicked");
        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        int animationCurveNumber = UnityEngine.Random.Range(0, animationCurves.Count);
        Debug.Log("Animation Curve No. : " + animationCurveNumber);

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime * 4;
            //Debug.Log("timer" + timer + " " + time);
            if (Mathf.RoundToInt(timer) == time - 2 && gameObject.GetComponent<AudioSource>() != null)
                Destroy(gameObject.GetComponent<AudioSource>());

            yield return 0;
        }
        transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;

        Debug.Log("Prize: " + prize[itemNumber]);//use prize[itemNumnber] as per requirement
        //GameManager.AddCoins(prize[itemNumber]);
        try
        {
            StartCoroutine(ShowCoinsAnimation(prize[itemNumber]));
        }catch(Exception e) { }

    }
    [SerializeField] GameObject _coinRef;
    [SerializeField] Transform spawnPoint, coinSpace, reachPoint;
    public IEnumerator ShowCoinsAnimation(int coinsEarned)
    {
        spawnPoint.GetComponentInChildren<UnityEngine.UI.Text>().text = coinsEarned.ToString();
        yield return new WaitForSeconds(2);

        for (int j = 0; j < 5; j++)
        {
            GameObject _coin = Instantiate(_coinRef, spawnPoint.localPosition, Quaternion.identity) as GameObject;
            _coin.transform.SetParent(coinSpace);
            _coin.transform.localScale = Vector3.one;
            _coin.transform.localPosition = spawnPoint.localPosition;
            _coin.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds((j) * .1f);
            _coin.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutBounce);
            _coin.GetComponent<CoinAnimation>().StartCoroutine(
            _coin.GetComponent<CoinAnimation>().Animate(.5f, reachPoint));
        }
        //StartCoroutine(coinsanim((int)(coinsEarned / 10)));
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + coinsEarned);
        GameManager.AddCoins();
        PlayerPrefs.SetInt("FSUsed", 1);
        FreeSpinBtn.SetActive(false);

#if UNITY_EDITOR
        MakeWVAvaialable();
#else
     
    if (_adManager.rewardBasedVideo.IsAdReady())
        {
            MakeWVAvaialable();
        }
        else
        {
            NoFreeSpinsAvailable.SetActive(true);
            WatchVideoSpinBtn.SetActive(false);
        }
#endif
    }
}
