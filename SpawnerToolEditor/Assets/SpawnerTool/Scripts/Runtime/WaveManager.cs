using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpawnerTool
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, rounds will start counting on awake. Otherwise, you must call StartRound method to start spawning.")] private bool _startTimerOnAwake = true;

        [Tooltip("Event when rounds finish.")]
        public UnityEvent OnRoundFinish;
        
        [Header("Debug")] 
        [SerializeField, Tooltip("Disables debug settings.")] 
        private bool _disableDebugSettings;
        [SerializeField, Tooltip("Starting round in the game.")] 
        private int _starterRound;
        
        private SpawnerGraph _currentGraph;
        private RoundTimer _roundTimer;
        private int _currentRound = 0;
        public bool IsRoundActive { get; private set; } = false;
        
        public SpawnerGraph SpawnerGraph
        {
            get
            {
                return null;
            }
            set
            {
                _currentGraph = value;
            }
        }

        private void Awake()
        {
            InitializeDebugSettings();
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
            _roundTimer = new RoundTimer(_currentGraph.rounds[_currentRound].totalRoundTime);
            _roundTimer.OnTimerEnd += WhenRoundEnds;
            IsRoundActive = true;
        }

        private void Update()
        {
            if (IsRoundActive is false)
                return;
            
            _roundTimer.Tick(Time.deltaTime);
        }

        private void WhenRoundEnds()
        {
            IsRoundActive = false;
            OnRoundFinish?.Invoke();
        }
    }
}

