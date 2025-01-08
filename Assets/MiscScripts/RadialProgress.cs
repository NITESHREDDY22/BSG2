using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour {
	public Image LoadingBar;
	float currentValue;
	public float speed;
	
	void Update() {
		if (this.gameObject.activeInHierarchy)
		{
			if (currentValue < 100)
			{
				currentValue += speed * Time.deltaTime;
			}
			else
			{
				//LoadingText.SetActive(false);
				//ProgressIndicator.text = "Done";
			}
			LoadingBar.fillAmount = currentValue / 100;

			if (currentValue > 100)
			{
				NeedExtralPanelButton.needextraballactivateStatic();
				//if(!GameManager.Instance.gameOverPanel.activeInHierarchy){
				//    GameManager.Instance.gameState = GameState.Lost;
				//    GameManager.Instance.OnGameFail();
				//}
				//AdScript.hideSkipRewardBtn = true;
			}
		}
	}
}