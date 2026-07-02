using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InHouseAdController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject closeButton;
    [SerializeField] private Text countdownText;
    [SerializeField] private string storeURL = "https://play.google.com/store/apps/details?id=YOUR_GAME_ID";

    private Action onAdClosedCallback;
    private string eventPrefix = "NotEnoughCoins";

    private void Awake()
    {
        // Object stays hidden until activated
        //gameObject.SetActive(false);
    }

    // This is the main entry point called by the Popup
    public void OpenAndShow(Action onComplete)
    {
        onAdClosedCallback = onComplete;
        
        // 1. Activate the object
        gameObject.SetActive(true);
        
        // 2. Ensure it's on top of all other UI in this Canvas
        transform.SetAsLastSibling();
        
        // 3. Force Unity to calculate the UI immediately (Fixes 1st click bug)
        Canvas.ForceUpdateCanvases();
        
        LogFirebase(eventPrefix + "_InHouseAd_Shown");
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        ResetUI();
        StartCoroutine(AdTimerRoutine());
    }

    private void ResetUI()
    {
        if (closeButton) closeButton.SetActive(false);
        if (countdownText)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "3";
        }
        // Ensure scale is normal
        transform.localScale = Vector3.one;
    }

    private void LogFirebase(string eventName)
    {
        string fullEvent = eventName;// + "_W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay;
        if (fullEvent.Length > 40) fullEvent = fullEvent.Substring(fullEvent.Length - 40);
        try
        {
            if (FirebaseEvents.IsFirebaseReady)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(fullEvent);
            }
        }
        catch
        {
            Debug.Log("Firebase event logging failed for event: " + fullEvent);
        }
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(fullEvent);
        Debug.Log("[InHouseAd Log]: " + fullEvent);
    }

    private IEnumerator AdTimerRoutine()
    {
        // Wait a frame to ensure components are fully awake
        yield return null; 

        int timeLeft = 3;
        while (timeLeft > 0)
        {
            if(countdownText) countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        if(countdownText) countdownText.gameObject.SetActive(false);
        if(closeButton) closeButton.SetActive(true);
    }

    public void OnAdClicked()
    {
        LogFirebase(eventPrefix + "_InHouseAd_Clkd");
        Application.OpenURL(storeURL);
    }

    public void CloseAd()
    {
        LogFirebase(eventPrefix + "_InHouseAd_Closed");
        gameObject.SetActive(false);
        onAdClosedCallback?.Invoke();
        onAdClosedCallback = null; 
    }
}