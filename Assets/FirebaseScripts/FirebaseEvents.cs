using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Firebase.Crashlytics;

public class FirebaseEvents : MonoBehaviour
{
    public static FirebaseEvents instance;
    public static bool IsFirebaseReady
    {
        get
        {
            return firebaseInitDone;
        }
    }

    private static bool firebaseInitDone = false;
    public static bool FirebaseInitDone
    {
        get
        {
            Debug.Log($"[Firebase] FirebaseInitDone getting: {firebaseInitDone}");
            return firebaseInitDone;
        }
        set
        {
            firebaseInitDone = value;
            Debug.Log($"[Firebase] FirebaseInitDone set to: {value}");
        }
    }
#if UNITY_IOS || UNITY_ANDROID
    public Firebase.InitResult isFirebaseInit;
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

        isFirebaseInit = (Firebase.InitResult)(-1); 
#if FIREBASEEVENTS
        StartCoroutine(InitFirebase());
#endif
    }

    /* IEnumerator InitFirebase()
    {
        yield return new WaitForSeconds(1f);
#if FIREBASEEVENTS

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                IsFirebaseReady = true;
                this.isFirebaseInit = Firebase.InitResult.Success;
                Debug.Log("###############FirebaseIntialised");
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Firebase.Analytics.FirebaseAnalytics.LogEvent("App open");
                try
                {
                    LogFirebaseEvent("Splash_screen");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[Firebase] Error logging event: {e.Message}");
                }
                // Crashlytics.IsCrashlyticsCollectionEnabled = true
                //FirebaseRemoteConfiguration.instance.FetchLibrary();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    //Firebase Unity SDK is not safe to use here.
                }
        });
#endif
    } */

    IEnumerator InitFirebase()
    {
        Debug.Log("[FIREBASE EVENTS] InitFirebase START");

        yield return new WaitUntil(() => FirebaseInitDone);

        Debug.Log("[FIREBASE EVENTS] FirebaseInitDone received");

        isFirebaseInit = Firebase.InitResult.Success;

        Debug.Log("[FIREBASE EVENTS] Analytics Ready");
    }


    /// just call this method to trigger events in firebase()
    /* public void LogFirebaseEvent(string _log, string paramname = null, string value = null)
    {
#if FIREBASEEVENTS

        try
        {
            Debug.Log($"LogFirebaseEvent called with event: {_log}, paramname: {paramname}, value: {value}");
            if (this.isFirebaseInit == Firebase.InitResult.Success)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(_log, new Firebase.Analytics.Parameter(paramname, value));
                Debug.Log($"<color=black>FirebaseEvent = {_log}, {paramname} = {value}</color>");
            }
            else
            {
                ///Debug.Log("initresultFailed");
            }
        }
        catch (NullReferenceException e)
        {
            //Crashlytics.LogException(e);
        }
#endif
    } */

    public void LogFirebaseEvent(string _log, string paramname = null, string value = null)
{
#if FIREBASEEVENTS
        if (!FirebaseInitDone)
        {
            // Debug.Log($"[Firebase] Call to '{_log}' blocked: Dependencies not ready.");
            return;
        }
        try
    {
        if (this.isFirebaseInit == Firebase.InitResult.Success)
        {
            if (string.IsNullOrEmpty(paramname))
            {
                Debug.Log(
                $"[Firebase] Sending event without parameters: '{_log}'");
                Firebase.Analytics.FirebaseAnalytics.LogEvent(_log);
            }
            else
            {
                 Debug.Log(
                $"[Firebase] Sending event with parameter | " +
                $"Event='{_log}' | {paramname}='{value}'");
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    _log,
                    new Firebase.Analytics.Parameter(paramname, value ?? "")
                );
                Debug.Log(
                $"[Firebase] SUCCESS - Event Sent: '{_log}' | " +
                $"{paramname}='{value}'");
            }
        }
    }
    catch (Exception e)
    {
        Debug.LogError(
            $"[Firebase] FAILED to send event '{_log}'.\n" +
            $"ParamName='{paramname}'\n" +
            $"Value='{value}'\n" +
            $"Exception: {e}");
    }
#endif
}

    //    public void LogMulFirebaseEvent(string _log, Firebase.Analytics.Parameter[] _parameters)
    //    {
    //#if FIREBASEEVENTS

    //        try
    //        {
    //            if (this.isFirebaseInit == Firebase.InitResult.Success)
    //            {
    //                Firebase.Analytics.FirebaseAnalytics.LogEvent(_log, _parameters);
    //            }
    //        }
    //        catch (NullReferenceException e)
    //        {
    //            ///Debug.Log(e, this);
    //        }
    //#endif
    //    }

//    public void SetUserProperty(string propertyName, string propertyValue)
//    {
//#if FIREBASEEVENTS

//        try
//        {
//            if (this.isFirebaseInit == Firebase.InitResult.Success)
//            {
//                Firebase.Analytics.FirebaseAnalytics.SetUserProperty(propertyName, propertyValue);
//            }
//        }
//        catch (NullReferenceException e)
//        {
//            //Debug.Log(e, this);
//        }
//#endif
//    }
#endif
}

/////IMPORT FIREBASE AND DEFINE FIREBASEEVENTS SYMBOL BEFORE YOU START ADDING THIS COMPONENT TO ANY GAMEOBJECT