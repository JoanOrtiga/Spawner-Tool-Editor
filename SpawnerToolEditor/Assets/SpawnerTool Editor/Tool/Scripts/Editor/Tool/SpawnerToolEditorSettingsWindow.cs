using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    public class SpawnerToolEditorSettingsWindow : EditorWindow
    {
        private static readonly Vector2 windowMinSize = new Vector2(321, 502);
        [SerializeField] private SpawnerToolEditorSettingsWindow _editorSettingsWindow;
        [SerializeField] private SpawnerToolEditorSettings _editorSettings;

        [MenuItem("Tools/SpawnerTool/SpawnerToolEditorSettings #&y", priority = 0)]
        public static void ShowWindow()
        {
            InitializeWindow();
        }

        private static void InitializeWindow()
        {
            SpawnerToolEditorSettingsWindow editorSettingsWindow;
            if (CheckIfWindowAlreadyExists(out editorSettingsWindow))
            {
                if (editorSettingsWindow._editorSettings == null)
                {
                    editorSettingsWindow._editorSettings = SearchEditorSettings();
                }
                return;
            }

            CreateWindow(ref editorSettingsWindow);
            editorSettingsWindow._editorSettings = SearchEditorSettings();
        }

        private static void CreateWindow(ref SpawnerToolEditorSettingsWindow es)
        {
            UnityEditor.EditorWindow.CreateWindow<SpawnerToolEditorSettingsWindow>("Editor Settings");
            es = UnityEditor.EditorWindow.GetWindow<SpawnerToolEditorSettingsWindow>(false, "Editor Settings", true);
            es._editorSettingsWindow = es;
            es.minSize = windowMinSize;
        }

        private static bool CheckIfWindowAlreadyExists(out SpawnerToolEditorSettingsWindow editorSettingsWindow)
        {
            UnityEditor.EditorWindow.FocusWindowIfItsOpen<SpawnerToolEditorSettingsWindow>();
            if (focusedWindow == null)
            {
                editorSettingsWindow = null;
                return false;
            }

            if (focusedWindow.GetType() == typeof(SpawnerToolEditorSettingsWindow))
            {
                editorSettingsWindow = focusedWindow as SpawnerToolEditorSettingsWindow;
                return true;
            }

            editorSettingsWindow = null;
            return false;
        }

        private void OnGUI()
        {
            GUIStyle title = new GUIStyle("label")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };
            GUILayout.Space(15);
            GUILayout.Label("Spawner Tool Editor Settings", title);
            Editor editorSettingsInspector = Editor.CreateEditor(_editorSettings);
            editorSettingsInspector.DrawDefaultInspectorWithoutScriptField();
        }

        private static SpawnerToolEditorSettings SearchEditorSettings()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(SpawnerToolEditorSettings));
            if (guids.Length == 0)
            {
                Debug.LogError(
                    "SPAWNERTOOL: No SpawnerToolEditorSettings.asset found. Make sure there's only 1 and original SpawnerToolEditorSettings.asset");
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning(
                    "SPAWNERTOOL: More than 1 SpawnerToolEditorSettings.asset found. That may cause problems. Make sure only original 'SpawnerToolEditorSettings.asset' is in the project.");
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            SpawnerToolEditorSettings editorSettings = AssetDatabase.LoadAssetAtPath(path, typeof(SpawnerToolEditorSettings)) as SpawnerToolEditorSettings;

            if (editorSettings == null)
            {
                Debug.LogError("SPAWNERTOOL: SpawnerToolEditorSettings.asset not found");
            }
            else
            {
                editorSettings.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            
            return editorSettings;
        }
    }
}

