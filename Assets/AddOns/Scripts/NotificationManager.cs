using System;
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.SceneManagement;
using Firebase.Analytics;

public class NotificationManager : MonoBehaviour
{
    private static bool _hasHandledThisSession = false;

    [Header("Settings")]
    public string channelId = "daily_reminders";
    public string smallIcon = "icon_small";
    public string largeIcon = "icon_large";

    private int DaysToSchedule => Global.notificationDaysCount > 0 ? Global.notificationDaysCount : 7;

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // We still run setup on MainMenu once to check if app was opened via notification 
        // and to request permissions for the first time.
        if (arg0.name.Contains("MainMenu") && !_hasHandledThisSession)
        {
            StartCoroutine(InitialSetup());
        }
    }

    private IEnumerator InitialSetup()
    {
        yield return new WaitForSeconds(1f);
        if (_hasHandledThisSession) yield break;

        // Check if opened via notification
        var intent = AndroidNotificationCenter.GetLastNotificationIntent();
        if (intent != null) LogFirebase("Notification_Opened");

        CreateNotificationChannel();

        // Request permission (needed for Android 13+)
        yield return RequestPermission();

        _hasHandledThisSession = true;
    }

    // --- KEY ADDITION: Handle Backgrounding ---
    private void OnApplicationPause(bool pauseStatus)
    {
        // If pauseStatus is true, the app is going to the background (User quit or minimized)
        if (pauseStatus)
        {
            Debug.Log("App pausing/minimizing: Scheduling updated notifications.");
            ScheduleProgressNotifications();
        }
    }

    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = "Daily Progress Reminders",
            Importance = Importance.Default,
            Description = "Bottle breaking progress updates",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
    }

    // Define these at the top of your class as private fields
    private readonly string[] titles = {
    "The Slingshot is Calling! 🎯",
    "Ready for some Shattered Glass? 💥",
    "Target Practice Awaits! 🏹",
    "The Bottles are Mocking You! 🔥",
    "Can You Clear the Map? 🗺️",
    "Bullseye Logic! 🧠",
    "New Records to Break! 🏆"
};

    private readonly string[] messages = {
    "You've smashed {0} levels already. {1} more stand in your way. Grab the slingshot!",
    "The sound of breaking glass is waiting! {0} levels done, {1} left to conquer.",
    "You’re on fire! 🔥 {0} levels mastered. Can you finish the remaining {1}?",
    "Precision is everything. {1} challenges are waiting for your perfect shot.",
    "Don't let the bottles win! You've cleared {0} levels—finish the other {1} now!",
    "Your aim was legendary last time. Come back and beat the {1} remaining levels!",
    "Strategy + Slingshot = Victory. {0} levels down, {1} more to go!"
};

    private void ScheduleProgressNotifications()
    {
        if (AndroidNotificationCenter.UserPermissionToPost != PermissionStatus.Allowed) return;

        AndroidNotificationCenter.CancelAllNotifications();

        // 1. Calculate Progress
        int totalLevelsInGame = 0;
        int totalUnlockedLevels = 0;
        for (int i = 0; i < WorldSelectionHandler.totalLevels.Length; i++)
        {
            totalLevelsInGame += WorldSelectionHandler.totalLevels[i];
            totalUnlockedLevels += LevelSelectionHandler.LevelsUnlocked(i);
        }
        int remaining = totalLevelsInGame - totalUnlockedLevels;

        // 2. Handle Game Completion Case
        if (remaining <= 0)
        {
            ScheduleCompletionNotifications(totalUnlockedLevels);
            return;
        }

        // --- TEST NOTIFICATION (Random Message) ---
#if CHEATS_ON
        SendRandomTestNotification(totalUnlockedLevels, remaining);
#endif

        // 3. Schedule for the next N days (Sequential Rotation)
        for (int day = 1; day <= DaysToSchedule; day++)
        {
            // For the real schedule, we use sequential rotation so they see different ones each day
            int index = (day - 1) % titles.Length;
            string finalTitle = titles[index];
            string finalBody = string.Format(messages[index], totalUnlockedLevels, remaining);

            DateTime fireTime = DateTime.Now.AddDays(day);
            SendNotification(finalTitle, finalBody, fireTime);
        }

        LogFirebase("Notifications_Scheduled_Success");
    }

#if CHEATS_ON
    private void SendRandomTestNotification(int finished, int remaining)
    {
        // Pick a completely random index from the list
        int randomIndex = UnityEngine.Random.Range(0, titles.Length);

        string testTitle = "[TEST] " + titles[randomIndex];
        string testBody = string.Format(messages[randomIndex], finished, remaining);

        // Schedule for 5 minutes from now
        SendNotification(testTitle, testBody, DateTime.Now.AddMinutes(5));

        Debug.Log($"<color=yellow>[Notification Test]</color> Picked index {randomIndex}: {testTitle}");
    }
#endif

    // Helper method to reduce code duplication
    private void SendNotification(string title, string body, DateTime fireTime)
    {
        var note = new AndroidNotification
        {
            Title = title,
            Text = body,
            FireTime = fireTime,
            SmallIcon = smallIcon,
            LargeIcon = largeIcon,
            ShowTimestamp = true
        };
        AndroidNotificationCenter.SendNotification(note, channelId);
    }
    private void ScheduleCompletionNotifications(int total)
    {
        string title = "The World is Quiet... 🤫";
        string body = $"You've smashed all {total} levels! Can you go back and get 3 stars on every single one?";

        for (int day = 1; day <= DaysToSchedule; day++)
        {
            AndroidNotificationCenter.SendNotification(new AndroidNotification
            {
                Title = title,
                Text = body,
                FireTime = DateTime.Now.AddDays(day),
                SmallIcon = smallIcon,
                LargeIcon = largeIcon
            }, channelId);
        }
    }

    private void LogFirebase(string eventName)
    {
        try
        {
            if (FirebaseEvents.IsFirebaseReady)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            }
        }
        catch (System.Exception) { /* Fail silently */ }
    }
}