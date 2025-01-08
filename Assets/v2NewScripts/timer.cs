using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class timer : MonoBehaviour
{
    //public Floatvariable lvl;
    //public bool isDisplayed = false;

    //[HideInInspector]
    public int lvlTime =0;

    public TextMeshProUGUI txt; 
    public Coroutine _timerunning;

    void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        if (this.gameObject.activeInHierarchy)
        {
            _timerunning = StartCoroutine(StartAction());
        }

    }


    public void stoptime()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.StopCoroutine(_timerunning);
        }
    }
  public IEnumerator StartAction()
   {
       if(lvlTime <= 0){
           lvlTime = 60;
       }
        
        if (lvlTime > 60)
        {
            txt.text = Mathf.Floor(lvlTime / 60) + " : " + lvlTime % 60;
        }
        else
        {
            txt.text = ""+(lvlTime % 60);
        }
        while (lvlTime > 0.5f){
            yield return new WaitForSeconds(1f);
            if(GameManager.Instance.gameState != GameState.Won&& GameManager.Instance.gameState != GameState.Lost && GameManager.Instance.gameState != GameState.Pause)
            {
                lvlTime -= 1;
                int minutes = Mathf.FloorToInt(lvlTime / 60F);
                int seconds = Mathf.FloorToInt(lvlTime - minutes * 60);
                if (minutes > 0)
                {
                    txt.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                }
                else
                {
                    txt.text = string.Format("{0:00}" ,seconds);
                }
                //txt.text = Mathf.Floor(lvlTime / 60) + " : " + lvlTime % 60;
            }
            else
            {
                stoptime();
            }    
       }
        //gameover logic call
        if (!GameManager.Instance.gameOverPanel.activeInHierarchy)
        {
            gameObject.GetComponent<BombScript>().isTimerBomb = true;
            gameObject.GetComponent<BombScript>().explode(false);
            GetComponent<SpriteRenderer>().enabled = false;
            txt.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            GameManager.Instance.slingShot.SlingBlast();
            Invoke("ShowGameOverPanel", 2.5f);
        }
        else
        {
            stoptime();
        }
   }

    void ShowGameOverPanel() {
        if (!GameManager.Instance.gameOverPanel.activeInHierarchy)
        {
            GameManager.Instance.gameState = GameState.Lost;
            GameManager.Instance.OnGameFail();
            Destroy(gameObject);
        }
    }


}
