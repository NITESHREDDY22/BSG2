using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Ump.Api;
using System.Collections;

public class PrivacyButtonHandler : MonoBehaviour
{
    [Tooltip("The actual button component to show/hide")]
    [SerializeField] private Button privacyButton;

    private void Awake()
    {
        if (privacyButton == null)
            privacyButton = GetComponent<Button>();

        if (privacyButton != null)
        {
            privacyButton.onClick.AddListener(OnPrivacyButtonClick);
        }
    }

    private void OnEnable()
    {
        // Start checking visibility as soon as the menu opens
        StartCoroutine(CheckVisibilityRoutine());
    }

    private IEnumerator CheckVisibilityRoutine()
    {
        // 1. Wait for AdManager to finish its initial GDPR check
        // Added a 10-second safety timeout so it doesn't loop forever if something fails
        float timeout = 10f;
        while ((AdManager._instance == null || !AdManager._instance.IsConsentGatheringFinished) && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        // 2. Check if the legal status requires the button to be visible
        var status = ConsentInformation.PrivacyOptionsRequirementStatus;
        
        if (status == PrivacyOptionsRequirementStatus.Required)
        {
            if(privacyButton != null) privacyButton.gameObject.SetActive(true);
            Debug.Log("<color=green>[PrivacyUI]</color> Privacy Button Enabled (GDPR Region).");
        }
        else
        {
            // IMPORTANT: Only hide the button, don't disable THIS script 
            // so it can re-check next time the menu opens.
            if(privacyButton != null) privacyButton.gameObject.SetActive(false);
            Debug.Log("<color=white>[PrivacyUI]</color> Privacy Button Hidden (Non-GDPR Region).");
        }
    }

    public void OnPrivacyButtonClick()
    {
        Debug.Log("<color=yellow>[PrivacyUI]</color> Privacy Button Clicked.");

        // Use the AdManager wrapper for Firebase to prevent crashes
        if (AdManager._instance != null)
        {
            AdManager._instance.FireBaseActions("Privacy_Settings_Clicked", "UI", "Click");
        }

        // Show the Google UMP Form
        ConsentForm.ShowPrivacyOptionsForm((FormError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"<color=red>[PrivacyUI]</color> Show Form Failed: {error.Message}");
            }
            else
            {
                Debug.Log("<color=green>[PrivacyUI]</color> User updated privacy settings.");
                
                if (AdManager._instance != null)
                {
                    AdManager._instance.FireBaseActions("Privacy_Settings_Updated", "Status", "Success");
                }
            }
        });

        if(FirebaseEvents.instance)
                FirebaseEvents.instance.LogFirebaseEvent("PrivacyBtn_clicked");

        AnalyticsManager.LogDesignEvent("PrivacyBtn_clicked");
    }
}