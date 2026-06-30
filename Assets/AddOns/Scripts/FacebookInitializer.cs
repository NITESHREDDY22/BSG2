using Facebook.Unity;
using UnityEngine;
using System;

public class FacebookInitializer : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("[Facebook] Awake");

        if (!FB.IsInitialized)
        {
            Debug.Log("[Facebook] SDK not initialized. Initializing...");
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            Debug.Log("[Facebook] SDK already initialized.");

            FB.ActivateApp();
            Debug.Log("[Facebook] ActivateApp() called.");

            PrintLoginStatus();
        }
    }

    private void OnInitComplete()
    {
        Debug.Log("[Facebook] OnInitComplete() called.");

        if (!FB.IsInitialized)
        {
            Debug.LogError("[Facebook] SDK initialization FAILED.");
            return;
        }

        Debug.Log("[Facebook] SDK initialization SUCCESS.");

        try
        {
            FB.ActivateApp();
            Debug.Log("[Facebook] ActivateApp() sent successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("[Facebook] ActivateApp Exception: " + ex);
        }

        PrintLoginStatus();
    }

    private void PrintLoginStatus()
    {
        Debug.Log("========================================");
        Debug.Log("[Facebook] Login Status");

        Debug.Log("[Facebook] FB.IsInitialized : " + FB.IsInitialized);
        Debug.Log("[Facebook] FB.IsLoggedIn    : " + FB.IsLoggedIn);
        Debug.Log("[Facebook] App ID: " + FB.AppId);
        Debug.Log("[Facebook] Client Token: " + FB.ClientToken);

        if (AccessToken.CurrentAccessToken != null)
        {
            Debug.Log("[Facebook] User ID       : " + AccessToken.CurrentAccessToken.UserId);
            Debug.Log("[Facebook] Token         : " + AccessToken.CurrentAccessToken.TokenString);
            Debug.Log("[Facebook] Expiration    : " + AccessToken.CurrentAccessToken.ExpirationTime);
        }
        else
        {
            Debug.Log("[Facebook] AccessToken is NULL.");
        }

        Debug.Log("========================================");
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("[Facebook] OnHideUnity : " + isGameShown);

        Time.timeScale = isGameShown ? 1f : 0f;
    }
}