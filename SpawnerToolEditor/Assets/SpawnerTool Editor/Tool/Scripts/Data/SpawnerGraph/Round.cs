﻿using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.Data
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/data/round")]
    [System.Serializable]
    public class Round
    {
        [SerializeField] private List<SpawnEnemyData> _spawningEnemiesData;
        public List<SpawnEnemyData> SpawningEnemiesData
        {
            get => _spawningEnemiesData;
            set => _spawningEnemiesData = value;
        }
        
        [SerializeField] private float _totalRoundTime;
        public float TotalRoundTime
        {
            get => _totalRoundTime;
            set => _totalRoundTime = value;
        }

        //Tool editor
        public int totalTracks;

        public Round(List<SpawnEnemyData> spawningEnemies = null, float totalRoundTime = default,
            int totalTracks = default)
        {
            if (spawningEnemies == null)
            {
                this.SpawningEnemiesData = new List<SpawnEnemyData>();
            }
            else
                this.SpawningEnemiesData = spawningEnemies;

            this.TotalRoundTime = totalRoundTime;
            this.totalTracks = totalTracks;
        }

        private void InitializeSpawningEnemies(List<SpawnEnemyData> spawningEnemies)
        {
            _spawningEnemiesData = spawningEnemies;
        }
    }
}