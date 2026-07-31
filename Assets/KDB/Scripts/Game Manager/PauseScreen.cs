using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject gamePausePanel,fullPanel;
    public GameObject resumeBtn,homeBtn;
    public Text levelNum;
    public Toggle soundBtn;

    public GameManager gameManager;
    public SplashMute splashMute;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager)
        {
            gameManager.gamePausePanel = gamePausePanel;
            levelNum.text = "Level " + (Global.CurrentLeveltoPlay + 1);

            homeBtn.GetComponent<Button>().onClick.AddListener(gameManager.GoBack);
            resumeBtn.GetComponent<Button>().onClick.AddListener(gameManager.resume);
        }
        splashMute = FindObjectOfType<SplashMute>();
        if(splashMute)
        {
            splashMute.Soundbtn = soundBtn;
            soundBtn.onValueChanged.AddListener((value) => splashMute.MuteAudios(soundBtn));
        }
        gamePausePanel.SetActive(false);
        fullPanel.SetActive(true);

    }
}
