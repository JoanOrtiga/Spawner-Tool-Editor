using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnerToolInspectorData))]
    public class SpawnerToolEditorInspector : Editor
    {
        private SpawnerToolInspectorData _sp;
        int _selected = 0;

        public const string Unnamed = "No type";

        private bool _visible = false;

        private void OnEnable()
        {
            _sp = target as SpawnerToolInspectorData;
            
            if (_sp == null)
                return;

            SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.howManyEnemies, 1);
            SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.spawnPointID, 0);
            SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
            SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.timeToStartSpawning, 0.0f);
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
                
                SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.howManyEnemies, 1);
                SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.spawnPointID, 0);
                SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
                SpawnerToolEditorUtility.ValidateValue(ref _sp.spawnEnemyData.timeToStartSpawning, 0.0f);
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
            foreach (var enemy in _sp.enemyInfo)
            {
                enemyNames.Add(enemy.name); 
                
                if (enemy.name == _sp.spawnEnemyData.enemyType)
                {
                    _selected = enemyNames.IndexOf(enemy.name);
                    x = true;
                }
            }
            if (!x)
                _selected = 0;

            _selected = EditorGUILayout.Popup("EnemyType", _selected, enemyNames.ToArray());

            if (_sp.enemyInfo.Count > 0)
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
            EditorGUILayout.LabelField(new GUIContent("SpawnerTool Project Settings", "You can modify this in ProjectSettings.asset."), title,
                GUILayout.Height(20));
            EditorGUILayout.Space();

            SerializedObject projectSettingsSerialized = new SerializedObject(ProjectConfiguration.instance.GetProjectSettings());
            SpawnerToolEditorUtility.ListToGUI(this, projectSettingsSerialized.FindProperty("enemyNames"), 
                ProjectConfiguration.instance.GetProjectSettings().GetAllColors(),"Enemy Blocks Color", 
                ref _visible);
            
            serializedObject.Update();
        }
        
        
    }
}