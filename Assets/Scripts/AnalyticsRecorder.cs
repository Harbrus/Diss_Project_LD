using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// A class to record data into UnityAnalytics when custom Events are fired.
public class AnalyticsRecorder : MonoBehaviour
{
    // Recyclable dictionary for custom events
    [SerializeField] private Dictionary<string, object> dict;
    [SerializeField] private List<string> node_path;

    private void Awake()
    {
        dict = new Dictionary<string, object>();
        GameManager.Instance.levelStartEvent += OnLevelStart;
        GameManager.Instance.nodeReachedEvent += OnNodeReached;
        GameManager.Instance.respawnEvent += OnRespawn;
        GameManager.Instance.levelCompleteEvent += OnLevelComplete;
    }

    // Add information into the dictionary on firing events
    public void OnLevelStart(string recordName, object recordObject)
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary on firing events
    public void OnNodeReached(string recordName, object recordObject)
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary on firing events
    public void OnRespawn(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
    }

    // Add information into the dictionary on level complete and sent information to Unity Analytics
    public void OnLevelComplete(string recordName, object recordObject) 
    {
        dict[recordName] = recordObject;
    }

    public void RegisterEvent(string eventName)
    {
        //Analytics.CustomEvent(eventName, dict);
    }
}
