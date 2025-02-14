using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NoAdsButton : MonoBehaviour
{
    [SerializeField] Button noAdsButton;
    [SerializeField] RectTransform rectTransform;
    private void OnEnable()
    {
        noAdsButton.onClick.RemoveAllListeners();
        noAdsButton.onClick.AddListener(()=> ShowNoadsPopUp());
        InappManager.OnPurchasedProduct += OnPurchaseSuccess;
    }

    void ShowNoadsPopUp()
    {
        if (InappManager.Instance != null)
        {
            InappManager.Instance.CheckPremiumPopup();
            if (FirebaseEvents.instance != null)
            {
                FirebaseEvents.instance.LogFirebaseEvent("AdBlockerbuttonClicked", "success");
            }
        }
    }

    private void OnDisable()
    {
        InappManager.OnPurchasedProduct -= OnPurchaseSuccess;

    }

    public void Reset()
    {

    }
    void OnPurchaseSuccess(ItemType itemType)
    {
        if(itemType==ItemType.Noads)
        gameObject.SetActive(false);
    }
}
