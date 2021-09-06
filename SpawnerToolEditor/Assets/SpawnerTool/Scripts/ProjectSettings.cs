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

        private Dictionary<string, Color> enemiesInfo = new Dictionary<string, Color>();
        
        public Dictionary<string, Color> GetAllColors()
        {
            return enemiesInfo;
        }

        public List<string> GetEnemyNames()
        {
            return enemyNames;
        }
        
        public Color GetEnemyColor(string enemyName)
        {
            Color color = Color.white;
            if (enemiesInfo.TryGetValue(enemyName, out color))
            {
                return color;
            }

            return Color.white;
        }

        public void SetEnemyColor(string enemyName, Color enemyBlockColor)
        {
            if (enemiesInfo.ContainsKey(enemyName))
            {
                enemiesInfo[enemyName] = enemyBlockColor;
            }
        }
        
        public string CheckEnemyName(string enemyName)
        {
            if (enemyNames.Contains(enemyName))
                return enemyName;
            else
                return SpawnerToolEditorInspector.Unnamed;
        }
        
        public void RandomizeColors()
        {
            List<string> keys = new List<string>(enemiesInfo.Keys);
            foreach (var enemyName in keys)
            {
                enemiesInfo[enemyName] = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            }
        }
        
        public void ResetColors()
        {
            enemiesInfo.Clear();
        }
    }

    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsCustomEditor : Editor
    {
        private static bool _colorMenu = true;
        public ProjectSettings _projectSettings;

        private void OnEnable()
        {
            if(target is ProjectSettings)
                _projectSettings = target as ProjectSettings;
        }

        public override void OnInspectorGUI()
        {
            DrawInspector(this, serializedObject, _projectSettings);
            Repaint();
        }

        public static void DrawInspector(Editor thisEditor, SerializedObject serializedObject, ProjectSettings projectSettings)
        {
            serializedObject.Update();
            SerializedProperty enemyNames = serializedObject.FindProperty("enemyNames");
            EditorGUILayout.PropertyField(enemyNames, new GUIContent("Enemy Names"), true);

            LinkToDictionary(thisEditor, enemyNames, projectSettings);
            ListToGUI(enemyNames,"Enemy Blocks Color", ref _colorMenu, projectSettings);
            serializedObject.ApplyModifiedProperties();
            CheckEnemyNames(enemyNames);

            if (GUILayout.Button("Randomize All Colors"))
            {
                projectSettings.RandomizeColors();
            }
            if (GUILayout.Button("Resets to black all colors"))
            {
                projectSettings.ResetColors();
            }
        }
        
        private static void LinkToDictionary(Editor thisEditor, SerializedProperty names, ProjectSettings projectSettings)
        {
            List<string> existingNames = new List<string>();
            for (int i = 0; i < names.arraySize; i++)
            {
                existingNames.Add(names.GetArrayElementAtIndex(i).stringValue);
            }
            
            //Add new keys
            for (int i = 0; i < existingNames.Count; i++)
            {
                if (!projectSettings.GetAllColors().ContainsKey(existingNames[i]))
                {
                    projectSettings.GetAllColors().Add(existingNames[i], Color.black);
                    thisEditor.Repaint();
                }
            }
            
            //Delete past keys.
            List<string> keysToDelete = new List<string>();
            foreach (var enemyInfo in projectSettings.GetAllColors())
            {
                if (!existingNames.Contains(enemyInfo.Key))
                    keysToDelete.Add(enemyInfo.Key);
            }

            for (int i = 0; i < keysToDelete.Count; i++)
            {
                projectSettings.GetAllColors().Remove(keysToDelete[i]);
            }
        }
        
        private static void ListToGUI(SerializedProperty names, string itemType, ref bool visible, ProjectSettings projectSettings)
        {
            visible = EditorGUILayout.Foldout(visible, itemType);
            if (visible)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                for (int i = 0; i < names.arraySize; i++)
                {
                    string name = names.GetArrayElementAtIndex(i).stringValue;
                    if (!projectSettings.GetAllColors().ContainsKey(name))
                    {
                        projectSettings.GetAllColors().Add(name, Color.black);
                    }

                    projectSettings.GetAllColors()[name] = EditorGUILayout.ColorField(new GUIContent(name), projectSettings.GetAllColors()[name], true, false, false);
                }
                
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
        
        private static void CheckEnemyNames(SerializedProperty names)
        {
            //Check for empty names.
            bool emptyName = false;
            for (int i = 0; i < names.arraySize; i++)
            {
                if (names.GetArrayElementAtIndex(i).stringValue == String.Empty)
                {
                    emptyName = true;
                }
            }
            if (emptyName)
            {
                EditorGUILayout.HelpBox("There's one empty name. Please delete or change it. Things might not work.",
                    MessageType.Error);
                EditorGUILayout.Space(10);
            }

            //Check for duplicated names.
            bool duplicatedName = false;
            var hashset = new HashSet<string>();
            for (int i = 0; i < names.arraySize; i++)
            {
                if (!hashset.Add(names.GetArrayElementAtIndex(i).stringValue))
                {
                    duplicatedName = true;
                }
            }
            if (duplicatedName)
            {
                EditorGUILayout.HelpBox("There are duplicated names.", MessageType.Error);
                EditorGUILayout.Space(10);
            }
        }
    }
}

