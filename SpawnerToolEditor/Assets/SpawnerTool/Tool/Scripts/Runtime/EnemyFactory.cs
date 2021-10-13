using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "Enemy Prefabs", menuName = "SpawnerTool/EnemyPrefabs", order = -10)]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private ProjectSettings _projectSettings;
        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
        
        private Dictionary<string, GameObject> _idToPrefab = new Dictionary<string, GameObject>();

        public Dictionary<string, GameObject> GetIdToPrefab()
        {
            return _idToPrefab;
        }

        public GameObject GetEnemyPrefab(string enemy)
        {
            GameObject prefab = null;
            if (_idToPrefab.TryGetValue(enemy, out prefab))
            {
                if (prefab == null)
                    throw new ArgumentNullException("SPAWNERTOOL: This enemy prefab is null. Make sure to add it.");
            }
            else
                throw new ArgumentOutOfRangeException("SPAWNERTOOL: This enemy type doesn't exists");

            return prefab;
        }

        public void SaveToList()
        {
            enemyPrefabs.Clear();
            foreach (var enemies in _idToPrefab.Values)
            {
                enemyPrefabs.Add(enemies);
            }
        }

        public void LoadFromLists()
        {
            for (int i = enemyPrefabs.Count-1; i < enemyPrefabs.Count-1; i++)
            {
                enemyPrefabs.Add(null);
            }
            
            _idToPrefab.Clear();
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                _idToPrefab.Add(_projectSettings.GetEnemyNames()[i], enemyPrefabs[i]);
            }
        }

        private void OnEnable()
        {
            LoadFromLists();
        }

        private void OnDestroy()
        {
            SaveToList();
        }

        private void OnDisable()
        {
            SaveToList();
        }
    }
}