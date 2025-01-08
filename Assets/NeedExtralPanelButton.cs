using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedExtralPanelButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public  void needextraballactivate()
    {
        Debug.LogError("NEEDEXTRA OK CLICKED");
        AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
        #if UNITY_EDITOR
                GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                AdManager._instance.rewardedvideosuccess = true;
        #else
                AdManager._instance.ShowRewardedVideo();
        #endif

    }

    public static void needextraballactivateStatic()
    {
        //AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
        #if UNITY_EDITOR
                GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                AdManager._instance.rewardedvideosuccess = true;
        #else
                        AdManager._instance.ShowRewardedVideo();
        #endif

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
