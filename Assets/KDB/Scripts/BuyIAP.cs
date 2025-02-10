using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyIAP : MonoBehaviour
{
    [SerializeField] ItemType currentItem;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button purchaseButton,cancelButton;

    private void OnEnable()
    {
        priceText.SetText(InappManager.Instance.GetProductCurrency(currentItem));
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => ClosePopup());

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => Butproduct());
        InappManager.OnPurchasedProduct += PostPurchase;
    }

    void Butproduct()
    {
        InappManager.Instance.PurchaseItem(currentItem);
    }

    void PostPurchase(ItemType itemType)
    {
        if(itemType==currentItem)
        {
            Debug.LogError("purchase success");
            ClosePopup();
        }
    }
    private void OnDisable()
    {
        InappManager.OnPurchasedProduct -= PostPurchase;

    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
