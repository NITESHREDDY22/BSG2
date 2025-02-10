using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyLoginHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly string FirstTimeLogin = "FirstTimeLogin";
    private readonly string ImpressionKey = "AdImpressionsOnDay ";
    private readonly string ClicksKey = "AdClicksOnDay ";

    void Start()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(FirstTimeLogin, string.Empty)))
        {
            PlayerPrefs.SetString(FirstTimeLogin, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));        
        }        
    }

    private void OnEnable()
    {
        AdMobNetworkHandler.OnAdClickedCallBack += OnAdClickedCallBack;
        AdMobNetworkHandler.OnAdImpressionCallBack += OnAdImpressionCallBack;
        LevelPlayNetworkHandler.OnAdClickedCallBack += OnAdClickedCallBack;
    }

    private void OnDisable()
    {
        AdMobNetworkHandler.OnAdClickedCallBack -= OnAdClickedCallBack;
        AdMobNetworkHandler.OnAdImpressionCallBack -= OnAdImpressionCallBack;
        LevelPlayNetworkHandler.OnAdClickedCallBack -= OnAdClickedCallBack;
    }

    int GetCurrentDay()
    {
       
        DateTime currentTime = DateTime.UtcNow;       
        string cachedTime= PlayerPrefs.GetString(FirstTimeLogin);

       // Debug.LogError("currentTime : " + currentTime);
       // Debug.LogError("cachedTime : " + cachedTime );       

        DateTime parsedDateTime;
        bool success = DateTime.TryParseExact(
            cachedTime,
            "yyyy-MM-dd HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out parsedDateTime
        );

        if (success)
        {
           // Debug.LogError("Successfully Parsed: " + parsedDateTime);
            double getHoursDifference=(currentTime-parsedDateTime).TotalDays;
            //Debug.LogError("TotalDays :: " + getHoursDifference);
            return (int)getHoursDifference;
        }
        else
        {
            //Debug.LogError("Invalid Date-Time Format!");
            return -1;
        }

    }

    private void OnAdClickedCallBack(string arg1, string arg2)
    {
        SendEvent(ClicksKey, arg1, arg2);
    }

    private void OnAdImpressionCallBack(string arg1, string arg2)
    {
        SendEvent(ImpressionKey, arg1, arg2);
    }

    void SendEvent(string eventKey,string adtype,string newtorktype)
    {
        int dayCount = GetCurrentDay();

        if (dayCount < 0 || dayCount > 6)
            return;

        dayCount++;
        string key=string.Concat(eventKey,dayCount);

        if (FirebaseEvents.instance != null)
        {
            FirebaseEvents.instance.LogFirebaseEvent(key, adtype, newtorktype);
        }
    }
}
