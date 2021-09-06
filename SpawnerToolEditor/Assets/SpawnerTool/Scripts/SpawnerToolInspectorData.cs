using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "SpawnerInspectorData", menuName = "SpawnerTool/Settings/SpawnerInspectorData", order = 2500)]
    public class SpawnerToolInspectorData : ScriptableObject
    {
        public SpawnEnemyData spawnEnemyData = new SpawnEnemyData();
        public bool init = false;
    }

    [CustomEditor(typeof(SpawnerToolInspectorData))]
    public class SpawnerToolEditorInspector : Editor
    {
        private SpawnerToolInspectorData _sp;
        private ProjectSettings _projectSettings;
        int _selected = 0;

        public const string Unnamed = "No type";

        private void OnEnable()
        {
            _sp = target as SpawnerToolInspectorData;

            if (_sp == null)
                return;

            ValidateValue(ref _sp.spawnEnemyData.howManyEnemies, 1);
            ValidateValue(ref _sp.spawnEnemyData.spawnPointID, 0);
            ValidateValue(ref _sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
            ValidateValue(ref _sp.spawnEnemyData.timeToStartSpawning, 0.0f);
        }

        public override void OnInspectorGUI()
        {
            DrawEnemyData();
            DrawToolSettings();
            
            if (Event.current.type == EventType.Used || _sp.init)
            {
                if(_sp.init)
                    _sp.spawnEnemyData.enemyType = Unnamed;
                _sp.init = false;
                
                ValidateValue(ref _sp.spawnEnemyData.howManyEnemies, 1);
                ValidateValue(ref _sp.spawnEnemyData.spawnPointID, 0);
                ValidateValue(ref _sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
                ValidateValue(ref _sp.spawnEnemyData.timeToStartSpawning, 0.0f);
            }
        }

        private void DrawEnemyData()
        {
            _sp.spawnEnemyData.spawnPointID = EditorGUILayout.IntField("Spawn point ID", _sp.spawnEnemyData.spawnPointID);
            _sp.spawnEnemyData.howManyEnemies =
                EditorGUILayout.IntField("How many enemies", _sp.spawnEnemyData.howManyEnemies);

            _sp.spawnEnemyData.timeBetweenSpawn =
                EditorGUILayout.FloatField("Time between spawns", _sp.spawnEnemyData.timeBetweenSpawn);
            _sp.spawnEnemyData.timeToStartSpawning =
                EditorGUILayout.FloatField("Time to start Spawning", _sp.spawnEnemyData.timeToStartSpawning);
            
            List<string> enemyNames = new List<string>();
            enemyNames.Add("Not Defined");
            bool x = false;
            foreach (var enemy in _projectSettings.GetEnemyNames())
            {
                enemyNames.Add(enemy); 
                
                if (enemy == _sp.spawnEnemyData.enemyType)
                {
                    _selected = enemyNames.IndexOf(enemy);
                    x = true;
                }
            }
            if (!x)
                _selected = 0;

            _selected = EditorGUILayout.Popup("EnemyType", _selected, enemyNames.ToArray());

            if (_projectSettings.GetEnemyNames().Count > 0)
            {
                if (_selected > 0)
                {
                    _sp.spawnEnemyData.enemyType = enemyNames[_selected];
                }
                else
                {
                    _sp.spawnEnemyData.enemyType = Unnamed;
                }
            }
        }

        private void DrawToolSettings()
        {
            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 13;
            title.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField(new GUIContent("SpawnerTool Project Settings", "Here is a tooltip"), title,
                GUILayout.Height(20));
            EditorGUILayout.Space();
            
            if (_projectSettings != null)
            {
                SerializedObject projectSettingsSerialized = new SerializedObject(_projectSettings);
                ProjectSettingsCustomEditor.DrawInspector(this, projectSettingsSerialized, _projectSettings);
            }
            
            EditorGUILayout.Space(30);
            _projectSettings = EditorGUILayout.ObjectField(new GUIContent("Project settings"), _projectSettings, typeof(ProjectSettings), true) as ProjectSettings;

            serializedObject.Update();
        }

        private void ValidateValue(ref float value, float minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        private void ValidateValue(ref int value, int minValue)
        {
            value = Mathf.Max(value, minValue);
        }
    }
}