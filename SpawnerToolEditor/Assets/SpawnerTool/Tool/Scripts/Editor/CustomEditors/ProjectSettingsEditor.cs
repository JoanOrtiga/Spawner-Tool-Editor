﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
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

            serializedObject.Update();
        }
        
        private void ListToGUI(SerializedProperty names, string itemType, ref bool visible)
        {
            visible = EditorGUILayout.Foldout(visible, itemType);
            if (visible)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                for (int i = 0; i < names.arraySize; i++)
                {
                    string enemyName = names.GetArrayElementAtIndex(i).stringValue;
                    if (!_projectSettings.GetAllColors().ContainsKey(enemyName))
                    {
                        _projectSettings.GetAllColors().Add(enemyName, Color.black);
                        Repaint();
                    }

                    _projectSettings.GetAllColors()[enemyName] = EditorGUILayout.ColorField(new GUIContent(enemyName),
                        _projectSettings.GetAllColors()[enemyName], true, false, false);
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