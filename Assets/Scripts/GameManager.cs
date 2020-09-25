using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool recordAnalytics = false;
    // Random used for level id generation 
    private static System.Random random = new System.Random();
    // ID string for the level
    private string levelID = "";
    [SerializeField] int idLenght = 5;
    // Respawn counter
    [SerializeField] private int respawnCounter;
    // Player prefab istance
    [SerializeField] private GameObject _playerPrefab;
    // Spawnpoint prefab istance
    [SerializeField] private GameObject _spawnPrefab;
    // Analytics recorder instance
    [SerializeField] private GameObject _recorder;
    // Game manager instance used for the singleton
    protected static GameManager _instance;
    // Time at the start of the level
    protected DateTime _started;
    // Time at the respawn
    protected DateTime _fromLastRespawn;
    /// the elapsed time since the start of the level
    public System.TimeSpan GlobalTimer { get { return DateTime.UtcNow - _started; } }
    // the elapsed time since the last respawn
    public System.TimeSpan CurrentTimer { get { return DateTime.UtcNow - _fromLastRespawn; } }

    // Series of action event for info to be recorded in the the analytics.
    public event Action<string, object> levelStartEvent;
    public event Action<string, object> nodeReachedEvent;
    public event Action<string, object> respawnEvent;
    public event Action<string, object> levelCompleteEvent;

    // Singleton to access the GameManager from other classes
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject gameManager = new GameObject();
                    _instance = gameManager.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    // Awake is called before anything else
    private void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        // if the singleton hasn't been initialized yet
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Initialization();
    }

    // Initiliase any required components and variables at Start
    protected virtual void Initialization()
    {
        _started = DateTime.UtcNow;
        _fromLastRespawn = _started;
        levelID = LevelID(idLenght);
        LevelStartedRecorder();
    }

    // Generate random level id
    public static string LevelID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Save parameters to be recorded and send the data to UnityAnalytics on Level Start
    private void LevelStartedRecorder()
    {
        if(!recordAnalytics) { return; }

        levelStartEvent("level_id", levelID);
        levelStartEvent("date", DateTime.Today);
        levelStartEvent("level_started", true);
        levelStartEvent("current_timer", CurrentTimer);
        levelStartEvent("node_name", _spawnPrefab.name);
        _recorder.GetComponent<AnalyticsRecorder>().RegisterEvent("LevelAnalytics");
    }

    // Save parameters to be recorded and send the data to UnityAnalytics when a new node is reached
    public void NodeReached(GameObject currentNode) 
    {
        if (!recordAnalytics) { return; }

        nodeReachedEvent("new_node_reached", true);
        nodeReachedEvent("node_name", currentNode.name);
        nodeReachedEvent("current_time", CurrentTimer);
        _recorder.GetComponent<AnalyticsRecorder>().RegisterEvent("LevelAnalytics");
    }

    // Save parameters to be recorded and send the data to UnityAnalytics on Respawn
    public void RespawnPlayer()
    {     
        respawnCounter++;        
        respawnEvent("death_position", _playerPrefab.transform.position);
        respawnEvent("death_timer", CurrentTimer);
        _fromLastRespawn = DateTime.UtcNow;
        respawnEvent("player_respawned", true);
        respawnEvent("node_name", _spawnPrefab.name);
        respawnEvent("respawn_counter", respawnCounter);
        _recorder.GetComponent<AnalyticsRecorder>().RegisterEvent("LevelAnalytics");
        SceneManager.LoadScene(0);
    }

    public void LevelComplete()
    {
        // to be done
    }
}
