using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "RoundsData", menuName = "SpawnerTool/SpawnerGraph", order = -5)]
    public class SpawnerGraph : ScriptableObject
    {
        [SerializeField] private List<Round> _rounds = new List<Round>();

        public List<Round> GetAllRounds()
        {
            return _rounds;
        }

        public SpawnEnemyData GetSpawnEnemyDataByTime(int round, float time)
        {
            foreach (var enemyData in _rounds[round].spawningEnemiesData)
            {
                if (enemyData.IsAlreadySpawned)
                    continue;

                if (enemyData.timeToStartSpawning >= time)
                {
                    enemyData.IsAlreadySpawned = true;
                    return enemyData;
                }
            }

            return null;
        }

        public void ResetGraphState()
        {
            foreach (var round in _rounds)
            {
                foreach (var enemyData in round.spawningEnemiesData)
                {
                    enemyData.IsAlreadySpawned = false;
                }
            }
        }

        public void ResetGraphRoundState(int round)
        {
            foreach (var enemyData in _rounds[round].spawningEnemiesData)
            {
                enemyData.IsAlreadySpawned = false;
            }
        }
    }
}