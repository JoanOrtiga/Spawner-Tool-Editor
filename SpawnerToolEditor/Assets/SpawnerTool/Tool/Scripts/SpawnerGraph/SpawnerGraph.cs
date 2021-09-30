using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "RoundsData", menuName = "SpawnerTool/SpawnerGraph", order = -5)]
    public class SpawnerGraph : ScriptableObject
    {
        public List<Round> rounds = new List<Round>();

        public SpawnEnemyData GetSpawnEnemyDataByTime(int round, float time)
        {
            foreach (var enemyData in rounds[round].spawningEnemiesData)
            {
                if(enemyData.IsAlreadySpawned)
                    continue;
                
                if (enemyData.timeToStartSpawning < time)
                {
                    enemyData.IsAlreadySpawned = true;
                    return enemyData;
                }
            }

            return null;
        }

        public void ResetGraphState()
        {
            foreach (var round in rounds)
            {
                foreach (var enemyData in round.spawningEnemiesData)
                {
                    enemyData.IsAlreadySpawned = false;
                }
            }
        }
        
#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(instanceID);
            SpawnerGraph scriptableObject = UnityEditor.AssetDatabase.LoadAssetAtPath<SpawnerGraph>(assetPath);
            if (scriptableObject != null)
            {
                SpawnerToolEditor.ShowWindow(scriptableObject);
                return true;
            }

            return false; //let unity open it.
        }
#endif
    }
}