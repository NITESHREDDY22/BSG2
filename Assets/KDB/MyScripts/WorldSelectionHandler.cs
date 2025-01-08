using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.SimpleLocalization;

public class WorldSelectionHandler : MonoBehaviour
{

    public Sprite[] worldImages;
    public GameObject WorldPrefab;//Achieve  30 stars to unlock this World
    public Transform contentPanel;
    public bool[] unlockedWorlds;
    public static int[] totalLevels = new int[] { 80, 30, 50, 20, 20 };
    public static int worldNumb = 0;

    public Text[] worldname_levelspanel;

    public int[] worldStars;
    public GameObject spinPopUP;
    void Start()
    {
       // GifAdsManager.Instance.HideAD(GifAdsManager.Instance._adObjs[0]);
    }
    void starsupdate()
    {
        Global.TotalStarsAchivedWorld1 = TotalStarsforworld(0);
        Global.TotalStarsAchivedWorld2 = TotalStarsforworld(1);
        Global.TotalStarsAchivedWorld3 = TotalStarsforworld(2);
        Global.TotalStarsAchivedWorld4 = TotalStarsforworld(3);
        Global.TotalStarsAchivedWorld5 = TotalStarsforworld(4);
    }
    public void GenerateWorlds()
    {
        //Global.CurrentLeveltoPlay = 0;
        /*for (int i = 0; i < worldImages.Length; i++)
        {
            worldStars[i] = TotalStarsforworld(i);
            Debug.LogWarning("Stars : " + TotalStarsforworld(i) + " : World 2 Req : "+ Global.World2ReqStars + 
                " : World 3 Req : " + Global.World3ReqStars + " : World 4 Req : " + Global.World4ReqStars + 
                " : World 5 Req : " + Global.World5ReqStars);
        }*/

        /* Global.TotalStarsAchivedWorld1 = TotalStarsforworld(0);
         Global.TotalStarsAchivedWorld2 = TotalStarsforworld(1);
         Global.TotalStarsAchivedWorld3 = TotalStarsforworld(2);
         Global.TotalStarsAchivedWorld4 = TotalStarsforworld(3);
         Global.TotalStarsAchivedWorld5 = TotalStarsforworld(4);*/
        starsupdate();
        Debug.LogWarning("World1 : " + Global.TotalStarsAchivedWorld1 + " || World 2 Req : " + Global.World2ReqStars +
                           "World2 : " + Global.TotalStarsAchivedWorld2 + " || World 3 Req : " + Global.World3ReqStars +
                           "World3 : " + Global.TotalStarsAchivedWorld3 + " || World 4 Req : " + Global.World4ReqStars +
                           "World4 : " + Global.TotalStarsAchivedWorld4 + " || World 5 Req : " + Global.World5ReqStars);

        unlockedWorlds = PlayerPrefsX.GetBoolArray("_unlockedWorlds", false, worldImages.Length);
        for (int i = 0; i < unlockedWorlds.Length; i++)
        {
            unlockedWorlds[i] = false;
        }
        unlockedWorlds[0] = true;
        if (Global.TotalStarsAchivedWorld1 >= Global.World2ReqStars)
        {
            unlockedWorlds[1] = true;
        }
        if (Global.TotalStarsAchivedWorld2 >= Global.World3ReqStars)
        {
            unlockedWorlds[2] = true;
        }
        if (Global.TotalStarsAchivedWorld3 >= Global.World4ReqStars)
        {
            unlockedWorlds[3] = true;
        }
        if (Global.TotalStarsAchivedWorld4 >= Global.World5ReqStars)
        {
            unlockedWorlds[4] = true;
        }
        //testing
        // for (int i = 0; i < worldImages.Length; i++)
        //{
        //    unlockedWorlds[i] = true;
        //}

        //for (int i=1;i<worldImages.Length;i++){
        //    if(TotalStarsforworld(i-1)>= FirebaseRemoteConfiguration.instance.worldStarcountRq[i]){
        //     unlockedWorlds[i]=true;
        //    }
        // }
        PlayerPrefsX.SetBoolArray("_unlockedWorlds", unlockedWorlds);
        //if (this.gameObject.activeInHierarchy)
        //{
        generateWorldObjs();
        ConfigLvlButtons();
        //}
    }

    public void generateWorldObjs()
    {
        btns = new List<GameObject>();
        for (int i = 0; i < worldImages.Length; i++)
        {
            GameObject Wobj = Instantiate(WorldPrefab, contentPanel);
            Wobj.name = worldImages[i].name;
            Wobj.GetComponent<Image>().sprite = worldImages[i];
            Wobj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectWorld(Wobj.transform));
            

            if (i == 1)
            {
                string txtLabel = LocalizationManager.Localize("Worldlock_popupClue1");
                string[] splitString = txtLabel.Split(new string[] { "_" }, StringSplitOptions.None);
                Wobj.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = splitString[0] + " " + (Global.World2ReqStars - Global.TotalStarsAchivedWorld1) + " " + splitString[1];
            }
            else if (i == 2)
            {
                string txtLabel = LocalizationManager.Localize("Worldlock_popupClue2");
                string[] splitString = txtLabel.Split(new string[] { "_" }, StringSplitOptions.None);
                Wobj.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = splitString[0] + " " + (Global.World3ReqStars - Global.TotalStarsAchivedWorld2) + " " + splitString[1];
            }
            else if (i == 3)
            {
                string txtLabel = LocalizationManager.Localize("Worldlock_popupClue3");
                string[] splitString = txtLabel.Split(new string[] { "_" }, StringSplitOptions.None);
                Wobj.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = splitString[0] + " " + (Global.World4ReqStars - Global.TotalStarsAchivedWorld3) + " " + splitString[1];
            }
            else if (i == 4)
            {
                string txtLabel = LocalizationManager.Localize("Worldlock_popupClue4");
                string[] splitString = txtLabel.Split(new string[] { "_" }, StringSplitOptions.None);
                Wobj.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = splitString[0] + " " + (Global.World5ReqStars - Global.TotalStarsAchivedWorld4) + " " + splitString[1];
            }
            btns.Add(Wobj);
        }
    }
    List<GameObject> btns;

    public void ConfigLvlButtons()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            if (unlockedWorlds[i])
            {
                btns[i].transform.GetChild(1).gameObject.SetActive(false);
                btns[i].transform.GetChild(3).gameObject.SetActive(true);
                btns[i].transform.GetChild(4).gameObject.SetActive(true);
                btns[i].transform.GetChild(3).transform.Find("WorldName").GetComponent<Text>().text = LocalizationManager.Localize("WorldsPanel_name"+i); //WorldsPanel_name0//btns[i].name;
                if (i == 0)
                {
                    btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = Global.TotalStarsAchivedWorld1 + "/" + (totalLevels[i] * 3);
                }
                else if (i == 1)
                {
                    btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = Global.TotalStarsAchivedWorld2 + "/" + (totalLevels[i] * 3);
                }
                else if (i == 2)
                {
                    btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = Global.TotalStarsAchivedWorld3 + "/" + (totalLevels[i] * 3);
                }
                else if (i == 3)
                {
                    btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = Global.TotalStarsAchivedWorld4 + "/" + (totalLevels[i] * 3);
                }
                else if (i == 4)
                {
                    btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = Global.TotalStarsAchivedWorld5 + "/" + (totalLevels[i] * 3);
                }
                //btns[i].transform.GetChild(3).GetChild(1).transform.Find("StarsLabel").GetComponent<Text>().text = TotalStarsforworld(i) + "/" + (totalLevels[i] * 3);//TotalStarsforworld
                btns[i].transform.GetChild(4).transform.Find("count").GetComponent<Text>().text = LevelSelectionHandler.LevelsUnlocked(i)
                            + "/" + totalLevels[i];
            }
            else
            {
                btns[i].transform.GetChild(1).gameObject.SetActive(true);
                btns[i].transform.GetChild(3).gameObject.SetActive(false);
                btns[i].transform.GetChild(4).gameObject.SetActive(false);
            }

        }
    }




    public static int worldSelected = 0;
    public void SelectWorld(Transform _tr)
    {
        if (totalLevels[_tr.GetSiblingIndex()] > 0)
        {
            SoundManager.PlaySFX("Click2");
            worldname_levelspanel[_tr.GetSiblingIndex()].GetComponent<LocalizedText>().doupdatetext("WorldsPanel_name" + _tr.GetSiblingIndex());
            UIPagesHandler._Instance.ShowPage(1 + _tr.GetSiblingIndex());
            worldSelected = _tr.GetSiblingIndex();
        }
    }

    public void spinPopupActivate() {
        spinPopUP.SetActive(true);
            }
    public void back()
    {
        SoundManager.PlaySFX("Click2");
        SceneManager.LoadScene("MainMenu");
    }
    public void getWorldLockText(Transform worldobj, string clue)
    {
        worldobj.GetChild(2).gameObject.SetActive(true);
        worldobj.GetChild(2).GetChild(0).GetComponent<Text>().text = clue;
    }

    public static int TotalStarsforworld(int worldNumb)
    {
        int TotalStars = 0;
        int[] lstars = PlayerPrefsX.GetIntArray("_levelStars" + worldNumb, 0, WorldSelectionHandler.totalLevels[worldNumb]);
        bool[] ULevels = PlayerPrefsX.GetBoolArray("_unlockedlevels" + worldNumb, false, WorldSelectionHandler.totalLevels[worldNumb]);

        for (int i = 0; i < lstars.Length; i++)
        {
            if (ULevels[i])
            {
                TotalStars = TotalStars + lstars[i];
                //Debug.Log("TotalStars " + TotalStars);
            }
        }
        return TotalStars;
    }
}
