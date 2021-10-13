using System;
using UnityEngine;

namespace SpawnerTool
{
    public class EnemySpawner
    {
        private SpawnEnemyData _spawnEnemyData;
        private Timer _timer;
        private int _enemiesToSpawn;
        
        /// <summary>
        /// string = EnemyType, int = SpawnPointID
        /// </summary>
        public event Action<string, int> OnSpawnEnemy; 
        
        public EnemySpawner(SpawnEnemyData spawnEnemyData)
        {
            _spawnEnemyData = spawnEnemyData;
            _enemiesToSpawn = 0;
            
            SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            //Check if more enemies to spawn.
            if (SpawnerFinished())
                return;

            //Create a new enemy.
            OnSpawnEnemy?.Invoke(_spawnEnemyData.enemyType, _spawnEnemyData.spawnPointID);
            _timer = new Timer(_spawnEnemyData.timeBetweenSpawn);
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
        public bool SpawnerFinished()
        {
            return _enemiesToSpawn >= _spawnEnemyData.howManyEnemies;
        }
    }
}