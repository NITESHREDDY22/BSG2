using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;
using System.Collections;

public class TutorialOverlay : MonoBehaviour
{
    public static TutorialOverlay Instance;

    [Header("UI")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button okButton;

    [Header("Description")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Settings")]
    [SerializeField] private bool showOnlyOnce = true;
    [SerializeField] private bool pauseGame = true;

    [Header("Tutorial Steps")]
    [SerializeField] private TutorialStep[] tutorialSteps;

    [Header("Finish UI")]
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Button rewatchButton;
    [SerializeField] private Button okayButton;

    public static event Action<bool> OnTutorialClosed;

    public enum VideoSourceType
    {
        Local,
        Stream
    }

    [Serializable]
    public class TutorialStep
    {
        public double pauseAtTime;

        [TextArea]
        public string description;

        public float resumeAfter = 2f;

        [HideInInspector]
        public bool triggered;
    }

    private const string PREF_KEY = "TutorialShown";

    private bool isShowing;
    private bool isShowingDescription;

    private Coroutine tutorialStepCoroutine;

    private VideoSourceType currentSourceType;
    private string currentVideoPath;
    private bool isTutorialPlaying;

    private void Awake()
    {
        Instance = this;

        tutorialPanel.SetActive(false);

        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        okButton.onClick.AddListener(() =>
        {
            Debug.Log("[Tutorial] OK Button Pressed");

            CloseTutorial(true);
        });

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnVideoCompleted;
        videoPlayer.errorReceived += OnVideoError;

        finishPanel.SetActive(false);

        rewatchButton.onClick.AddListener(OnRewatchClicked);
        okayButton.onClick.AddListener(OnOkayClicked);
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnPrepared;
        videoPlayer.loopPointReached -= OnVideoCompleted;
        videoPlayer.errorReceived -= OnVideoError;
    }

    public void ShowTutorial(VideoSourceType sourceType, string pathOrUrl)
    {
        if (isShowing)
        {
            Debug.Log("[Tutorial] Already Showing");
            return;
        }

        if (showOnlyOnce && PlayerPrefs.GetInt(PREF_KEY, 0) == 1)
        {
            Debug.Log("[Tutorial] Already Completed Earlier");
            return;
        }

        foreach (var step in tutorialSteps)
        {
            step.triggered = false;
        }

        isShowingDescription = false;

        StartCoroutine(PrepareAndPlay(sourceType, pathOrUrl));
        currentSourceType = sourceType;
        currentVideoPath = pathOrUrl;
    }

    private IEnumerator PrepareAndPlay(VideoSourceType sourceType, string pathOrUrl)
    {
        isShowing = true;

        tutorialPanel.SetActive(true);

        if (pauseGame)
            Time.timeScale = 0;

        videoPlayer.Stop();

        videoPlayer.source = VideoSource.Url;

        if (sourceType == VideoSourceType.Local)
        {
            videoPlayer.url = System.IO.Path.Combine(
                Application.streamingAssetsPath,
                pathOrUrl);

            Debug.Log("[Tutorial] Local Video : " + videoPlayer.url);
        }
        else
        {
            videoPlayer.url = pathOrUrl;

            Debug.Log("[Tutorial] Stream Video : " + videoPlayer.url);
        }

        Debug.Log("[Tutorial] Preparing Video");

        videoPlayer.Prepare();

        yield break;
    }

    private void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("[Tutorial] Prepared Successfully");
        Debug.Log("[Tutorial] Length : " + vp.length + " seconds");
        

        vp.Play();

        Debug.Log("[Tutorial] Video Started");
    }

    private void OnVideoCompleted(VideoPlayer vp)
    {
        if (!isTutorialPlaying)
            return;
        Debug.Log("[Tutorial] VIDEO FINISHED");

        //CloseTutorial(false);
        ShowFinishPanel();
    }
    private void ShowFinishPanel()
    {
        Debug.Log("[Tutorial] Video Finished");

        videoPlayer.Stop();

        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        finishPanel.SetActive(true);
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("[Tutorial] Video Error : " + message);
    }

    private void Update()
    {
        if (!isShowing)
            return;

        if (!videoPlayer.isPlaying)
            return;

        if (isShowingDescription)
            return;

        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if (tutorialSteps[i].triggered)
                continue;

            if (videoPlayer.time >= tutorialSteps[i].pauseAtTime)
            {
                tutorialSteps[i].triggered = true;

                tutorialStepCoroutine =
                    StartCoroutine(ShowTutorialStep(tutorialSteps[i]));

                break;
            }
        }
    }

    private IEnumerator ShowTutorialStep(TutorialStep step)
    {
        isTutorialPlaying = true;
        isShowingDescription = true;

        Debug.Log("[Tutorial] Pause at : " + step.pauseAtTime);

        videoPlayer.Pause();

        Debug.Log("[Tutorial] Video Paused");

        descriptionPanel.SetActive(true);

        descriptionText.text = step.description;

        Debug.Log("[Tutorial] Message : " + step.description);

        yield return new WaitForSecondsRealtime(step.resumeAfter);

        descriptionPanel.SetActive(false);

        if (tutorialPanel.activeInHierarchy)
        {
            videoPlayer.Play();

            Debug.Log("[Tutorial] Video Resumed");
        }

        isShowingDescription = false;
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(PREF_KEY);

        Debug.Log("[Tutorial] Reset Completed");
    }

    private void CloseTutorial(bool closedByButton)
    {
        if (!isShowing)
            return;

        isShowing = false;
        isTutorialPlaying = false;

        if (tutorialStepCoroutine != null)
        {
            StopCoroutine(tutorialStepCoroutine);

            tutorialStepCoroutine = null;
        }

        videoPlayer.Stop();

        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        tutorialPanel.SetActive(false);

        if (pauseGame)
            Time.timeScale = 1;

        if (showOnlyOnce)
        {
            PlayerPrefs.SetInt(PREF_KEY, 1);
            PlayerPrefs.Save();
        }

        Debug.Log("[Tutorial] Closed | By Button : " + closedByButton);

        OnTutorialClosed?.Invoke(closedByButton);
        finishPanel.SetActive(false);
    }

    [ContextMenu("Test Local Tutorial")]
    private void TestLocalTutorial()
    {
        ResetTutorial();

        ShowTutorial(
            VideoSourceType.Local,
            "BSG2TutVideo.mp4");
    }

    private void OnRewatchClicked()
    {
        Debug.Log("[Tutorial] Rewatch");

        finishPanel.SetActive(false);

        foreach (var step in tutorialSteps)
            step.triggered = false;

        isShowingDescription = false;

        StartCoroutine(
            PrepareAndPlay(
                currentSourceType,
                currentVideoPath));
    }
    private void OnOkayClicked()
    {
        Debug.Log("[Tutorial] Tutorial Closed");

        CloseTutorial(true);
    }
}
