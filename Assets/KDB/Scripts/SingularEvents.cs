using Singular;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Analytics;

public class SingularEvents : MonoBehaviour
{
    public static SingularEvents instance;

    private readonly string levelCompleteString = "BSG2_LevelComplete_";
    private readonly string achievementsString = "BSG2_AchievedStars_";

    private int[] starCount = new int[] { 50, 100, 150, 200, 250,300 };
    private int[] themesLevelsCount = new int[] { 80, 30, 50, 20, 20 };
    private int LevelNuberIteration = 15;


    private void Awake()
    {
        instance = this;
        //TO DO
        //for (int i = 0; i < themesLevelsCount.Length; i++)
        //{
        //    for (int j = 0; j < themesLevelsCount[i]; j++)
        //    {
        //        string eventString = TargetLevelString(i, j);
        //        if (!string.IsNullOrEmpty(eventString))
        //        {
        //            count++;
        //            StartCoroutine(waitEvent(i, j));
        //        }
        //       //SendLevelCompleteEvent(i, j);
        //    }
        //}
        //for (int i = 0; i < starCount.Length; i++)
        //{
        //    StartCoroutine(waitStarsEvent(30*(i+1)));
        //}
    }
    IEnumerator waitEvent(int i,int j)
    {
        yield return new WaitForSeconds(60* count);
        SendLevelCompleteEvent(i, j);       
    }
    int count = 0;
    IEnumerator waitStarsEvent(float timer)
    {
        yield return new WaitForSeconds(timer);
        //SendAchieventmentEvent(0);


    }

    public void SendLevelCompleteEvent(int worldNumber,int LevelNumber)
    {
        string eventString = TargetLevelString(worldNumber, LevelNumber);
        if (!string.IsNullOrEmpty(eventString))
        {
            SingularSDK.Event(eventString, "worldNumber", worldNumber, "LevelNumber", LevelNumber);
            Debug.LogError(eventString);
        }
    }

    public void SendAchieventmentEvent()
    {
        try
        {
            int levelStars = 0;
            for (int i = 0; i < WorldSelectionHandler.totalLevels.Length; i++)
            {
                int[] levelStarsArr = PlayerPrefsX.GetIntArray("_levelStars" + i, 0, WorldSelectionHandler.totalLevels[i]);
                for (int j = 0; j < levelStarsArr.Length; j++)
                {
                    levelStars += levelStarsArr[j];
                }
            }

            if (levelStars > 0)
            {
                int indexValue = -1;
                for (int i = 0; i < starCount.Length; i++)
                {
                    if (levelStars >= starCount[i])
                    {
                        indexValue = i;
                    }
                }
                if (indexValue >= 0)
                {
                    string eventKey = string.Concat(achievementsString, starCount[indexValue]);
                    // Debug.LogError(eventKey);
                    SingularSDK.Event(eventKey);
                }
                //Debug.LogError(eventString);
            }
        }
        catch 
        {
            
        }

    }

    public void SendLevelFailEvent(string worldNumber, string LevelNumber)
    {
       //SingularSDK.Event("LevelFail", "worldNumber", worldNumber, "LevelNumber", LevelNumber);
    }

    private string TargetLevelString(int worldNumber,int levelNumber)
    {
        int previouseLevelNumber = 0;
        if (worldNumber > 0)
        {
            for (int i = 0; i < worldNumber; i++)
            {
                previouseLevelNumber += themesLevelsCount[i];
            }
        }
        int totalLevelNumber=previouseLevelNumber+(levelNumber+1);
        int targetLevelNumber = 0;
        if(totalLevelNumber>0 && totalLevelNumber%LevelNuberIteration==0)
        {
            targetLevelNumber=totalLevelNumber/LevelNuberIteration;
        }
        if(targetLevelNumber>0)
        {
            return string.Concat(levelCompleteString,(targetLevelNumber*LevelNuberIteration));
        }
        return string.Empty;
       


    }

}
