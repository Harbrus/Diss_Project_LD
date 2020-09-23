using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Random used for level id generation 
    private static System.Random random = new System.Random();
    private string levelID = "";
    private int respawnCounter;
    // Game manager instance used for the singleton
    protected static GameManager _instance;
    // Time at the start of the level
    protected DateTime _started;
    // Time at the respawn
    protected DateTime _fromLastRespawn;
    // Player prefab istance
    [SerializeField] private GameObject _playerPrefab;
    // Spawnpoint prefab istance
    [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] int idLenght = 5;
    /// the elapsed time since the start of the level
    public System.TimeSpan GlobalTimer { get { return DateTime.UtcNow - _started; } }
    // the elapsed time since the last respawn
    public System.TimeSpan CurrentTimer { get { return DateTime.UtcNow - _fromLastRespawn; } }

    // Series of action event to be fired and recorded in the the analytics.
    public event Action<string, object> registerEvent;
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

        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this as GameManager;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
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

    private void LevelStartedRecorder()
    {
        registerEvent("level_id", levelID);
        registerEvent("level_started", true);
        registerEvent("current_timer", CurrentTimer);
        registerEvent("node_name", _spawnPrefab.name);
    }

    // Generate random level id
    public static string LevelID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Send information about new node reached
    public void NodeReached(GameObject currentNode) 
    {
        registerEvent("new_node_reached", true);
        registerEvent("node_name", currentNode.name);
        registerEvent("current_time", CurrentTimer);
    }

    // Respawn player
    public void RespawnPlayer()
    {
        respawnCounter++;
        registerEvent("death_position", _playerPrefab.transform.position);
        registerEvent("death_timer", CurrentTimer);
        _fromLastRespawn = DateTime.UtcNow;
        _playerPrefab.transform.position = _spawnPrefab.transform.position;
        registerEvent("player_respawned", true);
        registerEvent("node_name", _spawnPrefab.name);
        registerEvent("respawn_counter", respawnCounter);
        registerEvent("current_timer", CurrentTimer);
    }
}
