﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// A class to record data into UnityAnalytics when custom Events are fired.
public class AnalyticsRecorder : MonoBehaviour
{
    // Recyclable dictionary for custom events
    private Dictionary<string, object> dict = new Dictionary<string, object>();

    private void Awake()
    {
        GameManager.Instance.registerEvent += RegisterCustomEvent;
        GameManager.Instance.levelCompleteEvent += OnLevelComplete;
    }

    // Add information into the dictionary on firing events
    public void RegisterCustomEvent(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary on level complete and sent information to Unity Analytics
    public void OnLevelComplete(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
        Analytics.CustomEvent("LevelCompleted", dict);
    }
}
