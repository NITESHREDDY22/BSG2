using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyIAP : MonoBehaviour
{
    public string source = "store";
    [SerializeField] ItemType currentItem;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Text priceText1;

    [SerializeField] Button purchaseButton,cancelButton;

    private void OnEnable()
    {
        if(priceText)priceText.SetText(InappManager.Instance.GetProductCurrency(currentItem));
        priceText1.text = InappManager.Instance.GetProductCurrency(currentItem);
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => ClosePopup());

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => Butproduct());
        InappManager.OnPurchasedProduct += PostPurchase;
    }

    void Butproduct()
    {
        InappManager.Instance.PurchaseItem(currentItem);

        Firebase.Analytics.FirebaseAnalytics.LogEvent("Buy_product_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay+
        "_currentItem_"+currentItem.ToString()+"_source_"+source);
        /* if (source == "notEnoughCoins")
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("NotEnoughCoins_buyCoins_success_" + "W" + WorldSelectionHandler.worldSelected + "_L" + Global.CurrentLeveltoPlay +
        "_currentItem_" + currentItem.ToString() + "_source_" + source);
        } */
        Debug.Log($"[Butproduct] source {source}");

        if (FirebaseEvents.instance != null)
        {
            FirebaseEvents.instance.LogFirebaseEvent("AdBlockerPurchaseClicked", "success");
        }
        
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
