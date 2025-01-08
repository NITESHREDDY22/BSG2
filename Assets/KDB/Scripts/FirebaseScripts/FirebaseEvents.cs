using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Firebase.Crashlytics;

public class FirebaseEvents : MonoBehaviour
{
    public static FirebaseEvents instance;
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
#if FIREBASEEVENTS
        StartCoroutine(InitFirebase());
#endif
    }

    IEnumerator InitFirebase()
    {
        yield return new WaitForSeconds(1f);
#if FIREBASEEVENTS

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

                this.isFirebaseInit = Firebase.InitResult.Success;
                Debug.Log("###############FirebaseIntialised");
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Firebase.Analytics.FirebaseAnalytics.LogEvent("App open");

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
    }

    /// just call this method to trigger events in firebase()
    public void LogFirebaseEvent(string _log, string paramname = null, string value = null)
    {
#if FIREBASEEVENTS

        try
        {
            if (this.isFirebaseInit == Firebase.InitResult.Success)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(_log, new Firebase.Analytics.Parameter(paramname, value));
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