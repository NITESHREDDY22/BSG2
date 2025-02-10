using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;



public enum CurrentProductType
{
    Consumable = 0,
    NonConsumable = 1,
    Subscription = 2
}

public class InappManager : MonoBehaviour, IStoreListener, IStoreController, IDetailedStoreListener
{

    [System.Serializable]
    public class InAppProductInfo
    {
        public string ProductId;
        public ItemType CurrentItemType;
        public CurrentProductType PurchaseType;
    }

    //Add all product info here
    //it will be mapped to the Inapp ID


    #region Contrller_Region
    IStoreController m_StoreController; // The Unity Purchasing system.
    IExtensionProvider m_StoreExtensionProvider;
    IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;
    #endregion

    //Your products IDs. They should match the ids of your products in your store. 
    #region Product Info
    public List<InAppProductInfo> inAppProductInfos;
    public Dictionary<string, ItemType> ProductIdTypeKeyPair = new Dictionary<string, ItemType>();
    public Dictionary<ItemType, string> ProductTypeIdKeyPair = new Dictionary<ItemType, string>();
    public Dictionary<string, string> ProductIdPriceKeyPair = new Dictionary<string, string>();

    private bool isInitialized => m_StoreController != null;

    public ProductCollection products => throw new NotImplementedException();

    public static System.Action<ItemType> OnPurchasedProduct;

    public GameObject noAdsPopup;

    [SerializeField] int showPremiumLevelCheck;
    #endregion   

    public static InappManager Instance
    {
        get
        {
            return _instance;
        }
        private set { }
    }

    private static InappManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {      
        AdManager.OnConfigLoaded += OnConfigLoaded;
    }

    private void OnDisable()
    {       
        AdManager.OnConfigLoaded -= OnConfigLoaded;
    }

    private void OnConfigLoaded(GameConfig config)
    {
        showPremiumLevelCheck = config.showPremiumLevelCheck;
        showPremiumLevelCheck = showPremiumLevelCheck < 1 ? -1 : showPremiumLevelCheck;
    }
    async void Start()
    {
        Debug.Log(" Initializing Unity Gaming Services...");
        // Initialize Unity Gaming Services (UGS)
        await UnityServices.InitializeAsync();
        Debug.Log(" Unity Gaming Services Initialized Successfully!");

        InitializePurchasing();
    }

    private void InitializePurchasing()
    {
        if (isInitialized)
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //Add products that will be purchasable and indicate its type.        
        foreach (var item in inAppProductInfos)
        {
            int index = (int)item.PurchaseType;
            builder.AddProduct(item.ProductId, (ProductType)System.Enum.ToObject(typeof(ProductType), index));
            ProductIdTypeKeyPair.Add(item.ProductId, item.CurrentItemType);
            ProductTypeIdKeyPair.Add(item.CurrentItemType, item.ProductId);
        }
        UnityPurchasing.Initialize(this, builder);        
    }

    public void PurchaseItem(ItemType currentItem)
    {
        if (ProductTypeIdKeyPair.TryGetValue(currentItem, out string prodID))
        {
            if (!string.IsNullOrEmpty(prodID))
            {
                Debug.LogError("In app id has been assigned" + prodID);
                m_StoreController.InitiatePurchase(prodID);
            }
            else
            {
                Debug.LogError("In app id hasn't been assigned" + prodID);
            }
        }

    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.LogError("In-App Purchasing successfully initialized");
        m_StoreController = controller;
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        if (m_StoreController.products != null)
        {
            foreach (var item in m_StoreController.products.all)
            {
                ProductIdPriceKeyPair.Add(item.definition.id, item.metadata.localizedPriceString);
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        if (ProductIdTypeKeyPair.TryGetValue(product.definition.id, out ItemType item))
        {
            OnPurchasedProduct?.Invoke(item);
            GameConstants.PurchaseSuccess(item);
        }
        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public string GetProductCurrency(ItemType item)
    {
        if (ProductTypeIdKeyPair.TryGetValue(item, out string prodId))
        {
            if (ProductIdPriceKeyPair.TryGetValue(prodId, out string price))
            {
                return price;
            }
        }
        return "Buy";
    }

    public void RestorePurchase()
    {
        if (!isInitialized)
            return;
        m_GooglePlayStoreExtensions.RestoreTransactions(OnRestore);

    }

    void OnRestore(bool success, string str)
    {
        var restoreMessage = "";
        if (success)
        {
            // This does not mean anything was restored,
            // merely that the restoration process succeeded.
            foreach (var item in ProductIdTypeKeyPair)
            {
                GameConstants.PurchaseSuccess(item.Value);
            }
            restoreMessage = "Restore Successful";
        }
        else
        {
            // Restoration failed.
            restoreMessage = "Restore Failed";
        }
        Debug.Log(restoreMessage);
    }

    public void InitiatePurchase(Product product, string payload)
    {
        throw new NotImplementedException();
    }

    public void InitiatePurchase(string productId, string payload)
    {
        throw new NotImplementedException();
    }

    public void InitiatePurchase(Product product)
    {
        throw new NotImplementedException();
    }

    public void InitiatePurchase(string productId)
    {
        throw new NotImplementedException();
    }

    public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason> failCallback)
    {
        throw new NotImplementedException();
    }

    public void ConfirmPendingPurchase(Product product)
    {
        throw new NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason, string> failCallback)
    {
        throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        throw new NotImplementedException();
    }


    public bool canProceedToNextLevel => (GameConstants.GetNoAdsStatus ||
        (!GameConstants.GetNoAdsStatus &&  !GameConstants.targetLevelReached(showPremiumLevelCheck)));

    public void CheckPremiumPopup()
    {
        if (canProceedToNextLevel)
            return;

        if (noAdsPopup)
            noAdsPopup.SetActive(!GameConstants.GetNoAdsStatus);
    }

    public void CloseNoAdsPopup()
    {
        if (noAdsPopup)
            noAdsPopup.SetActive(false);
    }

}

//UseCase
#region INAPP REGION
/*
public void PurchaseProduct(GlobalInappConfig.ItemType currentItem, Action<GlobalInappConfig.ItemType> purchaseCallBack = null)
{
    Debug.LogError("Exists in Inapp :: " + currentItem.ToString());
    inappManager.PurchaseItem(currentItem);
    this.purchaseCallBack = purchaseCallBack;
}
public void ValidatePurchase(GlobalInappConfig.ItemType item)
{
    savedData.ValidatePurchase(item);
    purchaseCallBack?.Invoke(item);
}
*/
#endregion