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
    /// the elapsed time since the start of the level
    public System.TimeSpan GlobalTimer { get { return DateTime.UtcNow - _started; } }
    // the elapsed time since the last respawn
    public System.TimeSpan RespwanTimer { get { return DateTime.UtcNow - _fromLastRespawn; } }

    // Series of action event to be fired and recorded in the the analytics.
    public event Action<string, object> levelStartEvent;
    public event Action<string, object> respawnEvent;
    public event Action<string, object> levelCompleEvent;

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

    // initiliase any required components and variables at Start
    protected virtual void Initialization()
    {
        _started = DateTime.UtcNow;
        levelID = LevelID(5);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // Generate random level id
    public static string LevelID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
