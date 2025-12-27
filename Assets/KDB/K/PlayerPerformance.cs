using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPerformance : MonoBehaviour
{
    // Singleton Instance
    public static PlayerPerformance Instance { get; private set; }

    [Header("Game Tracking")]
    public int totalAttempts = 0;
    //public int currentLevel = 1;
    public int maxLevels = 5;

    [SerializeField]
    private int playerRating = 0;

    private void Awake()
    {
        // Persistence: Make sure this object stays alive across all 5 levels
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Call this when the player dies or restarts
    public void RecordAttempt()
    {
        totalAttempts++;
        Debug.Log($"Attempt logged! Total so far: {totalAttempts}");
    }

    // 2. Call this when the player finishes a level
    public void CompleteLevel()
    {
         Debug.Log($"GAME COMPLETE! \n level:{Global.CurrentLeveltoPlay},\n totalAttempts:{totalAttempts},\n maxLevels:{maxLevels}");
        if (Global.CurrentLeveltoPlay < maxLevels-1)
        {
            //currentLevel++;
            Debug.Log($"Level {Global.CurrentLeveltoPlay - 1} Clear! Next up: Level {Global.CurrentLeveltoPlay}");
            // SceneManager.LoadScene("Level" + currentLevel);
        }
        else
        {
            FinishGame();
        }
    }

    // 3. Logic to determine the Star Rating
    public int GetStarRating()
    {
        // Based on 5 levels total:
        if (totalAttempts <= maxLevels+2) return 3;   // Almost perfect (0-2 deaths total)
        if (totalAttempts <= (maxLevels*2)) return 2;  // Good (average 2-3 deaths per level)
        return 1;                           // Persistence (many deaths, but finished)
    }

    // 4. Logic to determine the Text Category
    public string GetRatingTitle()
    {
        int stars = GetStarRating();
        return stars switch
        {
            3 => "Elite Master",
            2 => "Skilled Player",
            _ => "Determined Survivor"
        };
    }

    private void FinishGame()
    {
        if(PlayerPrefs.HasKey("RatingDone") && PlayerPrefsX.GetBool("RatingDone") == true)
        return;

        int stars = GetStarRating();
        string title = GetRatingTitle();
        playerRating = stars;
        Global.playerRatingMultiplier = stars;
        Debug.Log($"GAME COMPLETE! Rating: {stars} Stars - {title} ({totalAttempts} attempts)");
        PlayerPrefsX.SetBool("RatingDone",true);
        PlayerPrefs.Save();
        // You would typically trigger your Win UI here
    }

    public void ResetStats()
    {
        totalAttempts = 0;
        //currentLevel = 1;
    }
}