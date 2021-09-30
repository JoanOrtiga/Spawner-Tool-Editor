using System;
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
            _enemyFactory.ProjectSettings = EditorGUILayout.ObjectField(new GUIContent("Project Settings"), _enemyFactory.ProjectSettings , typeof(ProjectSettings), true) as ProjectSettings;

            EditorGUILayout.Space(10);
            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 15;
           
            EditorGUILayout.LabelField("Prefabs: ", title);
            if(_enemyFactory.ProjectSettings != null)
            {
                foreach (var enemyName in _enemyFactory.ProjectSettings.GetEnemyNames())
                {
                    if (_enemyFactory.GetIdToPrefab().ContainsKey(enemyName))
                    {
                        _enemyFactory.GetIdToPrefab()[enemyName] = EditorGUILayout.ObjectField(new GUIContent(enemyName), _enemyFactory.GetIdToPrefab()[enemyName], typeof(GameObject),true) as GameObject;
                        Repaint();
                    }
                    else
                    {
                        _enemyFactory.GetIdToPrefab().Add(enemyName, null);
                    }
                } 
            }
            
            serializedObject.Update();
        }
    }
}