using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPagesHandler : MonoBehaviour {

    public WorldSelectionHandler worldSelectionHandler;
    public GameObject WorldsContent;
    public GameObject[] pages;
    public static UIPagesHandler _Instance;
    public Text Totalcoins1;
    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        worldSelectionHandler.GenerateWorlds();
        ShowPage(0);
    }
    public void ShowPage(int _index)
    {
        foreach (GameObject l in pages)
            {
                l.SetActive(false);
            }
        if(_index ==0){
            WorldsContent.GetComponent<RectTransform>().anchoredPosition3D =new Vector3(325f,0f,0f);
        }
        if(_index>0&& _index<(pages.Length -1)){ 
            pages[_index].GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        }
        pages[_index].SetActive(true);        
    }
    public void updateTotalCoins()
    {
        Totalcoins1.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }
}
