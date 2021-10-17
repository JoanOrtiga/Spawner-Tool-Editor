using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.Data
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/data/enemyfactory")]
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "SpawnerTool/Settings/EnemyFactory", order = -99)]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private ProjectSettings _projectSettings;
        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

        private Dictionary<string, GameObject> _idToPrefab = new Dictionary<string, GameObject>();

        /// <summary>
        /// Returns dictionary of enemy prefabs
        /// </summary>
        /// <returns>Dictionary of enemy prefabs</returns>
        public Dictionary<string, GameObject> GetIdToPrefab()
        {
            return _idToPrefab;
        }

        /// <summary>
        /// Returns dictionary of enemy prefabs. Strings are enemy type, and GameObject are enemy Prefabs.
        /// </summary>
        /// <param name="enemyType">Type of enemy you want to get Prefab of</param>
        /// <returns>Returns a Prefab given an enemy type</returns>
        public GameObject GetEnemyPrefab(string enemyType)
        {
            GameObject prefab = null;
            if (_idToPrefab.TryGetValue(enemyType, out prefab))
            {
                if (prefab == null)
                    throw new ArgumentNullException("SPAWNERTOOL: This enemy prefab is null. Make sure to add it.");
            }
            else
                throw new ArgumentOutOfRangeException("SPAWNERTOOL: This enemy type doesn't exists");

            return prefab;
        }

        #region Tool
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
            for (int i = enemyPrefabs.Count - 1; i < enemyPrefabs.Count - 1; i++)
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

        #endregion
    }
}