﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "Enemy Prefabs", menuName = "SpawnerTool/EnemyPrefabs", order = -10)]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
        private Dictionary<string, GameObject> _idToPrefab = new Dictionary<string, GameObject>();

        public SpawnerToolInspectorData SpawnerToolInspector { get; set; }
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

    [CustomEditor(typeof(EnemyFactory))]
    public class EnemyFactorCustomInspector : Editor
    {
        private EnemyFactory _enemyFactory;

        private void OnEnable()
        {
            _enemyFactory = target as EnemyFactory;
        }

        public override void OnInspectorGUI()
        {
            _enemyFactory.SpawnerToolInspector = EditorGUILayout.ObjectField(new GUIContent("Enemy list"), _enemyFactory.SpawnerToolInspector , typeof(SpawnerToolInspectorData)) as SpawnerToolInspectorData;
            
            EditorGUILayout.LabelField("Prefabs: ");
            if(_enemyFactory.SpawnerToolInspector != null)
            {
                foreach (var enemyInfo in _enemyFactory.SpawnerToolInspector.enemyInfo)
                {
                    if (_enemyFactory.GetIdToPrefab().ContainsKey(enemyInfo.name))
                    {
                        _enemyFactory.GetIdToPrefab()[enemyInfo.name] = EditorGUILayout.ObjectField(new GUIContent(enemyInfo.name), _enemyFactory.GetIdToPrefab()[enemyInfo.name], typeof(GameObject),true) as GameObject;

                    }
                    else
                    {
                        _enemyFactory.GetIdToPrefab().Add(enemyInfo.name, null);
                    }
                } 
            }
            
            serializedObject.Update();
        }
    }
}