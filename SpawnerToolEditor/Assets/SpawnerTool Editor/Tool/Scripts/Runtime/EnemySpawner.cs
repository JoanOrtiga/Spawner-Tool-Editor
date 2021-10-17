using System;
using SpawnerTool.Data;
using UnityEngine;

namespace SpawnerTool.Runtime
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/runtime/enemyspawner")]
    public class EnemySpawner
    {
        private SpawnEnemyData _spawnEnemyData;
        private Timer _timer;
        private int _enemiesToSpawn;
        private event Action<string, int> _onSpawnEnemy;

        /// <summary>
        /// When enemy spawns | string = EnemyType, int = SpawnPointID
        /// </summary>
        public Action<string, int> OnSpawnEnemy
        {
            get => _onSpawnEnemy;
            set => _onSpawnEnemy = value;
        }
        
        /// <summary>
        /// Constructor of EnemySpawner
        /// </summary>
        /// <param name="spawnEnemyData">SpawnEnemyData information in order to proceed with spawns.</param>
        public EnemySpawner(SpawnEnemyData spawnEnemyData)
        {
            _spawnEnemyData = spawnEnemyData;
            _enemiesToSpawn = 0;
            
            SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            //Check if more enemies to spawn.
            if (IsSpawnerFinished())
                return;

            //Create a new enemy.
            OnSpawnEnemy?.Invoke(_spawnEnemyData.EnemyType, _spawnEnemyData.SpawnPointID);
            _timer = new Timer(_spawnEnemyData.TimeBetweenSpawn);
            _timer.OnTimerEnd += NextEnemy;

            //Update spawner enemies quantity.
            _enemiesToSpawn++;
        }

        private void NextEnemy()
        {
            //Delete last timer references.
            _timer.OnTimerEnd -= NextEnemy;
            _timer = null;
            
            SpawnEnemy();
        }
        
        /// <summary>
        /// Updates spawner timer
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(float deltaTime)
        {
            _timer.Tick(deltaTime);
        }

        /// <summary>
        /// Returns whether spawner has already spawned all enemies.
        /// </summary>
        /// <returns></returns>
        public bool IsSpawnerFinished()
        {
            return _enemiesToSpawn > _spawnEnemyData.HowManyEnemies;
        }
    }
}