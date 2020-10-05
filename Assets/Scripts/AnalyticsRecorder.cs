using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;

// A class to record data into UnityAnalytics when custom Events are fired.
public class AnalyticsRecorder : MonoBehaviour
{
    // Recyclable dictionary for custom events
    [SerializeField] private Dictionary<string, object> dict;
    [SerializeField] private bool debug = true;

    private void Awake()
    {
        dict = new Dictionary<string, object>();
        GameManager.Instance.levelStartEvent += AddParamenter;
        GameManager.Instance.respawnEvent += AddParamenter;
        GameManager.Instance.levelCompleteEvent += AddParamenter;
    }

    // Add information into the dictionary on firing events
    public void AddParamenter(string recordName, object recordObject)
    {
        dict[recordName] = recordObject;
    }

    // Send dictionary to Unity analatics
    public void RegisterToEvent(string eventName)
    {
        if (debug)
        {
            return;
        }

        Analytics.CustomEvent(eventName, dict);
        dict.Clear();
    }
}
