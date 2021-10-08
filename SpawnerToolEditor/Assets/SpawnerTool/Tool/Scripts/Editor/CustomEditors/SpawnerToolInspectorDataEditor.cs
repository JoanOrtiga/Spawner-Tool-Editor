using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnerToolInspectorData))]
    public class SpawnerToolInspectorDataEditor : Editor
    {
        private SpawnerToolInspectorData _sp;
        int _selected = 0;

        public const string Unnamed = "No type";

        private bool _visible = true;

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
            if(_sp.selected)
                DrawEnemyData();
            else
                EditorGUILayout.HelpBox("\n\n\n  No block selected. Select a block to see and modify its information. \n\n\n", MessageType.Info, true);
            
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
            /*foreach (var enemy in _sp.enemyInfo)
            {
                enemyNames.Add(enemy.name); 
                
                if (enemy.name == _sp.spawnEnemyData.enemyType)
                {
                    _selected = enemyNames.IndexOf(enemy.name);
                    x = true;
                }
            }*/
            foreach (var enemyName in ProjectConfiguration.Instance.GetProjectSettings().GetEnemyNames())
            {
                enemyNames.Add(enemyName);

                if (enemyName == _sp.spawnEnemyData.enemyType)
                {
                    _selected = enemyNames.IndexOf(enemyName);
                    x = true;
                }
            }
            
            if (!x)
                _selected = 0;

            _selected = EditorGUILayout.Popup("EnemyType", _selected, enemyNames.ToArray());


                if (_selected > 0)
                {
                    _sp.spawnEnemyData.enemyType = enemyNames[_selected];
                }
                else
                {
                    _sp.spawnEnemyData.enemyType = Unnamed;
                }
            
        }

        private void DrawToolSettings()
        {
            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 13;
            title.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField(new GUIContent("Enemy Colors by Type", "You can modify this in ProjectSettings.asset."), title,
                GUILayout.Height(20));
            EditorGUILayout.Space();

            SerializedObject projectSettingsSerialized = new SerializedObject(ProjectConfiguration.Instance.GetProjectSettings());
            ListToGUI(this, projectSettingsSerialized.FindProperty("enemyNames"), projectSettingsSerialized.FindProperty("enemyColors"),"Enemy Blocks Color", 
                ref _visible);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        public void ListToGUI(Editor editor, SerializedProperty names, SerializedProperty enemyColors, string itemType,
            ref bool visible)
        {
            Dictionary<string, Color> dictionary = ProjectConfiguration.Instance.GetProjectSettings().EnemyColorByType;

            visible = EditorGUILayout.Foldout(visible, itemType);
            if (visible)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                for (int i = 0; i < names.arraySize; i++)
                {
                    string enemyName = names.GetArrayElementAtIndex(i).stringValue;
                    if (!dictionary.ContainsKey(enemyName))
                    {
                        dictionary.Add(enemyName, Color.black);
                        editor.Repaint();
                    }

                    dictionary[enemyName] = EditorGUILayout.ColorField(new GUIContent(enemyName),
                        dictionary[enemyName], true, false, false);

                    enemyColors.GetArrayElementAtIndex(i).colorValue = dictionary[enemyName];
                }

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
    }
}