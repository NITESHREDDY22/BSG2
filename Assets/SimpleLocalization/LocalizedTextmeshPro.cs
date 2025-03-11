using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextmeshPro : MonoBehaviour
    {
        public string LocalizationKey;

        public void OnEnable()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDisable()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        public void Localize()
        {
            if (GetComponent<TextMeshProUGUI>())
            {             
                string textToBeAssigned= LocalizationManager.Localize(LocalizationKey); ;
                GetComponent<TextMeshProUGUI>().text = textToBeAssigned;
            }
        }
    }
}