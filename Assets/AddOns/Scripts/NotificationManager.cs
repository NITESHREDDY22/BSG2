using System;
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.SceneManagement;
using Firebase.Analytics; // Added Firebase Namespace

public class NotificationManager : MonoBehaviour
{
    // Ensures the logic only runs once per app launch
    private static bool _hasHandledThisSession = false;

    [Header("Settings")]
    public string channelId = "daily_reminders";
    public string smallIcon = "icon_small";
    public string largeIcon = "icon_large";

    void OnEnable()
    {
        // Ensure that the notification logic only runs once per session
        SceneManager.sceneLoaded += SceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log($"SceneLoaded:{arg0.name}");
        if(arg0.name.Contains("MainMenu") && !_hasHandledThisSession)
        {
            StartCoroutine(SetupNotifications());
        }
    }


    private IEnumerator SetupNotifications()
    {
        yield return new WaitForSeconds(1f); // Wait a second to ensure everything is initialized
        if (_hasHandledThisSession) yield break;

        // --- NEW: Check if Game was opened via Notification ---
        var intent = AndroidNotificationCenter.GetLastNotificationIntent();
        if (intent != null)
        {
            LogFirebase("Notification_Opened");
            Debug.Log("App opened via notification tap.");
        }

        LogFirebase("Notification_Setup_Start");

        CreateNotificationChannel();

        // Request permission on startup (Android 13+)
        yield return RequestPermission();

        // If permission is granted, schedule the two-stage reminder
        if (AndroidNotificationCenter.UserPermissionToPost == PermissionStatus.Allowed)
        {
            LogFirebase("Notification_Permission_Allowed");
            ScheduleDoubleDailyReminder();
        }
        else
        {
            LogFirebase("Notification_Permission_Denied");
        }

        _hasHandledThisSession = true;
    }

    private void CreateNotificationChannel()
    {
        Debug.Log("CreateNotificationChannel");
        var channel = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = "Daily Rewards",
            Importance = Importance.Default,
            Description = "Reminders for your daily rewards",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
    }

    private void ScheduleDoubleDailyReminder()
    {
        // 1. Clear all old schedules to start fresh for this session
        AndroidNotificationCenter.CancelAllNotifications();

        // Calculate the base time (e.g., 24 hours from now)
        DateTime baseFireTime = DateTime.Now.AddHours(24);

        // --- NOTIFICATION 1: The First Reminder ---
        #if CHEATS_ON
        var note0 = new AndroidNotification
        {
            Title = "Testing Notification",
            Text = "Your next challenge is waiting! Grab the slingshot, break the bottles, and beat your best score.",
            FireTime = DateTime.Now.AddMinutes(5),
            RepeatInterval = TimeSpan.FromDays(1), // Repeat every 24 hours
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        };
        AndroidNotificationCenter.SendNotification(note0, channelId);
        #endif
        var note1 = new AndroidNotification
        {
            Title = "Ready to Smash Some Bottles?",
            Text = "Your next challenge is waiting! Grab the slingshot, break the bottles, and beat your best score.",
            FireTime = baseFireTime,
            RepeatInterval = TimeSpan.FromHours(Global.notificationInterval), // Repeat every 24 hours
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        };
        AndroidNotificationCenter.SendNotification(note1, channelId);

        // --- NOTIFICATION 2: The 5-Minute Follow-up ---
        var note2 = new AndroidNotification
        {
            Title = "New Levels Need a Champion!",
            Text = "Can you clear every bottle with the perfect shot? Jump back in and continue your bottle-breaking adventure!",
            FireTime = baseFireTime.AddHours(Global.secondNotificationDelay), // 1 hr gap
            RepeatInterval = TimeSpan.FromHours(Global.notificationInterval), // Repeat every 24 hours
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        };
        AndroidNotificationCenter.SendNotification(note2, channelId);

        LogFirebase("Notification_Scheduled_Success");
        Debug.Log("Scheduled 2 daily notifications with a 5-minute gap.");
    }

    // --- Firebase Logging Implementation ---
    private void LogFirebase(string eventName)
    {
        int worldNumber = WorldSelectionHandler.worldSelected;
        int levelNumber = Global.CurrentLeveltoPlay;

        string trimmedEventName = eventName;

        if (trimmedEventName.Length > 40)
        {
            trimmedEventName = trimmedEventName.Substring(trimmedEventName.Length - 40);
        }

        Debug.Log($"IsFirebaseReady: {FirebaseEvents.IsFirebaseReady}");

        try
        {
            if (FirebaseEvents.IsFirebaseReady)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    trimmedEventName
                );
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(
                $"Firebase event logging failed. Event={trimmedEventName}, World={worldNumber}, Level={levelNumber}\n{e}");
        }

        Debug.Log(
            $"[Firebase Log]: {trimmedEventName} | world={worldNumber} | level={levelNumber}");
    }
}