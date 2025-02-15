using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;

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
            OnInterNetCheck?.Invoke(result);
            callBack?.Invoke(result);

            if (isInterNetConnected)
            {
                CheckInterNetConnectivity(pingInterval);
            }        
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
                    if (!triggerEvent)
                    {
                        if (FirebaseEvents.instance != null)
                        {
                            FirebaseEvents.instance.LogFirebaseEvent("Internet_Connectivity", "Connected_succesfully ");
                        }
                        triggerEvent = true;
                    }
                    callBack?.Invoke(true);

                }
                catch (Exception e)
                {
                    callBack?.Invoke(false);
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
            if (hasFocus )
            {
                CheckNow();

                void CheckNow()
                {
                    CheckInterNetConnectivity(0, (callback) =>
                    {
                        OnInterNetCheck?.Invoke(callback);
                        if(callback==false)
                        {
                            CheckNowWithDelay();
                        }
                    }); 
                }

                void CheckNowWithDelay()
                {
                    CheckInterNetConnectivity(2, (callback) =>
                    {
                        OnInterNetCheck?.Invoke(callback);
                    });
                }

            }
        }
        catch (Exception e)
        { }

    }
    int count = 0;
    void EventForInternetCheck(bool status)
    {
        
        if (status == cacheInternetStatus && count>0)
            return;
     
        SendEvent();

        void SendEvent()
        {
            if (FirebaseEvents.instance!=null)
            {
                string connectString = status ? "InternetConnectedAt_" : "InternetDisConnectedAt_";
                string levelString= "W_" + WorldSelectionHandler.worldSelected+"_L_" + Global.CurrentLeveltoPlay;
                string targetString = string.Concat(connectString, levelString);
                targetString= targetString.Replace(" ", "");
                Debug.LogError(" targetString" + targetString);
                FirebaseEvents.instance.LogFirebaseEvent(targetString, "Status");
                if (count < 1)
                {
                    count++;
                }
                //Debug.LogError("connectString " + connectString+ " WORLD : " + WorldSelectionHandler.worldSelected+ " LEVEL : " + Global.CurrentLeveltoPlay);

            }
        }
        cacheInternetStatus = status;
    }
}
