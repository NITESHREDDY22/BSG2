using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AdsConfig : ScriptableObject
{
    public List<AdConfig> AdConfigContainer;
}
public enum NetworkType
{
    AdMob,
    LevelPlay
}
public enum ActiveStatus
{
    Active,
    Deactive
}
[System.Serializable]
public class AdUnitConfig
{   
    public AdType AdType;
    public string AdUnitId;
    public ActiveStatus ActiveStatus;
}
[System.Serializable]
public class AdConfig
{
    public NetworkType NetworkType;
    public string AppKey;
    public List<AdUnitConfig> adConfigs;
}
