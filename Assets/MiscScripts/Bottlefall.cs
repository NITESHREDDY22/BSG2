using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottlefall : MonoBehaviour {
    public float floatStrength = .2f;
    GameManager gm;
    public GameObject HintObj;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, floatStrength, 0);
        gm = GameManager.Instance;
        if (HintObj != null)
        {
            HintObj.SetActive(true);
            Destroy(HintObj, 5f);
        }
    }
  

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Bird" || target.gameObject.tag == "pig")
        {
            Destroy(target.gameObject);
            Invoke("failLevel", .5f);
        }
    }


    void failLevel()
    {
        if (!gm.gameFailed.activeInHierarchy)
        {
            int levelNum = (Global.CurrentLeveltoPlay + 1);
            gm.levelNoFail.text = "" + levelNum;
            gm.gameState = GameState.Lost;
            gm.OnGameFail();
            Destroy(gameObject);
        }
    }

}
