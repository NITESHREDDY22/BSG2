using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    [SerializeField]private Text reloadCoinsTxt;

    void OnEnable()
    {
        if(reloadCoinsTxt == null)
        reloadCoinsTxt = GetComponentInChildren<Text>();

        if(reloadCoinsTxt)
        reloadCoinsTxt.text = Global.coinsToReload.ToString();
    }
}
