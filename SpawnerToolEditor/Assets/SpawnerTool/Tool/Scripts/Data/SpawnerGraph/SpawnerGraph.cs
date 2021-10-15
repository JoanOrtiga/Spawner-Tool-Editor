using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.Data
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/data/spawnergraph")]
    [CreateAssetMenu(fileName = "RoundsData", menuName = "SpawnerTool/SpawnerGraph", order = -5)]
    [Serializable]
    public class SpawnerGraph : ScriptableObject
    {
        [SerializeField] private List<Round> _rounds = new List<Round>();

        /// <summary>
        /// Function to get all rounds that graph contains
        /// </summary>
        /// <returns>Returns all rounds that graph contains</returns>
        /// <see href="https://joanorba.gitbook.io/spawnertool/v/api/data/spawnergraph/getallrounds">Documentation</see>
        public List<Round> GetAllRounds()
        {
            return _rounds;
        }

        /// <summary>
        /// It returns SpawnEnemyData if time is achieved.
        /// </summary>
        /// <param name="round">The round you want to get enemies from</param>
        /// <param name="time">Time must be greater than spawn time, to return that SpawnEnemyData</param>
        /// <returns>Returns SpawnEnemyData if time is greater than an enemy spawn time in the given round.</returns>
        /// <see href="https://joanorba.gitbook.io/spawnertool/v/api/data/spawnergraph/getspawnenemydatabytime">Documentation</see>
        public SpawnEnemyData GetSpawnEnemyDataByTime(int round, float time)
        {
            foreach (var enemyData in _rounds[round].SpawningEnemiesData)
            {
                if (enemyData.IsAlreadySpawned)
                    continue;

                Debug.Log(enemyData.TimeToStartSpawning);
                if (enemyData.TimeToStartSpawning <= time)
                {
                    enemyData.IsAlreadySpawned = true;
                    return enemyData;
                }
            }

            return null;
        }

        /// <summary>
        /// Reset spawned state from all rounds.
        /// </summary>
        /// <see href="https://joanorba.gitbook.io/spawnertool/v/api/data/spawnergraph/resetgraphstate">Documentation</see>
        public void ResetGraphState()
        {
            foreach (var round in _rounds)
            {
                foreach (var enemyData in round.SpawningEnemiesData)
                {
                    enemyData.IsAlreadySpawned = false;
                }
            }
        }

        /// <summary>
        /// Reset spawned state from given round
        /// </summary>
        /// <param name="round"></param>
        /// <see href="https://joanorba.gitbook.io/spawnertool/v/api/data/spawnergraph/resetgraphroundstate">Documentation</see>
        public void ResetGraphRoundState(int round)
        {
            foreach (var enemyData in _rounds[round].SpawningEnemiesData)
            {
                enemyData.IsAlreadySpawned = false;
            }
        }
    }
}