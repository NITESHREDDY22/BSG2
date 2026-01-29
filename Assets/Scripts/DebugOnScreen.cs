using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DebugOnScreen : MonoBehaviour
{
    private List<string> logs = new List<string>();
    private Vector2 scrollPos;
    private bool showLogs = false; // toggle state
    private bool scrollToBottom = false; // flag to move scroller

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        #if !CHEATS_ON
        gameObject.SetActive(false);
        #endif
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log || type == LogType.Exception || type == LogType.Error)
        {
            logs.Add(logString);
            scrollToBottom = true; // mark to scroll down next frame
        }
    }

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = Screen.width / 60;
        buttonStyle.fontStyle = FontStyle.Bold;

        // Toggle button
        if (GUI.Button(new Rect(Screen.width * .3f, Screen.height * .02f, Screen.width * .25f, Screen.height * .05f), showLogs ? "Hide Logs" : "Show Logs", buttonStyle))
        {
            showLogs = !showLogs;
        }

        if (!showLogs)
            return;

        // Log style
        GUIStyle logStyle = new GUIStyle(GUI.skin.label);
        logStyle.fontSize = Screen.width / 60;
        logStyle.normal.textColor = Color.white;

        GUILayout.BeginArea(new Rect(10, 60, Screen.width / 2, Screen.height * .5f), GUI.skin.box);

        // Clear button
        if (GUILayout.Button("Clear Logs", buttonStyle))
        {
            logs.Clear();
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        foreach (string log in logs)
            GUILayout.Label(log, logStyle);

        // Auto-scroll to bottom
        if (scrollToBottom)
        {
            scrollPos.y = Mathf.Infinity;
            scrollToBottom = false;
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void EditLocalScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}