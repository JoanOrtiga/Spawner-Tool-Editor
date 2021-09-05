using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "SpawnerTool/Settings/ProjectSettings")]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private List<string> enemyNames = new List<string>();

        private Dictionary<string, Color> enemyInfo = new Dictionary<string, Color>();

        public Color GetEnemyColor(string enemyType)
        {
            return Color.black;
        }

        public Dictionary<string, Color> GetAllColors()
        {
            return enemyInfo;
        }
    }

    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsCustomEditor : Editor
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

            ListToGUI(enemyNames,"Enemy Blocks Color", ref _colorMenu);
            serializedObject.ApplyModifiedProperties();
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
                    string name = names.GetArrayElementAtIndex(i).stringValue;
                    if (!_projectSettings.GetAllColors().ContainsKey(name))
                    {
                        _projectSettings.GetAllColors().Add(name, Color.black);
                        Repaint();
                    }

                    _projectSettings.GetAllColors()[name] = EditorGUILayout.ColorField(new GUIContent(name),
                        _projectSettings.GetAllColors()[name], true, false, false);
                }
                
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
    }
}

