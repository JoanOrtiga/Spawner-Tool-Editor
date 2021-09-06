using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "Enemy Prefabs", menuName = "SpawnerTool/EnemyPrefabs", order = -10)]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();
        private Dictionary<string, GameObject> _idToPrefab = new Dictionary<string, GameObject>();

        public ProjectSettings ProjectSettings { get; set; }
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
            _enemyFactory.ProjectSettings = EditorGUILayout.ObjectField(new GUIContent("Enemy list"), _enemyFactory.ProjectSettings , typeof(ProjectSettings), true) as ProjectSettings;

            EditorGUILayout.Space(10);
            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 15;
           
            EditorGUILayout.LabelField("Prefabs: ", title);
            if(_enemyFactory.ProjectSettings != null)
            {
                foreach (var enemyInfo in _enemyFactory.ProjectSettings.GetEnemyNames())
                {
                    if (_enemyFactory.GetIdToPrefab().ContainsKey(enemyInfo))
                    {
                        _enemyFactory.GetIdToPrefab()[enemyInfo] = EditorGUILayout.ObjectField(new GUIContent(enemyInfo), _enemyFactory.GetIdToPrefab()[enemyInfo], typeof(GameObject),true) as GameObject;
                        Repaint();
                    }
                    else
                    {
                        _enemyFactory.GetIdToPrefab().Add(enemyInfo, null);
                    }
                } 
            }
            
            serializedObject.Update();
        }
    }
}