using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Asset usage example.
	/// </summary>
	public class LanguageManager : MonoBehaviour
	{

		public void Awake()
		{
			LocalizationManager.Read();
           SystemLanguage T = Application.systemLanguage;            
           //T = SystemLanguage.Spanish;
            switch (T)
			{
				case SystemLanguage.German:
					LocalizationManager.Language = "German";
					break;
                case SystemLanguage.Spanish:
                    LocalizationManager.Language = "Spanish";
                    break;
                case SystemLanguage.Russian:
					LocalizationManager.Language = "Russian";
					break;
                case SystemLanguage.Turkish:
                    LocalizationManager.Language = "Turkish";
                    break;
                case SystemLanguage.Portuguese:
                    LocalizationManager.Language = "Portuguese";
                    break;
                case SystemLanguage.Italian:
                    LocalizationManager.Language = "Italian";
                    break;
                case SystemLanguage.Chinese:
                    LocalizationManager.Language = "Chinese";
                    break;
                case SystemLanguage.Japanese:
                    LocalizationManager.Language = "Japanese";
                    break;
                case SystemLanguage.Polish:
                    LocalizationManager.Language = "Polish";
                    break;
                case SystemLanguage.Arabic:
                    LocalizationManager.Language = "Arabic";
                    break;
                case SystemLanguage.Dutch:
                    LocalizationManager.Language = "Dutch";
                    break;
                case SystemLanguage.French:
                    LocalizationManager.Language = "French";
                    break;
                default:
					LocalizationManager.Language = "English";
					break;
			}
            
        }

		public void SetLocalization(string localization)
		{
			LocalizationManager.Language = localization;
		}
	}
}