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
    [SerializeField] private bool debug = false;
    string path;

    private void Awake()
    {
        path = Application.dataPath + "/SaveData/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        dict = new Dictionary<string, object>();
        GameManager.Instance.levelStartEvent += AddToDictionary;
        GameManager.Instance.respawnEvent += AddToDictionary;
        GameManager.Instance.levelCompleteEvent += AddToDictionary;
    }

    // Add information into the dictionary on firing events
    public void AddToDictionary(string recordName, object recordObject)
    {
        dict[recordName] = recordObject;
    }

    public void SaveList(string eventName, object saveList) 
    {
        if (debug)
        {
            return;
        }

        string authorData = JsonConvert.SerializeObject(saveList, new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.Indented
        });

        File.WriteAllText(path + eventName + ".json", authorData);
    }

    // Send dictionary to Unity analatics
    public void SaveDictionary(string eventName)
    {
        if (debug)
        {
            return;
        }

        string authorData = JsonConvert.SerializeObject(dict, new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.Indented
        });

        File.WriteAllText(path  + eventName + ".json", authorData);

    }
}
