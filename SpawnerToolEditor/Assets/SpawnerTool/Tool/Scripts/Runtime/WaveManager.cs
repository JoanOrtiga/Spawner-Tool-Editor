using System;
using System.Collections;
using System.Collections.Generic;
using SpawnerTool.Data;
using UnityEngine;
using UnityEngine.Events;

namespace SpawnerTool.Runtime
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/runtime/wavemanager")]
    public class WaveManager : MonoBehaviour
    {
        [Header("Mandatory references")]
        [SerializeField, Tooltip("Spawner Graph, you can load it from this field or via Setter.")]
        private SpawnerGraph _currentGraph;
    

        [SerializeField, Tooltip("This should have a reference to SpawnPointsIDManager manager.")]
        private SpawnPointsIDManager _spawnPointsIDManager;

        [SerializeField, Tooltip("This should have a reference to a scriptable object of Enemy Factory.")]
        private EnemyFactory _enemyFactory;


        [Header("Optional parameters")]
        //
        [SerializeField, Tooltip("Parent transform for spawned enemies")]
        private Transform _parentToSpawnEnemies;

        [Space]
        [Header("Settings")]
        //
        [SerializeField, Tooltip("If true, rounds will start counting on awake. " +
                                 "Otherwise, you must call StartRound method to start spawning.")]
        private bool _startSpawningOnAwake = true;

        [SerializeField, Tooltip("If true, new round will start after Delay Between Rounds time." +
                                 "If false, you must call StartRound method again to start spawning")]
        private bool _startNewRoundAutomatically = true;

        [SerializeField, Tooltip("Delay in seconds between last round and new round.")]
        private float _delayBetweenRounds = 7f;

        [Space]
        [Header("Events")]
        //
        [Tooltip("When current round of the graph has finished.")]
        public UnityEvent OnRoundFinished;

        [Tooltip("When all rounds of the graph have been spawned.")]
        public UnityEvent OnAllRoundsFinished;

        [Tooltip("Raised when spawns enemy. It has the enemy GameObject reference")]
        public UnityEvent<GameObject> OnEnemySpawn;

        [Space]
        [Header("Debug")]
        //
        [SerializeField, Tooltip("Disables debug settings.")]
        private bool _disableDebugSettings;

        [SerializeField, Tooltip("Starting round in the game.")]
        private int _starterRound;


        private Timer _roundTimer;
        private int _currentRound = 0;
        private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();

        /// <summary>
        /// Is round currently spawning enemies?
        /// </summary>
        public bool IsRoundActive { get; private set; } = false;

        /// <summary>
        /// Change actual graph. It can only be changed if round is not active.
        /// </summary>
        public SpawnerGraph CurrentSpawnerGraph
        {
            get { return _currentGraph; }
            set 
            { 
                if(!IsRoundActive)
                    _currentGraph = value; 
            }
        }

        private void Awake()
        {
            InitializeDebugSettings();
            if (_startSpawningOnAwake)
                StartRound();
        }

        private void InitializeDebugSettings()
        {
            _currentRound = _starterRound;
        }

        /// <summary>
        /// Starts a new round.
        /// </summary>
        public void StartRound()
        {
            if (_currentGraph.GetAllRounds().Count <= _currentRound)
            {
                Debug.LogWarning($"SPAWNERTOOL: Round: {_currentRound} not defined in Graph: {_currentGraph.name}.");
                return;
            }
            
            _currentGraph.ResetGraphRoundState(_currentRound);
            _roundTimer = new Timer(_currentGraph.GetAllRounds()[_currentRound].TotalRoundTime);
            _roundTimer.OnTimerEnd += WhenRoundEnds;
            IsRoundActive = true;
            
            UpdateSpawners();
        }

        private void Update()
        {
            if (IsRoundActive is false)
                return;

            _roundTimer.Tick(Time.deltaTime);
            
            UpdateSpawners();
        }

        private void GetNewEnemies()
        {
            SpawnEnemyData sp;
            float time;
            
            if (_roundTimer == null)
                time = _currentGraph.GetAllRounds()[_currentRound].TotalRoundTime;
            else
                time = _roundTimer.GetCountUpTimer();

            sp = _currentGraph.GetSpawnEnemyDataByTime(_currentRound, time);

            if (sp == null)
                return;
            
            _enemySpawners.Add(new EnemySpawner(sp));
            _enemySpawners[_enemySpawners.Count-1].OnSpawnEnemy += SpawnEnemy;
        }

        private void UpdateSpawners()
        {
            GetNewEnemies();

            for (int i = _enemySpawners.Count - 1; i >= 0; i--)
            {
                _enemySpawners[i].Tick(Time.deltaTime);

                if (_enemySpawners[i].IsSpawnerFinished())
                {
                    _enemySpawners[i].OnSpawnEnemy -= SpawnEnemy;
                    _enemySpawners.RemoveAt(i);
                }
            }
        }

        private void WhenRoundEnds()
        {
            IsRoundActive = false;
            OnRoundFinished?.Invoke();

            _roundTimer.OnTimerEnd -= WhenRoundEnds;
            _roundTimer = null;

            if (_currentGraph.GetAllRounds().Count == _currentRound + 1)
            {
                OnAllRoundsFinished?.Invoke();
                return;
            }

            if (_startNewRoundAutomatically)
            {
                _roundTimer = new Timer(_delayBetweenRounds);
                _roundTimer.OnTimerEnd += StartNextRoundAutomatically;
            }
        }

        private void StartNextRoundAutomatically()
        {
            _roundTimer.OnTimerEnd -= StartNextRoundAutomatically;
            _roundTimer = null;
            StartRound();
        }

        /// <summary>
        /// In this function you should implement your own Spawner if you don't like current implementation. 
        /// </summary>
        /// <param name="enemyType"></param>
        private void SpawnEnemy(string enemyType, int spawnPointID)
        {
            Vector3 spawnPosition;
            if (_spawnPointsIDManager.TryGetSpawnPosition(spawnPointID, out spawnPosition))
            {
                GameObject enemy = null;
                enemy = Instantiate(_enemyFactory.GetEnemyPrefab(enemyType), spawnPosition, Quaternion.identity,
                    _parentToSpawnEnemies);
                OnEnemySpawn?.Invoke(enemy);
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    $"SPAWNERTOOL: Spawn point ID: {spawnPointID} doesn't exists. \n" +
                    $"Please, add enought childrens to SpawnPointIDManager, or change SpawnPointID for this enemy.");
            }
        }
    }
}