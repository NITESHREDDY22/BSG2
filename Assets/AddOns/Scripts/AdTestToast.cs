using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class AdTestToast : MonoBehaviour
{
    public static AdTestToast Instance;
    public TextMeshProUGUI toastText;
    public GameObject toastPanel;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        toastPanel.SetActive(false);
    }

    public void Show(string message)
    {
#if PRODUCTION_BUILD_OFF
        // Always ensure UI calls happen on the main thread
        try
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(DoToast(message));
        }
        catch (Exception e)
        {
            Debug.LogError($"AdTestToast Error: {e}");
        }
#else
        toastPanel.SetActive(false);
#endif

    }

    private IEnumerator DoToast(string message)
    {
        toastText.text = message;
        toastPanel.SetActive(true);
        yield return new WaitForSeconds(3f); // Duration
        toastPanel.SetActive(false);
    }
}