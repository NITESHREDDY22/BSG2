using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAdData", menuName = "Ads/Ad Data")]
public class AdData : ScriptableObject
{
    [System.Serializable]
    public class AdItem
    {
        public string name;
        public Sprite image;
        public string url;
    }

    [Header("Banner Ad Sets")]
    public List<AdItem> bannerListA;
    public List<AdItem> bannerListB;

    [Header("Interstitial Ad Sets")]
    public List<AdItem> interstitialListA;
    public List<AdItem> interstitialListB;
}