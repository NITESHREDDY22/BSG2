using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedExtralPanelButton : MonoBehaviour {

    // Use this for initialization
    public static bool AlreadyShown=false;
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
        #if UNITY_EDITOR
                GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                AdManager._instance.rewardedvideosuccess = true;
#else
                GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                AdManager._instance.ShowRewardedVideo(result=>
                        {
                            if(result)
                            {   
                            AdManager._instance.rewardedvideosuccess = true;
                            GameManager.Instance.ingamevideosuccess();
                            }
                        },AdType.RewardContinue);
#endif

    }

    public static void needextraballactivateStatic()
    {
        if (AlreadyShown)
            return;
        //AdManager._instance.rewardTypeToUnlock = RewardType.extraball;
        #if UNITY_EDITOR
                GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                AdManager._instance.rewardedvideosuccess = true;

#else
                        GameManager.Instance.gameState = GameState.Reward_Video_Completed;
                        AdManager._instance.ShowRewardedVideo(result=>
                        {
                            if(result)
                            {   
                            AdManager._instance.rewardedvideosuccess = true;
                            GameManager.Instance.ingamevideosuccess();
                            AlreadyShown=true;
                            }
                        },AdType.RewardContinue);
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
