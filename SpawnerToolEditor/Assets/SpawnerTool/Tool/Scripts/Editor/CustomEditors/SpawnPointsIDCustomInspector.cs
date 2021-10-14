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
        }
    }
}