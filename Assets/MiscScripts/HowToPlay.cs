using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour {
	public Text howToPlay;
	// Use this for initialization
	void Start () {
		howToPlay.text = 	gettext ();

	}

	string gettext(){
		string txt = "How to Play ";

		if (Application.systemLanguage == SystemLanguage.French) {
			txt = "Comment jouer";
		}else if (Application.systemLanguage == SystemLanguage.Arabic) {
			txt = "كيف ألعب";		
		}else if (Application.systemLanguage == SystemLanguage.Dutch) {
			txt = "Hoe te spelen";		
		}else if (Application.systemLanguage == SystemLanguage.German) {
			txt = "Spielanleitung";		
		}else if (Application.systemLanguage == SystemLanguage.Italian) {
			txt = "Come giocare";		
		}else if (Application.systemLanguage == SystemLanguage.Japanese) {
			txt = "遊び方";		
		}else if (Application.systemLanguage == SystemLanguage.Polish) {
			txt = "Jak grać";		
		}else if (Application.systemLanguage == SystemLanguage.Portuguese) {
			txt = "Como jogar";		
		}else if (Application.systemLanguage == SystemLanguage.Russian) {
			txt = "Как играть";		
		}else if (Application.systemLanguage == SystemLanguage.Spanish) {
			txt = "Cómo jugar";		
		}else if (Application.systemLanguage == SystemLanguage.Turkish) {
			txt = "Seviye Temizlendi";		
		}else if (Application.systemLanguage == SystemLanguage.Chinese) {
			txt = "怎么玩";		
		}
        else if (Application.systemLanguage == SystemLanguage.Vietnamese)
        {
            txt = "Cách chơi";
        }
        return txt;	
	}
}
