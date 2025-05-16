using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedExtralPanelButton : MonoBehaviour {

    // Use this for initialization
    public static bool AlreadyShown=false;
    private const string skipLevelKey = "SKIP_LEVEL";
    private const string extraballLevelKey = "EXTRABALL_LEVEL";

    private void OnEnable()
    {
        AlreadyShown = false;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public  void needextraballactivate()
    {
        Debug.Log("NEEDEXTRA OK CLICKED");
        AdManager._instance.rewardTypeToUnlock = RewardType.extraball;       
        AdManager._instance.ShowRewardedVideo(result=>
                {
                    if(result)
                    {
                        GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                        AdManager._instance.rewardedvideosuccess = true;
                        GameManager.Instance.ingamevideosuccess();
                    }
                },AdType.Reward);


    }

    public static void needextraballactivateStatic()
    {
        if (AlreadyShown)
            return;

        // AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
        if (AdManager._instance.rewardTypeToUnlock == RewardType.continuegame)
        {
            AdManager._instance.ShowRewardedInterstitial(result =>
            {
                if (result)
                {                   
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = true;
                    GameManager.Instance.ingamevideosuccess();
                    AlreadyShown = true;
                }
                else
                {
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = false;
                    GameManager.Instance.ingamevideosuccess();                  
                }
            }, AdType.RewardedInterStitial);
        }
        else
        {
            AdManager._instance.ShowRewardedVideo(result =>
            {
                if (result)
                {
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;                   
                    AdManager._instance.rewardedvideosuccess = true;
                    GameManager.Instance.ingamevideosuccess();
                    AlreadyShown = true;
                }
                else
                {
                    GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                    AdManager._instance.rewardedvideosuccess = false;
                    GameManager.Instance.ingamevideosuccess();                    
                }
            }, AdType.Reward);
        }
    }


    public void close()
    {
        if (!GameManager.Instance.gameOverPanel.activeInHierarchy)
        {
            GameManager.Instance.gameState = GameState.Lost;
            GameManager.Instance.OnGameFail();
        }
        else
        {
            GameManager.Instance.gameState = GameState.Won;
            GameManager.Instance.rewardCanvas.SetActive(false);
        }
    }
}
