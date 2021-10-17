using System;
using System.Collections.Generic;
using System.Linq;
using SpawnerTool.Data;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsEditor : Editor
    {
        private bool _colorMenu = true;
        private ProjectSettings _projectSettings;

        private void OnEnable()
        {
            _projectSettings = target as ProjectSettings;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty enemyNames = serializedObject.FindProperty("enemyNames");
            EditorGUILayout.PropertyField(enemyNames, new GUIContent("Enemy Names"), true);

            CheckEnemyNames(enemyNames);
            
           
            ListToGUI(enemyNames,"Enemy Blocks Color", ref _colorMenu);
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ListToGUI(SerializedProperty names, string itemType, ref bool visible)
        {
            SerializedProperty enemyColors = serializedObject.FindProperty("enemyColors");

            visible = EditorGUILayout.Foldout(visible, itemType);
            if (visible)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                for (int i = 0; i < names.arraySize; i++)
                {
                    string enemyName = names.GetArrayElementAtIndex(i).stringValue;
                    if (!_projectSettings.EnemyColorByType.ContainsKey(enemyName))
                    {
                        _projectSettings.EnemyColorByType.Add(enemyName, Color.black);
                        Repaint();
                    }

                    _projectSettings.EnemyColorByType[enemyName] = EditorGUILayout.ColorField(new GUIContent(enemyName),
                        _projectSettings.EnemyColorByType[enemyName], true, false, false);

                    if (i >= enemyColors.arraySize)
                    {
                        enemyColors.InsertArrayElementAtIndex(i);
                    }
                    
                    enemyColors.GetArrayElementAtIndex(i).colorValue = _projectSettings.EnemyColorByType[enemyName];
                }
                
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
        
        void CheckEnemyNames(SerializedProperty enemyNames)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < enemyNames.arraySize; i++)
            {
                names.Add(enemyNames.GetArrayElementAtIndex(i).stringValue);
            }

            bool wrong = false;
            foreach (var name in names)
            {
                if (name == String.Empty)
                {
                    wrong = true;
                }
            }

            if (wrong)
            {
                EditorGUILayout.HelpBox("There's one empty name. " +
                                        "Please delete or change it. " +
                                        "Things might not work.", MessageType.Error);
                EditorGUILayout.Space(10);
            }

            wrong = names.GroupBy(n => n).Any(g => g.Count() > 1);
            if (wrong)
            {
                EditorGUILayout.HelpBox("There are duplicated names.", MessageType.Error);
                EditorGUILayout.Space(10);
            }
        }
    }
}