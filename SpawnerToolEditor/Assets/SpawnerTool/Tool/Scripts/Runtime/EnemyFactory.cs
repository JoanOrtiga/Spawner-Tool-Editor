using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "Enemy Prefabs", menuName = "SpawnerTool/EnemyPrefabs", order = -10)]
    public class EnemyFactory : ScriptableObject
    {
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
    }
}