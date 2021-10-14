using System;
using SpawnerTool.Runtime;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnPointsIDManager))]
    public class SpawnPointsIDCustomInspector : Editor
    {
        SpawnPointsIDManager _spawnPointsIDManager;

        private SerializedProperty _showAlways;
        private SerializedProperty _showSpawnNumbersAsGizmos;
        private SerializedProperty _numbersFontSize;
        private SerializedProperty _fontColor;
        private SerializedProperty _showSpawnPoints;
        private SerializedProperty _sphereRadius;
        private SerializedProperty _sphereColor;

        [SerializeField] private string _rename = "SpawnID:";
        
        private void OnEnable()
        {
            _spawnPointsIDManager = target as SpawnPointsIDManager;
            _showAlways = serializedObject.FindProperty("_showAlways");
            _showSpawnNumbersAsGizmos = serializedObject.FindProperty("_showSpawnNumbersAsGizmos");
            _numbersFontSize = serializedObject.FindProperty("_numbersFontSize");
            _fontColor = serializedObject.FindProperty("_fontColor");
            _showSpawnPoints = serializedObject.FindProperty("_showSpawnPoints");
            _sphereRadius = serializedObject.FindProperty("_sphereRadius");
            _sphereColor = serializedObject.FindProperty("_sphereColor");
        }

        public override void OnInspectorGUI()
        {
           serializedObject.Update();
           EditorGUILayout.PropertyField(_showAlways);
           EditorGUILayout.PropertyField(_showSpawnNumbersAsGizmos);

           if (_showSpawnNumbersAsGizmos.boolValue)
           {
               EditorGUILayout.PropertyField(_numbersFontSize);
               EditorGUILayout.PropertyField(_fontColor);
           }
           
           EditorGUILayout.PropertyField(_showSpawnPoints);

           if (_showSpawnPoints.boolValue)
           {
               EditorGUILayout.PropertyField(_sphereRadius);
               EditorGUILayout.PropertyField(_sphereColor);
           }
           
           serializedObject.ApplyModifiedProperties();

           EditorGUILayout.Space(20);
           EditorGUILayout.LabelField("Renaming", new GUIStyle("label"){fontStyle = FontStyle.Bold, fontSize = 13});
           _rename = EditorGUILayout.TextField("Rename (+ incremental)",_rename);
           EditorGUILayout.LabelField("Your gameObjects will be renamed to:");
           
           EditorGUI.indentLevel++;
           EditorGUI.indentLevel++;
           EditorGUILayout.LabelField(_rename + " " + 0);
           EditorGUILayout.LabelField(_rename + " " + 1);
           EditorGUI.indentLevel--;
           EditorGUI.indentLevel--;

           if (GUILayout.Button("Rename child objects"))
           {
               RenameChildObjects();
           }
        }

        public void RenameChildObjects()
        {
            
            for (int i = 0; i < _spawnPointsIDManager.transform.childCount; i++)
            {
                _spawnPointsIDManager.transform.GetChild(i).name = _rename + " " + i;
            }
        }
    }
}