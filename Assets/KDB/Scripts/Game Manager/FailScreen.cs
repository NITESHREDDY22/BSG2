using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : MonoBehaviour
{
    public GameObject gameFailed,fullPanel;
    public GameObject failRetry,failMenu;
    public Text levelNoFail;
    [SerializeField] GameObject[] stars;

    public GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager)
        {
            gameManager.gameFailed = gameFailed;
            gameManager.failRetry = failRetry;
            gameManager.failMenu = failMenu;
            gameManager.levelNoFail = levelNoFail;

            failMenu.GetComponent<Button>().onClick.AddListener(gameManager.GoBackToWorldSel);
            failRetry.GetComponent<Button>().onClick.AddListener(gameManager.Load);
        }
        gameFailed.SetActive(false);
        fullPanel.SetActive(true);

    }
}
