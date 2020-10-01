using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    // Game manager instance used for the singleton
    protected static GameManager _instance;
    // Time at the start of the level
    protected DateTime _started;
    // Time at the respawn
    protected DateTime _fromLastRespawn;
    // Coins 
    public int coins = 0;
    /// the elapsed time since the start of the level
    public System.TimeSpan GlobalTimer { get { return DateTime.UtcNow - _started; } }
    // the elapsed time since the last respawn
    public System.TimeSpan CurrentTimer { get { return DateTime.UtcNow - _fromLastRespawn; } }

    // Series of action event for info to be recorded in the the analytics.
    public event Action<string, object> levelStartEvent;
    public event Action<string, object> nodeReachedEvent;
    public event Action<string, object> coinCollected;
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
            return; //Avoid doing anything else
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        Cursor.visible = false;

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

    // Save parameters to be recorded and send the data to UnityAnalytics on level start
    private void LevelStartedRecorder()
    {
        levelStartEvent("level_id", levelID);
        levelStartEvent("date", DateTime.Today);
        levelStartEvent("level_started", true);
        levelStartEvent("current_timer", CurrentTimer);
        levelStartEvent("node_name", _spawnPrefab.name);
        FindObjectOfType<AnalyticsRecorder>().RegisterToEvent("LevelAnalytics");
    }

    // Save parameters to be recorded and send the data to UnityAnalytics when a new node is reached
    public void NodeReached(GameObject currentNode) 
    {
        nodeReachedEvent("new_node_reached", true);
        nodeReachedEvent("node_name", currentNode.name);
        nodeReachedEvent("current_time", CurrentTimer);
        FindObjectOfType<AnalyticsRecorder>().RegisterToEvent("LevelAnalytics");
    }

    // Save parameters to be recorded and send the data to UnityAnalytics on respawn
    public void RespawnPlayer()
    {     
        respawnCounter++;        
        respawnEvent("death_position", _playerPrefab.transform.position);
        respawnEvent("death_timer", CurrentTimer);
        _fromLastRespawn = DateTime.UtcNow;
        respawnEvent("player_respawned", true);
        respawnEvent("node_name", _spawnPrefab.name);
        respawnEvent("respawn_counter", respawnCounter);
        FindObjectOfType<AnalyticsRecorder>().RegisterToEvent("LevelAnalytics");
        coins = 0;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Save parameters when a coin is collected
    public void CoinCollected(GameObject coin)
    {
        coins++;
        coinCollected("coin_collected", true);
        coinCollected("coin_name", coin.name);
        nodeReachedEvent("current_time", CurrentTimer);
        FindObjectOfType<AnalyticsRecorder>().RegisterToEvent("LevelAnalytics");
    }

    // Save parameters at the end of the level and close the game
    public void LevelComplete()
    {
        levelCompleteEvent("level_completed", true);
        levelCompleteEvent("death_number", respawnCounter);
        levelCompleteEvent("coins_amount", coins);
        levelCompleteEvent("current_timer", CurrentTimer);
        levelCompleteEvent("gloabal_timer", GlobalTimer);
        FindObjectOfType<AnalyticsRecorder>().RegisterToEvent("LevelAnalytics");
        Application.Quit();
    }
}
