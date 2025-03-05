using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Assets.SimpleLocalization
{
    //[RequireComponent(typeof(TextMeshProUGUI))]
    public class IgnoreNumberLocalization : MonoBehaviour
    {
        [Tooltip("for SELECTED")]
        public string LocalizationKey;
        [Tooltip("for SELECT")]
        public string LocalizationKey2;

        [SerializeField] Button currentButton;

        public void OnEnable()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
            currentButton.onClick.AddListener(()=>
            {
                DelayCall();
            });
        }

        public void OnDisable()
        {
            LocalizationManager.LocalizationChanged -= Localize;
            currentButton.onClick.RemoveListener(() =>
            {
                DelayCall();
            });
        }

        private void DelayCall()
        {
            CancelInvoke(nameof(Localize));
            Invoke(nameof(Localize),0.25f);

        }

        private void Localize()
        {
            string getCurrentText = string.Empty;

            if (GetComponent<Text>())
            {
                getCurrentText = GetComponent<Text>().text;
            }
            if (GetComponent<TextMeshProUGUI>())
            {
                getCurrentText = GetComponent<TextMeshProUGUI>().text;
            }

            bool containsNumbers = getCurrentText.Any(char.IsDigit);

            if (containsNumbers)
                return;

            string targetLocalizeKey = getCurrentText.ToUpper().Contains("SELECTED") ? LocalizationKey : LocalizationKey2;

            if (GetComponent<Text>())
            {
                GetComponent<Text>().text = LocalizationManager.Localize(targetLocalizeKey);
            }

            if (GetComponent<TextMeshProUGUI>())
            {
                GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(targetLocalizeKey);

            }
        }
    }
}
