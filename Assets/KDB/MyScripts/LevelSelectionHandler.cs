using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelectionHandler : MonoBehaviour {

    public static bool[] unlockedLevels;
    public static int[] levelStars;
    //public static int TotalStars =0;
    public int worldNumb = 1;
    public GameObject levelPrefab;

    void Awake()
    {
        Debug.unityLogger.logEnabled = true;
        //UnlockLevel(0);
        //PlayerPrefs.DeleteAll();
        unlockedLevels = PlayerPrefsX.GetBoolArray("_unlockedlevels" + worldNumb, false, WorldSelectionHandler.totalLevels[worldNumb]);
        // Debug.Log("wORLD levellsssssssssssssssssSSSS"+WorldSelectionHandler.totalLevels[worldNumb]);
        //Debug.Log(" uNLOCK lENGTH" + unlockedLevels.Length);
        unlockedLevels[0] = true;
        //  Unlock levels for testing
        //for (int i = 0; i < unlockedLevels.Length; i++)
        //{
        //    unlockedLevels[i] = true;
        //}
        // Unlock levels for test end

        PlayerPrefsX.SetBoolArray("_unlockedlevels" + worldNumb, unlockedLevels);
        levelStars = PlayerPrefsX.GetIntArray("_levelStars" + worldNumb, 0, WorldSelectionHandler.totalLevels[worldNumb]);
        ConfigLvlButtons();
        // TotalStarsforworld();

    }

    Button[] btns;
    [SerializeField] Transform levelsPanel;
    [SerializeField] Sprite[] starSkins;
    public void ConfigLvlButtons()
    {

        for (int i = 0; i < unlockedLevels.Length; i++)
        {
            //Debug.Log(" 2uNLOCK lENGTH" + unlockedLevels.Length);
            GameObject LP = Instantiate(levelPrefab, levelsPanel.transform);
            LP.name = "Level_(" + (i + 1) + ")";
            LP.GetComponent<Button>().onClick.AddListener(() => SelectLevel(LP.transform));
        }

        btns = levelsPanel.GetComponentsInChildren<Button>();

        for (int i = 0; i < btns.Length; i++)
        {
            if (unlockedLevels[i])
            {
                btns[i].transform.Find("lock").GetComponent<Image>().enabled = false;
                btns[i].interactable = true;
                //Debug.Log("lvl" + i + " lvlStars " + levelStars[i]);
                btns[i].image.sprite = starSkins[levelStars[i]];
            }
            else
            {
                btns[i].transform.Find("lock").GetComponent<Image>().enabled = true;
                btns[i].interactable = false;
            }
            btns[i].transform.Find("serial").GetComponent<Text>().text = (i + 1) + "";
        }
    }

    public static int LevelsUnlocked(int world)
    {

        unlockedLevels = PlayerPrefsX.GetBoolArray("_unlockedlevels" + world, false, WorldSelectionHandler.totalLevels[world]);
        //unlockedLevels[0] = true;
        int unlocked = 0;
        foreach (bool val in unlockedLevels)
        {
            if (val == true)
                unlocked++;
        }
        return unlocked;
    }

    public void SelectLevel(Transform _tr)
    {
        AdManager._instance.ShowLoadingPanel();
        SoundManager.PlaySFX("Click2");
        Global.CurrentLeveltoPlay = _tr.GetSiblingIndex();
        SceneManager.LoadScene("GamePlay_W" + WorldSelectionHandler.worldSelected.ToString() + "_" + (int)(Global.CurrentLeveltoPlay / 5));
        //SceneManager.LoadScene("GamePlay_W" + WorldSelectionHandler.worldSelected.ToString()+"_"+(int)(Global.CurrentLeveltoPlay/10));
        //SceneManager.LoadScene("GamePlay_W" + WorldSelectionHandler.worldSelected.ToString());
    }

    public static void UnlockLevel(int level)
    {
        //Debug.Log("********UnlockLevel " + level+" worldSelected "+WorldSelectionHandler.worldSelected);
        unlockedLevels = PlayerPrefsX.GetBoolArray("_unlockedlevels" + WorldSelectionHandler.worldSelected,
            false, WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected]);
        unlockedLevels[level] = true;
        PlayerPrefsX.SetBoolArray("_unlockedlevels" + WorldSelectionHandler.worldSelected, unlockedLevels);

    }

    //public static void SetStarsOfLevel(int level,int worldno, int stars)
    //{
    //    //Debug.Log("***** UnlockLevel stars " + level+" stars "+stars);
    //    /* levelStars = PlayerPrefsX.GetIntArray("_levelStars" + WorldSelectionHandler.worldSelected,
    //         0, WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected]);
    //     levelStars[level] = stars;
    //     PlayerPrefsX.SetIntArray("_levelStars" + WorldSelectionHandler.worldSelected, levelStars); */

    //    levelStars = PlayerPrefsX.GetIntArray("_levelStars" + worldno,
    //        0, WorldSelectionHandler.totalLevels[worldno]);
    //    levelStars[level] = stars;
    //    PlayerPrefsX.SetIntArray("_levelStars" + worldno, levelStars);
    //}

    public static void SetStarsOfLevel(int level, int worldno, int stars)
    {
        //Debug.Log("***** UnlockLevel stars " + level+" stars "+stars);
        /* levelStars = PlayerPrefsX.GetIntArray("_levelStars" + WorldSelectionHandler.worldSelected,
             0, WorldSelectionHandler.totalLevels[WorldSelectionHandler.worldSelected]);
         levelStars[level] = stars;
         PlayerPrefsX.SetIntArray("_levelStars" + WorldSelectionHandler.worldSelected, levelStars); */
        levelStars = PlayerPrefsX.GetIntArray("_levelStars" + worldno,
            0, WorldSelectionHandler.totalLevels[worldno]);
        if (levelStars[level] < stars)
        {
            levelStars[level] = stars;
            PlayerPrefsX.SetIntArray("_levelStars" + worldno, levelStars);
        }
    }

}
