using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "RoundsData", menuName = "SpawnerTool/SpawnerGraph", order = -5)]
    public class SpawnerGraph : ScriptableObject
    {
        public List<Round> rounds = new List<Round>();
        
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            SpawnerGraph scriptableObject = AssetDatabase.LoadAssetAtPath<SpawnerGraph>(assetPath);
            if (scriptableObject != null)
            {
                SpawnerToolEditor window = (SpawnerToolEditor)EditorWindow.GetWindow(typeof(SpawnerToolEditor));
                window.ChangeSpawnerGraph(scriptableObject);
                window.Show();
                window.titleContent = new GUIContent("Spawner Tool");
                return true;
            }
            return false; //let unity open it.
        }
    }

    [System.Serializable]
    public class Round
    {
        public List<SpawnEnemyData> spawningEnemiesData;
        public float totalRoundTime;

        //Tool editor
        public int totalTracks;

        public Round(List<SpawnEnemyData> spawningEnemies = null, float totalRoundTime = default,
            int totalTracks = default)
        {
            if (spawningEnemies == null)
            {
                this.spawningEnemiesData = new List<SpawnEnemyData>();
            }
            else
                this.spawningEnemiesData = spawningEnemies;

            this.totalRoundTime = totalRoundTime;
            this.totalTracks = totalTracks;
        }

        public void InitializeSpawningEnemies(List<SpawnEnemyData> spawningEnemies)
        {
            this.spawningEnemiesData = spawningEnemies;
        }
    }

    [System.Serializable]
    public class SpawnEnemyData
    {
        public string enemyType;
        public int spawnPointID;
        public int howManyEnemies;
        public float timeToStartSpawning;
        public float timeBetweenSpawn;

        //Tool editor
        public int currentTrack;

        public SpawnEnemyData(int spawnPointID = 1, float timeToStartSpawning = 0, int howManyEnemies = 5,
            string enemyType = "", float timeBetweenSpawn = 1)
        {
            this.spawnPointID = spawnPointID;
            this.timeToStartSpawning = timeToStartSpawning;
            this.howManyEnemies = howManyEnemies;
            this.enemyType = enemyType;
            this.timeBetweenSpawn = timeBetweenSpawn;
            this.currentTrack = 0;
        }

        public SpawnEnemyData()
        {
            this.spawnPointID = 1;
            this.timeToStartSpawning = 0.0f;
            this.howManyEnemies = 5;
            this.enemyType = "";
            this.timeBetweenSpawn = 1.0f;
            this.currentTrack = 0;  
        }
    }
}