using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
#if FIREBASEEVENTS
//using Firebase;
//using Firebase.RemoteConfig;
#endif
using UnityEngine.UI;
using TMPro;
//using UnityEngine.Purchasing;

public class FirebaseRemoteConfiguration : MonoBehaviour
{
    public static FirebaseRemoteConfiguration instance;
    //public int[] worldStarcountRq;

#if FIREBASEEVENTS

#if UNITY_IOS || UNITY_ANDROID
        void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    /*

    public void FetchLibrary()
     {
        //Debug.Log("###############Fetching");
        try
        {
            Task FetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            FetchTask.ContinueWith(FetchComplete);
        }
        catch (Exception e)
        { }
    }


    void FetchComplete(System.Threading.Tasks.Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            //Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            //Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            //Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;

        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                //FirebaseRemoteConfig.ActivateFetched();

                //Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                SetRemoteValues();
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        //Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        //Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                //Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }


    public void SetRemoteValues()
    {
            AdManager._instance.GOFAdInterval = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("GOFAdInterval").StringValue);
            AdManager._instance.GOWAdInterval = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("GOWAdInterval").StringValue);
            AdManager._instance.enableBanner = (int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enableBanner").StringValue) == 1) ? true : false ;
            AdManager._instance.enableUnityAds = (int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enableUnityAds").StringValue) == 1) ? true : false;
            AdManager._instance.enableGreedy = (int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enableGreedy").StringValue) == 1) ? true : false;
        worldStarcountRq[1] = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world2StarcountRq").StringValue);
        worldStarcountRq[2] = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world3StarcountRq").StringValue);
        worldStarcountRq[3] = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world4StarcountRq").StringValue);
        worldStarcountRq[4] = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world5StarcountRq").StringValue);

        Global.World2ReqStars = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world2StarcountRq").StringValue);
        Global.World3ReqStars = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world3StarcountRq").StringValue);
        Global.World4ReqStars = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world4StarcountRq").StringValue);
        Global.World5ReqStars = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("world5StarcountRq").StringValue);
    }
*/
#endif
#endif
}
