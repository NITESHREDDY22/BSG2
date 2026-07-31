using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    [SerializeField]private Text reloadCoinsTxt;

    void OnEnable()
    {
        Debug.Log($"Reload finalReloadCoins:{Global.finalReloadCoins}");
        if(reloadCoinsTxt == null)
        reloadCoinsTxt = GetComponentInChildren<Text>();

        if(reloadCoinsTxt)
        reloadCoinsTxt.text = Global.finalReloadCoins.ToString();

        if(Global.finalReloadCoins <=0)
        {
            reloadCoinsTxt.text = "";
            Sprite replaySprite = Resources.Load<Sprite>("ReplayBtn2");

            // Assign it to the Image component
            if (replaySprite != null)
            {
                GetComponent<Image>().sprite = replaySprite;
            }
            else
            {
                Debug.LogError("Replay Sprite not found in Resources folder!");
            }


        }
    }
}
