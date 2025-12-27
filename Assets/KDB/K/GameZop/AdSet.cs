using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAdSet", menuName = "Ads/Ad Set")]
public class AdSet : ScriptableObject
{
    [System.Serializable]
    public class AdItem
    {
        public string adName;
        public Sprite adImage;
        public string targetUrl;
    }

    public List<AdItem> listA;
    public List<AdItem> listB;
}