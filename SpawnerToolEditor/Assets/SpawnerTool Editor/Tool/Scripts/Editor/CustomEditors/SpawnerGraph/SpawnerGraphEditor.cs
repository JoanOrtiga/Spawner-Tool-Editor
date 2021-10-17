using System;
using System.Collections.Generic;
using SpawnerTool.Data;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [CustomEditor(typeof(SpawnerGraph))]
    public class SpawnerGraphEditor : Editor
    {
        private SpawnerGraph _spawnerGraph;
        private void OnEnable()
        {
            _spawnerGraph = target as SpawnerGraph;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button(new GUIContent("OPEN", "Open spawner tool with this graph."), GUILayout.Height(40)))
            {
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(target.GetInstanceID());
                SpawnerGraph scriptableObject = UnityEditor.AssetDatabase.LoadAssetAtPath<SpawnerGraph>(assetPath);
                if (scriptableObject != null)
                {
                    SpawnerToolEditor.ShowWindow(scriptableObject);
                }
            }
            
            EditorGUILayout.Space(20);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rounds"));
            serializedObject.Update();
        }
        
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(instanceID);
            SpawnerGraph scriptableObject = UnityEditor.AssetDatabase.LoadAssetAtPath<SpawnerGraph>(assetPath);
            if (scriptableObject != null)
            {
                SpawnerToolEditor.ShowWindow(scriptableObject);
                return true;
            }

            return false; //let unity open it.
        }
    }
}