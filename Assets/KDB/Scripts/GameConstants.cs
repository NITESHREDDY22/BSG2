
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    None,
    Noads
}

public static class GameConstants 
{
    static readonly string NoAdsKey = "NoAds";
    static readonly string NoAdsPurchased = "Purchased";
    static readonly string NoAdsNotPurchased = "NotPurchased";
    static readonly string CoinsKey = "coins";

    public static bool InternetConnected=false;   
   
    /// <summary>
    /// returns true if NoAds purchased
    /// </summary>
    public static bool GetNoAdsStatus
    {
        get
        {
            return string.Equals(PlayerPrefs.GetString(NoAdsKey, NoAdsNotPurchased), NoAdsPurchased);
        }        
    }

    public static void PurchaseSuccess(ItemType item)
    {
        switch(item)
        {

            case ItemType.Noads:
                PlayerPrefs.SetString(NoAdsKey, NoAdsPurchased);
                Coins += 5000;
                break;
        }
    }

    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt(CoinsKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(CoinsKey, value);
        }
    }

    public static bool targetLevelReached(int targetLevelNumber)
    {        
        if(targetLevelNumber<0)
            return false;

        int WorldNumber = getLastWorldUnlocked;
        return ((WorldNumber > 0) || (WorldNumber < 1 && (targetLevelNumber-1)<=getLastUnlcokedLevel)) ? true : false;        
    }

    public static int getLastUnlcokedLevel
    {
        get
        {
            List<bool> ULevels = PlayerPrefsX.GetBoolArray("_unlockedlevels" + getLastWorldUnlocked, false, WorldSelectionHandler.totalLevels[getLastWorldUnlocked]).ToList();
            return ULevels.FindLastIndex(x => x == true);
        }
    }

    public static int getLastWorldUnlocked
    {
        get
        {
            List<bool> unlockedWorlds = PlayerPrefsX.GetBoolArray("_unlockedWorlds", false, WorldSelectionHandler.totalLevels.Length).ToList();
            int index = unlockedWorlds.FindLastIndex(x => x == true);
            return index < 0 ? 0 : index;
        }
    }

}
