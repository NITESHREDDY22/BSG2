using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizatioWatchToContinue : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<Text>().text = getText();

    }

    // Update is called once per frame
    void Update()
    {

    }

    string getText()
    {
        string txt = "Watch video to\n continue Playing..";
        
        if (Application.systemLanguage == SystemLanguage.French)
        {
            GetComponent<Text>().fontSize = 25;
            txt = "Regarder vidéo\n pour continuer à jouer ..";
        }
        else if (Application.systemLanguage == SystemLanguage.Arabic)
        {
            GetComponent<Text>().fontSize = 25;
            GetComponent<Text>().fontStyle = FontStyle.Bold;
            txt = " شاهد  الفيديولمواصلة اللعب";
        }
        else if (Application.systemLanguage == SystemLanguage.Dutch)
        {
            GetComponent<Text>().fontSize = 20;
            txt = "Bekijk video\n om door te gaan met spelen ..";
        }
        else if (Application.systemLanguage == SystemLanguage.German)
        {
            txt = "Schau Video\n weiter spielen ..";
        }
        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            GetComponent<Text>().fontSize = 23;
            txt = "Guarda un video\n per continuare a giocare ..";
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            txt = "ビデオを見る\n 遊び続けるために..";
        }
        else if (Application.systemLanguage == SystemLanguage.Polish)
        {
            GetComponent<Text>().fontSize = 25;
            txt = "Obejrzyj wideo\n aby kontynuować grę..";
        }
        else if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            GetComponent<Text>().fontSize = 25;
            txt = "Assista vídeo\n para continuar jogando..";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            GetComponent<Text>().fontSize = 25;
            GetComponent<Text>().fontStyle = FontStyle.Bold;
            txt = "Смотреть видео на\n продолжить играть ..";
        }
        else if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            txt = "Ver video para\n sigue jugando ..";
        }
        else if (Application.systemLanguage == SystemLanguage.Turkish)
        {
            txt = "İçin Video İzle\n oynamaya devam et..";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            txt = "观看视频 继续玩..";
        }
        else if (Application.systemLanguage == SystemLanguage.Vietnamese)
        {
            txt = "Xem video để\n tiếp tục chơi..";
        }

        
        return txt;
    }
}
