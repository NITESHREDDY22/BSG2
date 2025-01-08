using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextLocalization : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = getSkipLevelText();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    string getSkipLevelText()
    {

          string txt = "Watch video to\n skip level";
        
        if (Application.systemLanguage == SystemLanguage.French)
        {
            GetComponent<Text>().fontSize = 28;
            txt = "Regarder la vidéo pour\nSauter niveau";
        }
        else if (Application.systemLanguage == SystemLanguage.Arabic)
        {
            GetComponent<Text>().fontStyle = FontStyle.Bold;
            txt = "شاهد الفيديو لتخطي المستوى";
        }
        else if (Application.systemLanguage == SystemLanguage.Dutch)
        {
            txt = "Bekijk video naar\nsla level over";
        }
        else if (Application.systemLanguage == SystemLanguage.German)
        {
            GetComponent<Text>().fontSize = 22;
            txt = "Sehen Sie sich das Video an\num die Ebene zu überspringen";
        }
        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            txt = "Guarda il video \nper saltare il livello";
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            GetComponent<Text>().fontSize = 23;
            txt = "レベルをスキップするために\nビデオを見る";
        }
        else if (Application.systemLanguage == SystemLanguage.Polish)
        {
            txt = "Obejrzyj wideo, \naby pominąć poziom";
        }
        else if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            txt = "Assista ao vídeo \npara pular o nível";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            GetComponent<Text>().fontSize = 22;
            GetComponent<Text>().fontStyle = FontStyle.Bold;
            txt = "Смотреть видео,\nчтобы пропустить уровень";
        }
        else if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            txt = "Ver video para saltar de nivel";
        }
        else if (Application.systemLanguage == SystemLanguage.Turkish)
        {
            txt = "Seviyeyi atlamak için\nvideoyu izleyin";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            txt = "观看视频以跳过级别";
        }
        else if (Application.systemLanguage == SystemLanguage.Vietnamese)
        {
            txt = "Xem video để \n bỏ qua cấp độ hiện tại";
        }
        return txt;
    }
}
