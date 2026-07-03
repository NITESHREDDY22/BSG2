using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
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
    /* public int mandatoryInternetToPlayFromLevel = 40; */
    //[SerializeField] private GameObject closeBtn;

    private const string MandatoryInternetLevelKey = "MandatoryInternetLevel";

    private void Awake()
    {
        Instance= this;
    Debug.Log(
        $"[InternetValidator] Cached Mandatory Level = {mandatoryInternetToPlayFromLevel}");
        CheckInterNetConnectivity();

#if UNITY_EDITOR
       
        //PlayerPrefsX.SetBoolArray("_unlockedlevels" + worldNumb, unlockedLevels);
        //for (int i = 0; i < WorldSelectionHandler.totalLevels.Length; i++)
        //{          
        //    for (int j = 0;j< WorldSelectionHandler.totalLevels[i];j++)
        //    {
        //        WorldSelectionHandler.worldSelected = i;
        //        LevelSelectionHandler.UnlockLevel(j);
        //        LevelSelectionHandler.SetStarsOfLevel(j, i, 3);
        //    }      
        //    Debug.LogError("World Number " + i);
        //}        
#endif
    }
    public static int mandatoryInternetToPlayFromLevel
    {
        get => PlayerPrefs.GetInt(MandatoryInternetLevelKey, 40);
        set
        {
            PlayerPrefs.SetInt(MandatoryInternetLevelKey, value);
            PlayerPrefs.Save();
        }
    }

    private void OnEnable()
    {
        //OnInterNetCheck += EventForInternetCheck;
        AdManager.OnConfigLoaded += OnConfigLoaded;
    }

   

    private void OnDisable()
    {
        //OnInterNetCheck -= EventForInternetCheck;
        AdManager.OnConfigLoaded -= OnConfigLoaded;
    }

    private void OnConfigLoaded(GameConfig config)
    {
        mandatoryInternetToPlayFromLevel = config.InternetMandtoryLevel;
        Debug.Log(
            $"[InternetValidator] Saved Mandatory Level = {mandatoryInternetToPlayFromLevel}");
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
            if (noInterNetPopup && !canProceedToNextLevel())
            {
                noInterNetPopup.SetActive(true);
            }
        }
    }

    IEnumerator CheckInternet(float timer = 0, Action<bool> callBack = null)
    {
        yield return new WaitForSeconds(timer);

        if ( Application.internetReachability != NetworkReachability.NotReachable)
        {
            callBack?.Invoke(Application.internetReachability != NetworkReachability.NotReachable);

            if (!triggerEvent && Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Internet_Connectivity","status", "Connected_succesfully");
                }
                triggerEvent = true;
            }
            // Debug.Log("No Internet - Skipping HTTP Request");
            yield break;
        }
        /*
        using (UnityWebRequest request = UnityWebRequest.Get("https://clients3.google.com/generate_204"))
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
        */

    }

    /* public bool canProceedToNextLevel =>(isInterNetConnected || 
        (!isInterNetConnected && !(GameConstants.targetLevelReached(mandatoryInternetToPlayFromLevel)))); */

    /* public bool canProceedToNextLevel()
    {
        //CheckInterNetConnectivity();
        int LevelNumber = Global.CurrentLeveltoPlay;//GameConstants.getLastUnlcokedLevel;
        bool isConnected = Application.internetReachability != NetworkReachability.NotReachable;
        Debug.Log($"canGotoNextLevel isInterNetConnected:{isInterNetConnected},isConnected?{isConnected}");
        isInterNetConnected = isConnected;
        if (isInterNetConnected)
        {
            return true;
        }
        else
        {
            int worldNumber = WorldSelectionHandler.worldSelected;
            Debug.Log($"canGotoNextLevel worldNumber:{worldNumber},LevelNumber:{LevelNumber}, mandatoryInternetToPlayFromLevel:{mandatoryInternetToPlayFromLevel}");
            if (worldNumber > -1)
            {
                if (mandatoryInternetToPlayFromLevel <= -1)
                {
                    return true;
                }

                if (mandatoryInternetToPlayFromLevel != -1 && LevelNumber < mandatoryInternetToPlayFromLevel)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        return true;

    } */

    private string logHead = "[canProceedToNextLevel]";
    public bool canProceedToNextLevel()
    {
        bool isConnected = Application.internetReachability != NetworkReachability.NotReachable;
        isInterNetConnected = isConnected;

        Debug.Log($"{logHead} Internet Connected: {isInterNetConnected}");

        if (isInterNetConnected)
        {
            Debug.Log($"{logHead} Internet available. Allowing gameplay.");
            return true;
        }

        if (mandatoryInternetToPlayFromLevel <= -1)
        {
            Debug.Log($"{logHead} Mandatory Internet Level = {mandatoryInternetToPlayFromLevel}. Restriction disabled.");
            return true;
        }

        int worldNumber = WorldSelectionHandler.worldSelected;   // 0-based
        int levelNumber = Global.CurrentLeveltoPlay;             // 1-based

        int globalLevel = levelNumber;

        Debug.Log($"{logHead} Calculating Global Level...");
        Debug.Log($"{logHead} Current World: {worldNumber}, Local Level: {levelNumber}");

        for (int i = 0; i < worldNumber; i++)
        {
            globalLevel += WorldSelectionHandler.totalLevels[i];

            Debug.Log($"{logHead} Added World {i} Levels ({WorldSelectionHandler.totalLevels[i]}) -> Running Global Level = {globalLevel}");
        }

        Debug.Log($"{logHead} Final Global Level = {globalLevel}");
        Debug.Log($"{logHead} Mandatory Internet From Global Level = {mandatoryInternetToPlayFromLevel}");

        bool canProceed = globalLevel < mandatoryInternetToPlayFromLevel;

        Debug.Log($"{logHead} Decision = {(canProceed ? "ALLOW (Offline)" : "BLOCK (Internet Required)")}");

        return canProceed;
    }

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

    public void CloseButtonClick()
    {
        //closeBtn.SetActive(false);
        noInterNetPopup.SetActive(false);
    }
}
