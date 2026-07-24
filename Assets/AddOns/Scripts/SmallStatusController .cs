using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SmallStatusController : MonoBehaviour
{
    public GameCompletionPopup gameCompletionPopup;
    [Header("UI References")]
    [SerializeField] private GameObject visualPanel; // The actual UI container
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField]Button statusBtn;
    

    private void Start()
    {
        // Every time the scene starts or script loads, check the status
        TryInitializeUI(false);
        
        statusBtn.onClick.AddListener(MaximizePopup);
    }

    void OnEnable()
    {
        //MainMenuController.onWorldSelectionShown += Refresh;
        WorldSelectionHandler.worldSelectionShown += Refresh;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == "LevelSelection")
        {
            //TryInitializeUI(true);
        }
        else
        {
            TryInitializeUI(false);
        }
    }

    void OnDisable()
    {
        //MainMenuController.onWorldSelectionShown -= Refresh;
        WorldSelectionHandler.worldSelectionShown -= Refresh;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Checks PlayerPrefs to see if the status should be visible
    /// </summary>
    public void TryInitializeUI(bool isWorldSelect)
    {
        // Check if the key exists and is set to 1 (Unlocked)
        bool isUnlocked = PlayerPrefs.GetInt("GameComplete_Unlocked", 0) == 1;
        if (!isWorldSelect)
        {
            visualPanel.SetActive(false);
            return;
        }

        if (isUnlocked)
        {
            int totalStars = Global.TotalStarsAchivedWorld1+Global.TotalStarsAchivedWorld2+Global.TotalStarsAchivedWorld3+Global.TotalStarsAchivedWorld4+Global.TotalStarsAchivedWorld5;
            float targetFillNormalized = Mathf.Clamp01((float)totalStars / gameCompletionPopup.maxPossibleStars);
            // Retrieve the saved progress
            //float savedProgress = PlayerPrefs.GetFloat("Global_Progress_Value", 0f);
            float savedProgress = targetFillNormalized;
            
            // Populate and Show
            fillImage.fillAmount = savedProgress;
            percentageText.text = $"{Mathf.RoundToInt(savedProgress * 100f)}%";
            
            visualPanel.SetActive(true);
            Debug.Log("[SmallStatus] Saved data found. UI is now Visible.");
        }
        else
        {
            // Key not found or game not finished yet
            visualPanel.SetActive(false);
            Debug.Log("[SmallStatus] No completion data found. UI remains Hidden.");
        }
    }

    // Optional: Call this if you want to update the small UI while the game is running 
    // without reloading the scene.
    public void Refresh(bool status)
    {
        Debug.Log($"Refresh Small Status{status}");
        TryInitializeUI(status);
    }

    public void MaximizePopup()
    {
        int totalStars = Global.TotalStarsAchivedWorld1+Global.TotalStarsAchivedWorld2+Global.TotalStarsAchivedWorld3+Global.TotalStarsAchivedWorld4+Global.TotalStarsAchivedWorld5;
            Debug.Log($"[Show GameCompletionPopup]totalStars:{totalStars},getNatureStars:{Global.TotalStarsAchivedWorld1},getDesertStars:{Global.TotalStarsAchivedWorld2},getSnowStars:{Global.TotalStarsAchivedWorld3},getJungleStars:{Global.TotalStarsAchivedWorld4},getArcticStars:{Global.TotalStarsAchivedWorld5}");
        gameCompletionPopup.Show(totalStars);
        if(!Global.limitedEvents)
            Firebase.Analytics.FirebaseAnalytics.LogEvent("GameComplete_Show_" + "totalStars_" + totalStars);
    }
}