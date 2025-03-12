using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.SimpleLocalization;
using TMPro;
public class StoreManager : MonoBehaviour
{
    public Sprite SelectedBG, SelectBG;
    public GameObject StoreObjectPrefab;
    public GameObject BottleObjectPrefab;
    public BottleStoreObject[] Bottles;
    public StoreObject[] Balls;
    
    public Transform ScrollContent, ScrollContent2;
    [Header("Dont Edit")]
    public List<GameObject> storeobjs;
    public List<GameObject> bottleobjs;
    public static StoreManager _instance;

    public Text CoinsCount;
    public float scaleDefault, scaleSelected;
    public float scaleDefault2, scaleSelected2;
    public GameObject RewardPanel;

    public void Start()
    {
        //test
        //PlayerPrefs.DeleteAll();
        if (_instance == null)
        {
            _instance = this;
        }

        string selectedString = LocalizationManager.Localize("SELECTED");
        string selectString = LocalizationManager.Localize("SelectString");

        storeobjs = new List<GameObject>();
        bottleobjs = new List<GameObject>();

        if (!PlayerPrefs.HasKey("Ball_Stat_0"))
        {
            //PlayerPrefs.SetInt("coins", 1000);
            PlayerPrefs.SetInt("Ball_Stat_0", 2);
            PlayerPrefs.SetInt("BallSelected", 0);
        }
        if (!PlayerPrefs.HasKey("Bottle_Stat_0"))
        {
            PlayerPrefs.SetInt("Bottle_Stat_0", 2);
            PlayerPrefs.SetInt("BottleSelected", 0);
        }
        try
        {
            for (int i = 0; i < Balls.Length; i++)
        {
            GameObject g = Instantiate(StoreObjectPrefab, ScrollContent);
            storeobjs.Add(g);
            g.name = i.ToString();
            g.transform.GetChild(2).GetComponent<Image>().sprite = Balls[i].ballImg;
            g.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => CTA(int.Parse(g.name)));
            if (PlayerPrefs.GetInt("Ball_Stat_" + i, 0) == 0)
            {
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = Balls[i].price + "";
                g.transform.GetChild(1).gameObject.SetActive(false);
                //g.transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                //g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault, scaleDefault);
                Balls[i].myStat = StoreObjectStatus.NotPurchased;

            }
            else if (PlayerPrefs.GetInt("Ball_Stat_" + i, 0) == 1)
            { 
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectString;
                g.transform.GetChild(1).gameObject.SetActive(false);
                g.transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                //g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault, scaleDefault);
                Balls[i].myStat = StoreObjectStatus.Select;

            }
            else if (PlayerPrefs.GetInt("Ball_Stat_" + i, 0) == 2)
            {
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedString;
                    g.transform.GetChild(1).gameObject.SetActive(true);
                g.transform.GetChild(3).GetComponent<Image>().sprite = SelectedBG;
                //g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleSelected, scaleSelected);
                Balls[i].myStat = StoreObjectStatus.Selected;
            }
        }

        //Generate Bottle StoreObejcts
        for (int i = 0; i < Bottles.Length; i++)
        {
            GameObject g = Instantiate(BottleObjectPrefab, ScrollContent2);
            bottleobjs.Add(g);
            g.name = i.ToString();
            g.transform.GetChild(2).GetComponent<Image>().sprite = Bottles[i].bottleImg;
            g.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => CTA1(int.Parse(g.name)));
            if (PlayerPrefs.GetInt("Bottle_Stat_" + i, 0) == 0)
            {
                    
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = Bottles[i].price + "";
                g.transform.GetChild(1).gameObject.SetActive(false);
                //g.transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                //g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault2, scaleDefault2);
                Bottles[i].myStatBottle = BottleStoreobjstatus.NotPurchased_bottle;
            }
            else if (PlayerPrefs.GetInt("Bottle_Stat_" + i, 0) == 1)
            {
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("SelectString");
                    g.transform.GetChild(1).gameObject.SetActive(false);
                g.transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                //g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault2, scaleDefault2);
                Bottles[i].myStatBottle = BottleStoreobjstatus.Select_bottle;
            }
            else if (PlayerPrefs.GetInt("Bottle_Stat_" + i, 0) == 2)
            {
                g.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("SELECTED");
                    g.transform.GetChild(1).gameObject.SetActive(true);
                g.transform.GetChild(3).GetComponent<Image>().sprite = SelectedBG;
                // g.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleSelected2, scaleSelected2);
                Bottles[i].myStatBottle = BottleStoreobjstatus.Selected_bottle;

            }
        }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Storemanager", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
       
    public void dostorecoinsupdate()
    {
        CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }
    public void UpdateStoreObjects(int index)
    {
        string selectedString= LocalizationManager.Localize("SELECTED");
        string selectString = LocalizationManager.Localize("SelectString");


        for (int i = 0; i < storeobjs.Count; i++)
        {
            if (Balls[i].myStat == StoreObjectStatus.Selected)
            {
                storeobjs[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedString;
                storeobjs[i].transform.GetChild(3).GetComponent<Image>().sprite = SelectedBG;
                storeobjs[i].transform.GetChild(1).gameObject.SetActive(true);
                //storeobjs[i].transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleSelected, scaleSelected);
                if(FirebaseEvents.instance)
                FirebaseEvents.instance.LogFirebaseEvent("Ball_" + i + "selected");
            }
            else
            {
                if (Balls[i].myStat == StoreObjectStatus.Select)
                {
                    storeobjs[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectString;
                    storeobjs[i].transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                    storeobjs[i].transform.GetChild(1).gameObject.SetActive(false);
                    //storeobjs[i].transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault, scaleDefault);
                }
            }
        }
    }
    //BottleStore update
    public void UpdateBottleStoreObjects(int index)
    {

        string selectedString = LocalizationManager.Localize("SELECTED");
        string selectString = LocalizationManager.Localize("SelectString");

        for (int i = 0; i < bottleobjs.Count; i++)
        {
            if (Bottles[i].myStatBottle == BottleStoreobjstatus.Selected_bottle)
            {
                bottleobjs[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedString;
                bottleobjs[i].transform.GetChild(3).GetComponent<Image>().sprite = SelectedBG;
                bottleobjs[i].transform.GetChild(1).gameObject.SetActive(true);
                // bottleobjs[i].transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleSelected2, scaleSelected2);
                FirebaseEvents.instance.LogFirebaseEvent("Bottle_" + i + "selected");

            }
            else
            {
                if (Bottles[i].myStatBottle == BottleStoreobjstatus.Select_bottle)
                {
                 bottleobjs[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectString;
                    bottleobjs[i].transform.GetChild(3).GetComponent<Image>().sprite = SelectBG;
                    bottleobjs[i].transform.GetChild(1).gameObject.SetActive(false);
                    //bottleobjs[i].transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(scaleDefault2, scaleDefault2);
                }
            }
        }
    }
    public void CTA(int index)
    {

        int totalcoins = PlayerPrefs.GetInt("coins");
        StoreObjectStatus status = Balls[index].myStat;
        if (status == StoreObjectStatus.Select)
        {

            //update storeobjs
            setstoreobjselected(index);
            UpdateStoreObjects(index);
            PlayerPrefs.SetInt("BallSelected", index);
            PlayerPrefs.SetInt("Ball_Stat_" + index, 2); //NotPurchased = 0 ,Select = 1 ,Selected = 2
                                                         //Update UI Button price to Select Label 
        }
        else if (status == StoreObjectStatus.NotPurchased)
        {

            if (totalcoins >= Balls[index].price)
            {
                totalcoins = totalcoins - Balls[index].price;
                Balls[index].myStat = StoreObjectStatus.Select;
                UpdateStoreObjects(index);
                PlayerPrefs.SetInt("Ball_Stat_" + index, 1);
                PlayerPrefs.SetInt("coins", totalcoins);
                CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();

            }
            else
            {

                //Show Popup For not Enough Coins
                if (AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo.CanShowAd()
                    || AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo.IsAdReady())
                {
                    RewardPanel.SetActive(true);
                    FirebaseEvents.instance.LogFirebaseEvent("StoreRewardPopupShown");

                }
              
                //totalcoinsAnim.Play();
            }
        }
        else if (status == StoreObjectStatus.Selected)
        {
            //Already Selected .. nothing to change
        }

    }

    public void CTA1(int index)
    {

        int totalcoins = PlayerPrefs.GetInt("coins");
        BottleStoreobjstatus status1 = Bottles[index].myStatBottle;
        //Debug.LogError("AAAAADDFFDD_" + index);
        //yield return new WaitForSeconds(0f);

        if (status1 == BottleStoreobjstatus.Select_bottle)
        {

            //update bottle storeobjs
            setbottlestoreobjselected(index);
            UpdateBottleStoreObjects(index);
            PlayerPrefs.SetInt("BottleSelected", index);
            PlayerPrefs.SetInt("Bottle_Stat_" + index, 2); //NotPurchased = 0 ,Select = 1 ,Selected = 2
                                                           //Update UI Button price to Select Label 
        }
        else if (status1 == BottleStoreobjstatus.NotPurchased_bottle)
        {

            if (totalcoins >= Bottles[index].price)
            {
                totalcoins = totalcoins - Bottles[index].price;
                Bottles[index].myStatBottle = BottleStoreobjstatus.Select_bottle;
                UpdateBottleStoreObjects(index);
                PlayerPrefs.SetInt("Bottle_Stat_" + index, 1);
                PlayerPrefs.SetInt("coins", totalcoins);
                CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();

            }
            else
            {
                //Show Popup For not Enough Coins
                if (AdManager._instance.adMobNetworkHandler.adMobRewardBasedVideo.CanShowAd()
                   || AdManager._instance.levelPlayNetworkHandler.levelPlayrewardBasedVideo.IsAdReady())
                {
                    RewardPanel.SetActive(true);
                    FirebaseEvents.instance.LogFirebaseEvent("StoreRewardPopupShown");
                }
                //totalcoinsAnim.Play();
            }
        }
        else if (status1 == BottleStoreobjstatus.Selected_bottle)
        {
            //Already Selected .. nothing to change
        }

    }
    void setstoreobjselected(int index)
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            if (i == index)
            {
                Balls[i].myStat = StoreObjectStatus.Selected;
            }
            else
            {
                if (Balls[i].myStat == StoreObjectStatus.Selected)
                {
                    Balls[i].myStat = StoreObjectStatus.Select;
                    PlayerPrefs.SetInt("Ball_Stat_" + i, 1);
                }
            }

        }
    }
    void setbottlestoreobjselected(int index)
    {
        for (int i = 0; i < Bottles.Length; i++)
        {
            if (i == index)
            {
                Bottles[i].myStatBottle = BottleStoreobjstatus.Selected_bottle;
            }
            else
            {
                if (Bottles[i].myStatBottle == BottleStoreobjstatus.Selected_bottle)
                {
                    Bottles[i].myStatBottle = BottleStoreobjstatus.Select_bottle;
                    PlayerPrefs.SetInt("Bottle_Stat_" + i, 1);
                }
            }

        }

    }
    public void CancelReward()
    {
        RewardPanel.SetActive(false);
        FirebaseEvents.instance.LogFirebaseEvent("StoreRewardPopupClosed");
        //GifAdsManager.Instance.HideAD(GifAdsManager.Instance._adObjs[0]);
    }

    public void watchRewardAd()
    {
        AdManager._instance.rewardTypeToUnlock = RewardType.store;
#if UNITY_EDITOR

        AdManager._instance.rewardedvideosuccess = true;
        onRewardVideoSuccess();
 #else
        AdManager._instance.ShowRewardedVideo(result =>
        {
            onRewardVideoSuccess();
        });
#endif
    }

    public void onRewardVideoSuccess()
    {
        
        //PlayerPrefs.SetInt("coins", 1000);
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") +1000);
        CoinsCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        RewardPanel.SetActive(false);
        FirebaseEvents.instance.LogFirebaseEvent("StoreRewardVideoCompleted");
    }
    [Serializable]
    public class StoreObject
    {
        public Sprite ballImg;
        public int price;
        public StoreObjectStatus myStat;
    }
    [Serializable]
    public class BottleStoreObject
    {
        public Sprite bottleImg;
        public int price;
        public BottleStoreobjstatus myStatBottle;
    }
    public enum StoreObjectStatus
    {
        NotPurchased = 0, Select = 1, Selected = 2

    }


    public enum BottleStoreobjstatus
    {
        NotPurchased_bottle = 0, Select_bottle = 1, Selected_bottle = 2
    }

}