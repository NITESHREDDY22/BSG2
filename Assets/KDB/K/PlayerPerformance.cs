using UnityEngine;

public class PlayerPerformance : MonoBehaviour
{
    public static PlayerPerformance Instance { get; private set; }

    public enum CampaignRatingType
    {
        StandardCampaign,   // 5 Levels (LIVE)
        ExtendedCampaign    // 10 Levels (NEW)
    }

    [Header("Campaign Configuration")]
    public int standardCampaignLevels = 5;
    public int extendedCampaignLevels = 10;

    [Header("Attempts Tracking")]
    public int standardCampaignAttempts = 0;
    public int extendedCampaignAttempts = 0;

    [SerializeField] private int standardCampaignRating = 0;
    [SerializeField] private int extendedCampaignRating = 0;

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PlayerPerformance] Singleton initialized (persistent).");
        }
        else
        {
            Debug.LogWarning("[PlayerPerformance] Duplicate instance destroyed.");
            Destroy(gameObject);
        }
    }

    #endregion

    #region Attempt Tracking

    public void RecordAttempt(CampaignRatingType campaignType)
    {
        if (campaignType == CampaignRatingType.StandardCampaign)
        {
            standardCampaignAttempts++;
            Debug.Log($"[PlayerPerformance][Attempt] Standard Campaign | Total Attempts: {standardCampaignAttempts}");
        }
        else
        {
            extendedCampaignAttempts++;
            Debug.Log($"[PlayerPerformance][Attempt] Extended Campaign | Total Attempts: {extendedCampaignAttempts}");
        }
    }

    #endregion

    #region Level Completion

    public void CompleteLevel(CampaignRatingType campaignType)
    {
        Debug.Log($"[PlayerPerformance][Level Complete] Campaign: {campaignType}, LevelIndex: {Global.CurrentLeveltoPlay}");

        if (campaignType == CampaignRatingType.StandardCampaign)
        {
            if (Global.CurrentLeveltoPlay >= standardCampaignLevels - 1)
            {
                Debug.Log("[PlayerPerformance] Standard Campaign finished.");
                FinishStandardCampaign();
            }
        }
        else
        {
            if (Global.CurrentLeveltoPlay >= extendedCampaignLevels - 1)
            {
                Debug.Log("[PlayerPerformance] Extended Campaign finished.");
                FinishExtendedCampaign();
            }
        }
    }

    #endregion

    #region Rating Logic

    public int GetStarRating(CampaignRatingType campaignType)
    {
        int attempts;
        int totalLevels;

        if (campaignType == CampaignRatingType.StandardCampaign)
        {
            attempts = standardCampaignAttempts;
            totalLevels = standardCampaignLevels;
        }
        else
        {
            attempts = extendedCampaignAttempts;
            totalLevels = extendedCampaignLevels;
        }

        int stars =
            attempts <= totalLevels + 2 ? 3 :
            attempts <= totalLevels * 2 ? 2 : 1;

        Debug.Log($"[PlayerPerformance][Rating Calc] {campaignType} | Attempts: {attempts}, Stars: {stars}");
        return stars;
    }

    public string GetRatingTitle(CampaignRatingType campaignType)
    {
        int stars = GetStarRating(campaignType);

        string title = stars switch
        {
            3 => "Elite Master",
            2 => "Skilled Player",
            _ => "Determined Survivor"
        };

        Debug.Log($"[PlayerPerformance][Rating Title] {campaignType} | {title}");
        return title;
    }

    #endregion

    #region Finish Campaigns

    // 🔒 DO NOT CHANGE PlayerPrefs KEY (LIVE USERS)
    private void FinishStandardCampaign()
    {
        if (PlayerPrefs.HasKey("RatingDone") && PlayerPrefsX.GetBool("RatingDone"))
        {
            Debug.Log("[PlayerPerformance] Standard Campaign rating already exists. Skipping.");
            return;
        }

        int stars = GetStarRating(CampaignRatingType.StandardCampaign);
        string title = GetRatingTitle(CampaignRatingType.StandardCampaign);

        standardCampaignRating = stars;
        Global.playerRatingMultiplier = stars;

        Debug.Log($"[PlayerPerformance][FINISH] Standard Campaign | Stars: {stars} ({title}) | Attempts: {standardCampaignAttempts}");

        PlayerPrefsX.SetBool("RatingDone", true);
        PlayerPrefs.Save();
    }

    // 🆕 SAFE NEW KEY
    private void FinishExtendedCampaign()
    {
        if (PlayerPrefsX.GetBool("ExtendedCampaignRatingDone", false))
        {
            Debug.Log("[PlayerPerformance] Extended Campaign rating already exists. Skipping.");
            return;
        }

        int stars = GetStarRating(CampaignRatingType.ExtendedCampaign);
        string title = GetRatingTitle(CampaignRatingType.ExtendedCampaign);

        extendedCampaignRating = stars;
        Global.playerExtendedRating = stars;

        Debug.Log($"[PlayerPerformance][FINISH] Extended Campaign | Stars: {stars} ({title}) | Attempts: {extendedCampaignAttempts}");

        PlayerPrefsX.SetBool("ExtendedCampaignRatingDone", true);
        PlayerPrefs.Save();
    }

    public static bool IsExtendedRatingGiven()
    {
        return PlayerPrefsX.GetBool("ExtendedCampaignRatingDone", false);
    }

    #endregion

    #region Reset (Use Carefully!)

    public void ResetStats()
    {
        standardCampaignAttempts = 0;
        extendedCampaignAttempts = 0;

        // ❗ DO NOT reset "RatingDone" for live users
        PlayerPrefsX.SetBool("ExtendedCampaignRatingDone", false);

        Debug.Log("[PlayerPerformance] Stats reset (Standard Campaign preserved).");
    }

    #endregion
}
