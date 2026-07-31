using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public GameObject gameOverPanel,fullPanel,gameOverFrame;
    public GameObject levelup, menu, retry, star0;
    public Button storeBtn;
    public Transform  spawnPoint, coinSpace, reachPoint;
    public Text levelNo,totalcoinsatWin;
    [SerializeField] GameObject[] stars;

    public GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager)
        {
            gameManager.gameOverPanel = gameOverPanel;
            gameManager.levelup = levelup;
            gameManager.menu = menu;
            gameManager.retry = retry;
            gameManager.star0 = star0;
            gameManager.spawnPoint = spawnPoint;
            gameManager.coinSpace = coinSpace;
            gameManager.reachPoint = reachPoint;
            gameManager.levelNo = levelNo;
            gameManager.totalcoinsatWin = totalcoinsatWin;
            gameManager.stars = stars;
            if(gameOverFrame)
            gameManager.gameOverFrame = gameOverFrame;

            menu.GetComponent<Button>().onClick.AddListener(gameManager.GoBackToWorldSel);
            levelup.GetComponent<Button>().onClick.AddListener(gameManager.NextLevel);
            retry.GetComponent<Button>().onClick.AddListener(gameManager.Load);
            storeBtn.onClick.AddListener(gameManager.StorePanel);
        }
        gameOverPanel.SetActive(false);
        fullPanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
