using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if !CHEATS_ON
        gameObject.SetActive(false);
        #endif
    }

    public void LevelCompelte()
    {
        if(GameManager.Instance)
        GameManager.Instance.ShowNewLevelComplete();
    }
}
