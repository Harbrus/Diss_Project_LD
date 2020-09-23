using System.Collections;
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
        GameManager.Instance.levelStartEvent += OnLevelStart;
        GameManager.Instance.levelStartEvent += OnRespwan;
        GameManager.Instance.levelStartEvent += OnLevelComplete;
    }

    // Add information into the dictionary on level start
    public void OnLevelStart(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary on respawn
    public void OnRespwan(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary when the players collect an item.
    public void OnCollection(string recordName, object recordObject)
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
