using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;
using System.Collections.Generic;
using Assets.SimpleLocalization;

public class GameCompletionPopup : MonoBehaviour
{
    // Singleton Instance
    public static GameCompletionPopup Instance { get; private set; }

    [Header("Game Configuration")]
    [SerializeField] private int totalLevels = 50;
    [SerializeField] private int starsPerLevel = 5;
    [SerializeField] private float animationDuration = 2.0f;

    [Header("UI References")]
    [SerializeField] public GameObject popupPanel; 
    [SerializeField] private Image fillImage;        // Must be Image Type: Filled
    [SerializeField] private TextMeshProUGUI progressText; // Shows "%"
    [SerializeField] private TextMeshProUGUI titleText; // Shows "%"
    [SerializeField] private Text bodyTxt; // Shows "%"
    [SerializeField]Button menuBtn,masteryBtn,backBtn;

    public int maxPossibleStars;

    public SystemLanguage debugLanguage;

    void OnEnable()
    {
        menuBtn.onClick.AddListener(GoBack);
        menuBtn.onClick.AddListener(Close);
        masteryBtn.onClick.AddListener(Mastery);
        masteryBtn.onClick.AddListener(Close);
    }
    void OnDisable()
    {
        menuBtn.onClick.RemoveAllListeners();
        masteryBtn.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        // 1. Singleton Logic
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[GameCompletionPopup] Multiple instances detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 2. Initialize Logic
        maxPossibleStars = totalLevels * starsPerLevel;
        Debug.Log($"[GameCompletionPopup] Initialized. Max stars calculated: {maxPossibleStars}");
        
        if(popupPanel != null) popupPanel.SetActive(false);
    }

    /// <summary>
    /// Call this from any script: GameCompletionPopup.Instance.Show(175);
    /// </summary>
    public void Show(int totalStarsAchieved)
    {
        // Validation Log
        if (fillImage == null || progressText == null)
        {
            Debug.LogError("[GameCompletionPopup] UI References missing! Please assign Fill Image and Text in Inspector.");
            return;
        }

        float targetFillNormalized = Mathf.Clamp01((float)totalStarsAchieved / maxPossibleStars);
        
        Debug.Log($"[GameCompletionPopup] Opening Popup. Achieved: {totalStarsAchieved}/{maxPossibleStars} | Target Fill: {targetFillNormalized * 100}%");
        PlayerPrefs.SetString(GameConstants.allLevelsCompleteKey,"ALC_");
        PlayerPrefs.Save();
        popupPanel.SetActive(true);
        StopAllCoroutines(); 
        StartCoroutine(AnimateCompletion(targetFillNormalized));
        titleText.text = GetStarHeadText(totalStarsAchieved,maxPossibleStars);//$"{totalStarsAchieved}/{maxPossibleStars} STARS";
        bodyTxt.text = GetBodyText(maxPossibleStars,totalStarsAchieved);//$"Collect {maxPossibleStars-totalStarsAchieved} more Stars to become MASTER";

        bool worldSelection = SceneManager.GetActiveScene().name.Contains("MainMenu");
        menuBtn.gameObject.SetActive(!worldSelection);
        masteryBtn.gameObject.SetActive(!worldSelection);
        backBtn.gameObject.SetActive(worldSelection);
    }

    private IEnumerator AnimateCompletion(float targetFill)
    {
        fillImage.fillAmount = 0;
        progressText.text = "0%";

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            // Smooth Ease-Out formula
            float easedT = 1f - Mathf.Pow(1f - t, 3f); 

            // Unity Fill Image amount (0.0 to 1.0)
            float currentFill = Mathf.Lerp(0, targetFill, easedT);
            fillImage.fillAmount = currentFill;

            // Percentage calculation for Text
            float currentPercent = currentFill * 100f;
            progressText.text = $"{Mathf.RoundToInt(currentPercent)}%";

            yield return null;
        }

        // Final Snap
        fillImage.fillAmount = targetFill;
        progressText.text = $"{Mathf.RoundToInt(targetFill * 100f)}%";
        
        Debug.Log($"[GameCompletionPopup] Animation Finished at {progressText.text} completion.");
    }

    public void Close()
    {
        Debug.Log("[GameCompletionPopup] Closing Popup.");
        popupPanel.SetActive(false);
        PlayerPrefs.SetInt("GameComplete_Unlocked", 1);

        // 2. Save the final progress percentage (0.0 to 1.0)
        float finalProgress = fillImage.fillAmount;
        PlayerPrefs.SetFloat("Global_Progress_Value", finalProgress);

        // 3. Force save to disk
        PlayerPrefs.Save();

        Debug.Log($"[GameCompletionPopup] Progress Saved: {finalProgress * 100}% | Status: Unlocked");
    }

    public void GoBack()
    {
        Mastery();
    }
    public void Mastery()
    {
        if(SceneManager.GetActiveScene().name.Contains("LevelSelection"))
        {
            Debug.Log("[GameCompletionPopup] Already in LevelSelection. No need to navigate.");
            return;
        }
        int soundProp = PlayerPrefs.GetInt ("SOUND");
		/* if (soundProp == 1 && AudioManager.Instance != null) 			
			AudioManager.Instance.ClickSound.Play ();
 */
		/* Global.exitLevel = Global.currentLevel;
		Global.currentLevel = 1; */
        /* Global.fromLevelSelection = true; */
        SceneManager.LoadScene("LevelSelection");
        if(FirebaseEvents.instance)
            FirebaseEvents.instance.LogFirebaseEvent("MasteryBtn_clicked_GameCompletionPopup");
    
    }
    string GetStarHeadText(int totalStarsAchieved, int maxPossibleStars)
{
    string progressText = $"{totalStarsAchieved}/{maxPossibleStars} Stars";
    return progressText;
    if (LocalizationManager.Language == LocalizationManager.French)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Étoiles";
    }
    else if (LocalizationManager.Language == LocalizationManager.Arabic)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} نجمة";
    }
    else if (LocalizationManager.Language == LocalizationManager.Dutch)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Sterren";
    }
    else if (LocalizationManager.Language == LocalizationManager.German)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Sterne";
    }
    else if (LocalizationManager.Language == LocalizationManager.Italian)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Stelle";
    }
    else if (LocalizationManager.Language == LocalizationManager.Japanese)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} スター";
    }
    else if (LocalizationManager.Language == LocalizationManager.Polish)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Gwiazdek";
    }
    else if (LocalizationManager.Language == LocalizationManager.Portuguese)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Estrelas";
    }
    else if (LocalizationManager.Language == LocalizationManager.Russian)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Звёзд";
    }
    else if (LocalizationManager.Language == LocalizationManager.Spanish)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Estrellas";
    }
    else if (LocalizationManager.Language == LocalizationManager.Turkish)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Yıldız";
    }
    else if (LocalizationManager.Language == LocalizationManager.Chinese)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} 颗星";
    }
    else if (LocalizationManager.Language == LocalizationManager.Vietnamese)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Ngôi sao";
    }
    else if (LocalizationManager.Language == LocalizationManager.Korean)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} 별";
    }
    else if (LocalizationManager.Language == LocalizationManager.Indonesian)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Bintang";
    }
    else if (LocalizationManager.Language == LocalizationManager.Thai)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} ดาว";
    }
    else if (LocalizationManager.Language == LocalizationManager.Hindi)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} सितारे";
    }
    else if (LocalizationManager.Language == LocalizationManager.Ukrainian)
    {
        progressText = $"{totalStarsAchieved}/{maxPossibleStars} Зірок";
    }

    return progressText;
}


    string GetBodyText(int maxPossibleStars, int totalStarsAchieved)
{
    int remainingStars = maxPossibleStars - totalStarsAchieved;

    string worldLockDescription =
        $"Collect {remainingStars} more Stars to become MASTER";

    if (LocalizationManager.Language == LocalizationManager.French)
    {
        worldLockDescription =
            $"Collectez encore {remainingStars} étoiles pour devenir MAÎTRE";
    }
    else if (LocalizationManager.Language == LocalizationManager.Arabic)
    {
        worldLockDescription =
            $"اجمع {remainingStars} نجمة إضافية لتصبح المحترف";
    }
    else if (LocalizationManager.Language == LocalizationManager.Dutch)
    {
        worldLockDescription =
            $"Verzamel nog {remainingStars} sterren om MEESTER te worden";
    }
    else if (LocalizationManager.Language == LocalizationManager.German)
    {
        worldLockDescription =
            $"Sammle noch {remainingStars} Sterne, um MEISTER zu werden";
    }
    else if (LocalizationManager.Language == LocalizationManager.Italian)
    {
        worldLockDescription =
            $"Raccogli ancora {remainingStars} stelle per diventare MAESTRO";
    }
    else if (LocalizationManager.Language == LocalizationManager.Japanese)
    {
        worldLockDescription =
            $"あと{remainingStars}個のスターを集めてマスターになろう";
    }
    else if (LocalizationManager.Language == LocalizationManager.Polish)
    {
        worldLockDescription =
            $"Zbierz jeszcze {remainingStars} gwiazdek, aby zostać MISTRZEM";
    }
    else if (LocalizationManager.Language == LocalizationManager.Portuguese)
    {
        worldLockDescription =
            $"Colete mais {remainingStars} estrelas para se tornar MESTRE";
    }
    else if (LocalizationManager.Language == LocalizationManager.Russian)
    {
        worldLockDescription =
            $"Соберите ещё {remainingStars} звёзд, чтобы стать МАСТЕРОМ";
    }
    else if (LocalizationManager.Language == LocalizationManager.Spanish)
    {
        worldLockDescription =
            $"Recoge {remainingStars} estrellas más para convertirte en MAESTRO";
    }
    else if (LocalizationManager.Language == LocalizationManager.Turkish)
    {
        worldLockDescription =
            $"USTA olmak için {remainingStars} yıldız daha topla";
    }
    else if (LocalizationManager.Language == LocalizationManager.Chinese)
    {
        worldLockDescription =
            $"再收集 {remainingStars} 颗星星即可成为大师";
    }
    else if (LocalizationManager.Language == LocalizationManager.Vietnamese)
    {
        worldLockDescription =
            $"Thu thập thêm {remainingStars} ngôi sao để trở thành BẬC THẦY";
    }
    else if (LocalizationManager.Language == LocalizationManager.Korean)
    {
        worldLockDescription =
            $"{remainingStars}개의 별을 더 모아 마스터가 되세요";
    }
    else if (LocalizationManager.Language == LocalizationManager.Indonesian)
    {
        worldLockDescription =
            $"Kumpulkan {remainingStars} bintang lagi untuk menjadi MASTER";
    }
    else if (LocalizationManager.Language == LocalizationManager.Thai)
    {
        worldLockDescription =
            $"เก็บดาวเพิ่มอีก {remainingStars} ดวงเพื่อเป็น MASTER";
    }
    else if (LocalizationManager.Language == LocalizationManager.Hindi)
    {
        worldLockDescription =
            $"मास्टर बनने के लिए {remainingStars} और सितारे एकत्र करें";
    }
    else if (LocalizationManager.Language == LocalizationManager.Ukrainian)
    {
        worldLockDescription =
            $"Зберіть ще {remainingStars} зірок, щоб стати МАЙСТРОМ";
    }

    return worldLockDescription;
}

}