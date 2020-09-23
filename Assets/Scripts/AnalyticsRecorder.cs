using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// A class to record data into UnityAnalytics when custom Events are fired.
public class AnalyticsRecorder : MonoBehaviour
{
    // Recyclable dictionary for custom events
    private static Dictionary<string, object> dict = new Dictionary<string, object>();

    private void Awake()
    {
        GameManager.Instance.levelStartEvent += OnLevelStart;
        GameManager.Instance.levelStartEvent += OnRespwan;
        GameManager.Instance.levelStartEvent += OnLevelComplete;
    }

    public void OnLevelStart(string recordName, object recordObject) { }

    public void OnRespwan(string recordName, object recordObject) { }

    public void OnLevelComplete(string recordName, object recordObject) { }


}
