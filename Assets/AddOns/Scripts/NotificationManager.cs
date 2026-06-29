using System;
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.SceneManagement;

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

        CreateNotificationChannel();

        // Request permission on startup (Android 13+)
        yield return RequestPermission();

        // If permission is granted, schedule the two-stage reminder
        if (AndroidNotificationCenter.UserPermissionToPost == PermissionStatus.Allowed)
        {
            ScheduleDoubleDailyReminder();
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
            RepeatInterval = TimeSpan.FromDays(1), // Repeat every 24 hours
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        };
        AndroidNotificationCenter.SendNotification(note1, channelId);

        // --- NOTIFICATION 2: The 5-Minute Follow-up ---
        var note2 = new AndroidNotification
        {
            Title = "New Levels Need a Champion!",
            Text = "Can you clear every bottle with the perfect shot? Jump back in and continue your bottle-breaking adventure!",
            FireTime = baseFireTime.AddMinutes(5), // 5 minute gap
            RepeatInterval = TimeSpan.FromDays(1), // Repeat every 24 hours
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        };
        AndroidNotificationCenter.SendNotification(note2, channelId);

        Debug.Log("Scheduled 2 daily notifications with a 5-minute gap.");
    }
}