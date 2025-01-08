using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GifAdsManager : MonoBehaviour
    {
        [Header("Ads List")]
        public GIFCTA_adObj[] _adObjs;

        [Header("DirectADs")]
        public GIFCTA_DemandadObj _adTest;

        public static GifAdsManager Instance;
        private void Awake()//ad intialise
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            if (_adObjs.Length != 0)
            {
                for(int i= 0; i < _adObjs.Length; i++)
                {
                    //UnityMainThreadDispatcher.Instance().Enqueue(GetADText(_adObjs[i]));
                    StartCoroutine(GetADText(_adObjs[i]));
                }
            }

            //directadTest
            //UnityMainThreadDispatcher.Instance().Enqueue(GetDirectAd(_adTest));
            //StartCoroutine(GetDirectAd(_adTest));

        }

        IEnumerator GetADText(GIFCTA_adObj CTA_adObj)
        {
            UnityWebRequest www = new UnityWebRequest(CTA_adObj._hostedJsonUrl);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                CTA_adObj._GIFCTAList = JsonUtility.FromJson<GIFCTAList>(www.downloadHandler.text);
                // Or retrieve results as binary data
                for(int i = 0; i < CTA_adObj._GIFCTAList._list.Count; i++)
                {
                byte[] results = www.downloadHandler.data;
                //UnityMainThreadDispatcher.Instance().Enqueue(LoadAdObjs(CTA_adObj));
                    if (i == 0)
                    {
                        CTA_adObj._finalAdBase = new AdBase();
                        CTA_adObj._finalAdBase = CTA_adObj._GIFCTAList._list[0];
                        StartCoroutine(LoadAdObjs(CTA_adObj, true));
                    }
                    else
                    {
                        StartCoroutine(LoadAdObjs(CTA_adObj, false));
                    }
                }
                
        }
    }

        public IEnumerator GetDirectAd(GIFCTA_DemandadObj _refCTA_adObj)
        {
            if (_refCTA_adObj._finalAdBase.GIFUrl != "")
            {
                if (!Regex.IsMatch(_refCTA_adObj._finalAdBase.GIFUrl, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                {
                    throw new ArgumentException("Wrong URL!");
                }

                var www = new WWW(_refCTA_adObj._finalAdBase.GIFUrl);
                while (!www.isDone)
                {
                    //ProgressFill.fillAmount = www.progress;
                    yield return null;
                }
                if (www.error != null)
                {
                    throw new Exception(www.error);
                }
                var iterator = Gif.DecodeIterator(www.bytes);
                var iteratorSize = Gif.GetDecodeIteratorSize(www.bytes);
                var frames = new List<GifFrame>();

                foreach (var frame in iterator)
                {
                    frames.Add(frame);
                    yield return null;
                }
                _refCTA_adObj._gifloaded = new Gif(frames);
            //test
                if (_refCTA_adObj._Adtype != ADType.Interstitial && SceneManager.GetActiveScene().buildIndex > 2)
                {
                    ShowAdDemand(_refCTA_adObj);
                }
            }
        }
    public void ShowAdDemand(GIFCTA_DemandadObj _refCTA_adObj)
    {
        
        if (_refCTA_adObj._gifloaded != null)
        {
            if (_refCTA_adObj.RootTint != null)
            {
                _refCTA_adObj.RootTint.SetActive(false);
            }
            _refCTA_adObj._AD_Image.gameObject.SetActive(true);
            if (_refCTA_adObj._AnimImg != null)
            {
                _refCTA_adObj._AnimImg.Play(_refCTA_adObj._gifloaded);
            }
            if (_refCTA_adObj.Cgrp != null)
            {
                _refCTA_adObj.Cgrp.alpha = 1.0f;
            }
            if (_refCTA_adObj._AD_Image != null)
            {
                _refCTA_adObj._AD_Image.sizeDelta = new Vector2(_refCTA_adObj._gifloaded.Frames[0].Texture.width, _refCTA_adObj._gifloaded.Frames[0].Texture.height);
            }
            if (_refCTA_adObj._CTABtn != null)
            {
                _refCTA_adObj._CTABtn.onClick.RemoveAllListeners();
                _refCTA_adObj._CTABtn.onClick.AddListener(() => CTAFuncDemand(_refCTA_adObj._finalAdBase.CTA_android_Url, _refCTA_adObj._finalAdBase.CTA_ios_Url, _refCTA_adObj));
            }
            if (_refCTA_adObj._CloseBtn != null)
            {
                _refCTA_adObj._CloseBtn.onClick.AddListener(() =>HideAdDemand(_refCTA_adObj));
            }
        }
    }

    public IEnumerator LoadAdObjs(GIFCTA_adObj _refCTA_adObj,bool loadnow)
        {
            if (_refCTA_adObj._finalAdBase.GIFUrl != "")
            {
                if (!Regex.IsMatch(_refCTA_adObj._finalAdBase.GIFUrl, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                {
                    throw new ArgumentException("Wrong URL!");
                }

                var www = new WWW(_refCTA_adObj._finalAdBase.GIFUrl);
                while (!www.isDone)
                {
                    //ProgressFill.fillAmount = www.progress;
                    yield return null;
                }
                if (www.error != null)
                {
                    throw new Exception(www.error);
                }
                var iterator = Gif.DecodeIterator(www.bytes);
                var iteratorSize = Gif.GetDecodeIteratorSize(www.bytes);
                var frames = new List<GifFrame>();

                foreach (var frame in iterator)
                {
                    frames.Add(frame);
                    yield return null;
                }
                _refCTA_adObj._gifloaded = new Gif(frames);
            //test
            if (loadnow)
            {
                if (_refCTA_adObj._Adtype != ADType.Interstitial && SceneManager.GetActiveScene().buildIndex == 1)
                {
                    ShowAd(_refCTA_adObj);
                }
            }
                
            }
        }

        public void ShowAd(GIFCTA_adObj _refCTA_adObj)
        {
        _refCTA_adObj.adcounter += 1;
        if(_refCTA_adObj.adcounter >= _refCTA_adObj._GIFCTAList._list.Count)
        {
            _refCTA_adObj.adcounter = 0;
        }else if (_refCTA_adObj.adcounter < 0 )
        {
            _refCTA_adObj.adcounter = 0;
        }
        _refCTA_adObj._finalAdBase = new AdBase();
        _refCTA_adObj._finalAdBase = _refCTA_adObj._GIFCTAList._list[_refCTA_adObj.adcounter];
        if (_refCTA_adObj._gifloaded != null) {
                if (_refCTA_adObj.RootTint != null)
                {
                    _refCTA_adObj.RootTint.SetActive(true);
                }
                _refCTA_adObj._AD_Image.gameObject.SetActive(true);
                if (_refCTA_adObj._AnimImg != null)
                {
                    _refCTA_adObj._AnimImg.Play(_refCTA_adObj._gifloaded);
                }
                if (_refCTA_adObj.Cgrp != null)
                {
                    _refCTA_adObj.Cgrp.alpha = 1.0f;
                }
                if (_refCTA_adObj._AD_Image != null)
                {
                    _refCTA_adObj._AD_Image.sizeDelta = new Vector2(_refCTA_adObj._gifloaded.Frames[0].Texture.width, _refCTA_adObj._gifloaded.Frames[0].Texture.height);
                }
                if (_refCTA_adObj._CTABtn != null)
                {
                    _refCTA_adObj._CTABtn.onClick.RemoveAllListeners();
                    _refCTA_adObj._CTABtn.onClick.AddListener(() => CTAFunc(_refCTA_adObj._finalAdBase.CTA_android_Url, _refCTA_adObj._finalAdBase.CTA_ios_Url, _refCTA_adObj));
                }
                if (_refCTA_adObj._CloseBtn != null)
                {
                    _refCTA_adObj._CloseBtn.onClick.AddListener(() => HideAD(_refCTA_adObj));
                }
            try
            {
                Analytics.CustomEvent(_refCTA_adObj._Adtype.ToString().ToLower() + "_gif_shown");
                FirebaseEvents.instance.LogFirebaseEvent(_refCTA_adObj._Adtype.ToString().ToLower() + "_gif_shown");
            }
            catch (Exception e)
            {
                //Debug.Log(e);
            }
        }
        }
    

        public void HideAD(GIFCTA_adObj _refCTA_adObj)
        {
            _refCTA_adObj._AD_Image.gameObject.SetActive(false);
            if (_refCTA_adObj.RootTint != null)
            {
                _refCTA_adObj.RootTint.SetActive(false);
            }
            if (_refCTA_adObj._Adtype == ADType.Interstitial)
            {
                //AdScript.oninterstitialclosedfunc();
            }
        }

        public void HideAdDemand(GIFCTA_DemandadObj _adObj)
        {
            _adObj._AD_Image.gameObject.SetActive(false);
            if (_adObj.RootTint != null)
            {
                _adObj.RootTint.SetActive(false);
            }
        }

        public bool InterstititalClosed(GIFCTA_adObj _refCTA_adObj)
        {
            if (_refCTA_adObj._Adtype == ADType.Interstitial)
            {
                if (_refCTA_adObj._AD_Image.gameObject.activeInHierarchy)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        public void CTAFunc(string androidurl,string iosurl, GIFCTA_adObj _ad)
        {
            #if UNITY_ANDROID || UNITY_EDITOR
              Application.OpenURL(androidurl);
#elif UNITY_IOS
              Application.OpenURL(iosurl);
#endif
        try
        {
            Analytics.CustomEvent(_ad._Adtype.ToString().ToLower() + "_gif_Clicked");
            FirebaseEvents.instance.LogFirebaseEvent(_ad._Adtype.ToString().ToLower() + "_gif_Clicked");
        }
        catch (Exception e)
        {
            //Debug.Log(e);
        }
    }

    public void CTAFuncDemand(string androidurl, string iosurl, GIFCTA_DemandadObj _Ad)
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL(androidurl);
#elif UNITY_IOS
              Application.OpenURL(iosurl);
#endif
        try
        {
            Analytics.CustomEvent(_Ad._Adtype.ToString().ToLower() + "_gif_Clicked");
            FirebaseEvents.instance.LogFirebaseEvent(_Ad._Adtype.ToString().ToLower() + "_gif_Clicked");
        }
        catch (Exception e)
        {
            //Debug.Log(e);
        }
    }
}

    [Serializable]
    public class AdBase
    {
        public string CTA_android_Url;
        public string CTA_ios_Url;
        public string GIFUrl;
        
    }

    [Serializable]
    public class GIFCTAList
    {
        public List<AdBase> _list = new List<AdBase>();
    }

    [Serializable]
    public class GIFCTA_adObj
    {
        public string name;//your name variable to edit
        public ADType _Adtype = ADType.Banner;
        public GIFCTAList _GIFCTAList = new GIFCTAList();
        public AdBase _finalAdBase;
        public AnimatedImage _AnimImg;
        public RectTransform _AD_Image;
        public GameObject RootTint;
        public CanvasGroup Cgrp;
        public Button _CloseBtn;
        public Gif _gifloaded;
        public Button _CTABtn;
        public string _hostedJsonUrl;
        public int adcounter = -1;
    }


    [Serializable]
    public class GIFCTA_DemandadObj
    {
        public ADType _Adtype = ADType.Banner;
        public AdBase _finalAdBase;
        public AnimatedImage _AnimImg;
        public RectTransform _AD_Image;
        public GameObject RootTint;
        public CanvasGroup Cgrp;
        public Button _CloseBtn;
        public Gif _gifloaded;
        public Button _CTABtn;
    }

    public enum ADType
    {
        Banner,
        Native,
        Interstitial
    }


/*UnityWebRequest www = UnityWebRequest.Get(_refCTA_adObj._finalAdBase.GIFUrl);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    byte[] results = null;
                    while (www.downloadProgress >= 1.0f)
                    {
                        results = www.downloadHandler.data;
                        yield return null;
                    }
                    Debug.Log("Doneeee");

                    var iterator = Gif.DecodeIterator(results);
                    var iteratorSize = Gif.GetDecodeIteratorSize(results);
                    var frames = new List<GifFrame>();

                    foreach (var frame in iterator)
                    {
                        frames.Add(frame);
                        yield return null;
                    }
                    var gif = new Gif(frames);
                    if (_refCTA_adObj._AnimImg != null)
                    {
                        _refCTA_adObj._AnimImg.Play(gif);
                    }
                    if (_refCTA_adObj.Cgrp != null)
                    {
                        _refCTA_adObj.Cgrp.alpha = 1.0f;
                    }
                    if (_refCTA_adObj._AD_Image != null)
                    {
                        _refCTA_adObj._AD_Image.sizeDelta = new Vector2(gif.Frames[0].Texture.width, gif.Frames[0].Texture.height);
                    }
                }*/
/*
{
"_list": [
{
"CTA_android_Url": "https://play.google.com/store/apps/details?id=com.rovio.tnt",
"CTA_ios_Url": "https://play.google.com/store/apps/details?id=com.rovio.tnt",
"GIFUrl": "https://media.giphy.com/media/l41YnNM4fAbBu3tHW/giphy.gif"
},
{
"CTA_android_Url": "https://play.google.com/store/apps/details?id=com.playrix.fishdomdd.gplay",
"CTA_ios_Url": "https://play.google.com/store/apps/details?id=com.playrix.fishdomdd.gplay",
"GIFUrl": "https://media.giphy.com/media/pyYCrkZ8FsbtFCoXCO/giphy.gif"
}
]
}*/
