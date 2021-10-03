using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace SpawnerTool
{
    public class SpawnerToolEditor : EditorWindow
    {
        /*
         * All sizes are in pixels unit.
         */

        #region Variables

        private static SpawnerToolEditor _spawnerToolEditor;
        private static SpawnerToolEditorSettings _editorSettings;

        private static SpawnerToolInspector _spawnerToolInspector;
        private static SpawnerToolGraphController _spawnerToolGraphController;

        private static readonly Vector2 _playgroundSize = new Vector2(500, 500);
        private static readonly Vector2 _marginToPlaygroundSize = new Vector2(57, 58);
        private static readonly float _scrollbarSize = 13;
        private static readonly float _scrollBarPixelSize = 13;
        
        //CELLS
        public static readonly float CellXPercentatge = 0.2f;
        public static readonly Vector2 CellSize = new Vector2(100, 100);
        
        public int CurrentRound { get; set; }
        public float CurrentTotalRoundTime { get; set; }
        public int CurrentRoundTracks { get; set; }
        public List<SpawnerBlock> Blocks { get; private set; }

        #endregion
        
        #region CreateWindow

        [MenuItem("SpawnerTool/Spawner")]
        public static void ShowWindow()
        {
            InitializeWindow();
        }

        public static void ShowWindow(SpawnerGraph spawnerGraph)
        {
            InitializeWindow(spawnerGraph);
        }

        private static void InitializeWindow(SpawnerGraph spawnerGraph = null)
        {
            if (CheckIfWindowAlreadyExists())
            {
                if (spawnerGraph != null)
                {
                    _spawnerToolGraphController.ChangeGraph(spawnerGraph);
                }

                return;
            }

            CreateWindow();
            CheckConfiguration(spawnerGraph);
        }

        private static void CreateWindow()
        {
            EditorWindow.CreateWindow<SpawnerToolEditor>("SpawnerTool");
            _spawnerToolEditor = EditorWindow.GetWindow<SpawnerToolEditor>(false, "Spawner Tool", true);

            _spawnerToolEditor.minSize = _playgroundSize + _marginToPlaygroundSize +
                new Vector2(_scrollbarSize, _scrollbarSize) - new Vector2(0, _scrollBarPixelSize);
            _spawnerToolEditor.wantsMouseEnterLeaveWindow = true;
            _spawnerToolEditor.wantsMouseMove = true;
        }

        private static bool CheckIfWindowAlreadyExists()
        {
            EditorWindow.FocusWindowIfItsOpen<SpawnerToolEditor>();
            if (focusedWindow == null)
                return false;
            if (focusedWindow.GetType() == typeof(SpawnerToolEditor))
                return true;

            return false;
        }

        #endregion

        #region Configuration

        private static void CheckConfiguration(SpawnerGraph spawnerGraph = null)
        {
            if (_editorSettings == null)
                LoadEditorSettings();
            if (_spawnerToolInspector == null)
                CreateInspector();
            if (_spawnerToolGraphController == null)
                _spawnerToolGraphController = new SpawnerToolGraphController(_spawnerToolEditor, spawnerGraph);
        }
        
        private static void LoadEditorSettings()
        {
            _editorSettings = ProjectConfiguration.Instance.GetSpawnerToolEditorSettings();
            SpawnerBlock.Texture = _editorSettings.whiteTexture;
        }

        private static void CreateInspector()
        {
            _spawnerToolInspector = new SpawnerToolInspector();
        }
        
        #endregion
        
        private void OnGUI()
        {
            CheckConfiguration();
            
            if (_spawnerToolGraphController.IsGraphNull())
            {
                GraphIsNull();
                return;
            }
        }

        void GraphIsNull()
        {
            GUIStyle createGraphStyle = new GUIStyle("button");
            createGraphStyle.fontSize = 16;
            createGraphStyle.fontStyle = FontStyle.Bold;

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Create SpawnerGraph asset in SpawnerGraphs folder", createGraphStyle))
            {
                if (!AssetDatabase.IsValidFolder("Assets/SpawnerGraphs"))
                {
                    AssetDatabase.CreateFolder("Assets", "SpawnerGraphs");
                }

                SpawnerGraph spawnerGraph = ScriptableObject.CreateInstance<SpawnerGraph>();

                int counter = 0;
                while (AssetDatabase.LoadAssetAtPath($"Assets/SpawnerGraphs/SpawnerGraph{counter}.asset",
                    typeof(SpawnerGraph)) != null)
                {
                    counter++;
                }

                AssetDatabase.CreateAsset(spawnerGraph, $"Assets/SpawnerGraphs/SpawnerGraph{counter}.asset");
                _spawnerToolGraphController.ChangeGraph(spawnerGraph);
                EditorGUIUtility.PingObject(spawnerGraph);
            }

            GUIStyle labels = new GUIStyle(GUI.skin.label);
            labels.fontSize = 16;
            labels.alignment = TextAnchor.MiddleLeft;
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("or", labels);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            labels.fontSize = 12;
            labels.fontStyle = FontStyle.Bold;
            GUILayout.Label("Select an existing graph from assets folder. (You can create one with create assets menu)",
                labels);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}