using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InternetValidator : MonoBehaviour
{
    public static InternetValidator Instance;
    [SerializeField] AdManager adManager;
    public bool isInterNetConnected;
    private bool cacheInternetStatus=false; 
    public GameObject noInterNetPopup;
    int pingInterval = 60;
    private bool triggerEvent;
    public Action<bool> OnInterNetCheck;
    private string cachedLevel;
    public int mandatoryInternetToPlayFromLevel = 40;
    private void Awake()
    {
        Instance= this;
        CheckInterNetConnectivity();
    }

    private void OnEnable()
    {
        OnInterNetCheck += EventForInternetCheck;
        AdManager.OnConfigLoaded += OnConfigLoaded;
    }

   

    private void OnDisable()
    {
        OnInterNetCheck -= EventForInternetCheck;
        AdManager.OnConfigLoaded -= OnConfigLoaded;


    }

    private void OnConfigLoaded(GameConfig config)
    {
        mandatoryInternetToPlayFromLevel = config.InternetMandtoryLevel;
    }

    public void CheckInterNetConnectivity(int timer = 0, Action<bool> callBack = null)
    {
        StopCoroutine(CheckInternet());
        StartCoroutine(CheckInternet(timer, (result) =>
        {
            isInterNetConnected = result;
            callBack?.Invoke(result);
            if (isInterNetConnected)
            {
                CheckInterNetConnectivity(pingInterval);
            }
            else
            {
                Debug.Log("Show error popup");
            }
            OnInterNetCheck?.Invoke(result);
            GameConstants.InternetConnected = isInterNetConnected;
        }));
    }

    public void ForceCheckInternet()
    {
        CheckInterNetConnectivity(0, (result) =>
        {
            CheckNoInterNetPopup();
        });
    }

    public void CheckNoInterNetPopup()
    {
        if (isInterNetConnected)
        {
            if (noInterNetPopup)
            {
                noInterNetPopup.SetActive(false);
            }
        }
        else
        {
            if (noInterNetPopup && !canProceedToNextLevel)
            {
                noInterNetPopup.SetActive(true);
            }
        }
    }

    IEnumerator CheckInternet(float timer = 0, Action<bool> callBack = null)
    {
        yield return new WaitForSeconds(timer);

        if (triggerEvent && Application.internetReachability != NetworkReachability.NotReachable)
        {
            callBack?.Invoke(Application.internetReachability != NetworkReachability.NotReachable);
            // Debug.Log("No Internet - Skipping HTTP Request");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get("http://clients3.google.com/generate_204"))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    callBack?.Invoke(true);
                    if (!triggerEvent)
                    {
                        if (FirebaseEvents.instance != null)
                        {
                            FirebaseEvents.instance.LogFirebaseEvent("Inetnet Connectivity", "Connected succesfully ");
                        }
                        triggerEvent = true;
                    }

                }
                catch (Exception e)
                {

                }
            }
            else
            {
                callBack?.Invoke(false);
            }
        }

    }

    public bool canProceedToNextLevel =>(isInterNetConnected || 
        (!isInterNetConnected && !(GameConstants.targetLevelReached(mandatoryInternetToPlayFromLevel))));

    void OnApplicationFocus(bool hasFocus)
    {
        try
        {
            if (hasFocus )//&& adManager.CheckMandatoryInterNet)
            {
                CheckInterNetConnectivity(0, (callback) =>
                {
                    OnInterNetCheck?.Invoke(callback);                    
                });
            }
        }
        catch (Exception e)
        { }

    }

    void EventForInternetCheck(bool status)
    {
        
        if (status == cacheInternetStatus)
            return;

        string currentLevel = string.Concat(WorldSelectionHandler.worldSelected, Global.CurrentLeveltoPlay);
        Debug.LogError("Current Level " + currentLevel);
         

        SendEvent();

        void SendEvent()
        {
            if (FirebaseEvents.instance)
            {
                string connectString = status ? "Inetnet Connected at" : "Inetnet DisConnected at";
                FirebaseEvents.instance.LogFirebaseEvent(connectString, "WORLD : " + WorldSelectionHandler.worldSelected, "LEVEL : " + Global.CurrentLeveltoPlay);
            }
        }
        cacheInternetStatus = status;
    }
}
