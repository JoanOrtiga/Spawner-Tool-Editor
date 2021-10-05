using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditor.PackageManager.UI;

namespace SpawnerTool
{
    /*
    public class SpawnerToolEditorTest2 : EditorWindow
    {
        /*
         * All sizes are in pixels unit.
         *

        #region Variables

        private static readonly Vector2 _playgroundSize = new Vector2(500, 500);
        public static Vector2 MarginToPlaygroundSize { get; } = new Vector2(57, 58);
        private static readonly float _scrollbarSize = 13;
        private static readonly float _scrollBarPixelSize = 13;
        private static readonly float _unityWindowPixelBug = 6;

        //CELLS
        public static readonly float CellXPercentatge = 0.2f;
        public static readonly Vector2 CellSize = new Vector2(100, 100);

        private static SpawnerToolEditorTest2 _spawnerToolEditorTest2;
        private static SpawnerToolInspector _spawnerToolInspector;
        private static SpawnerToolGraphController _spawnerToolGraphController;
        private static Playground _playground;
        public SpawnerToolEditorSettings EditorSettings { get; set; }

        public Vector2 WindowSize { get; set; }
        private Vector2 _scrollPosition;

        public SpawnerBlock SelectedSpawnerBlock { get; set; }
        //RECTS

        public int CurrentRound { get; set; }
        private int _lastRound = 0; 
        public float CurrentTotalRoundTime { get; set; }
        public int CurrentRoundTracks { get; set; }
        public List<SpawnerBlock> Blocks { get; private set; } = new List<SpawnerBlock>();

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
            EditorWindow.CreateWindow<SpawnerToolEditorTest2>("SpawnerTool");
            _spawnerToolEditorTest2 = EditorWindow.GetWindow<SpawnerToolEditorTest2>(false, "Spawner Tool", true);

            _spawnerToolEditorTest2.minSize = _playgroundSize + MarginToPlaygroundSize +
                                         new Vector2(_scrollbarSize, _scrollbarSize)- new Vector2(0, _unityWindowPixelBug);
            _spawnerToolEditorTest2.wantsMouseEnterLeaveWindow = true;
            _spawnerToolEditorTest2.wantsMouseMove = true;
        }

        private static bool CheckIfWindowAlreadyExists()
        {
            EditorWindow.FocusWindowIfItsOpen<SpawnerToolEditorTest2>();
            if (focusedWindow == null)
                return false;
            if (focusedWindow.GetType() == typeof(SpawnerToolEditorTest2))
                return true;

            return false;
        }

        #endregion

        #region Configuration

        private static void CheckConfiguration(SpawnerGraph spawnerGraph = null)
        {
            if (EditorSettings == null)
                LoadEditorSettings();
            if (_spawnerToolInspector == null)
                CreateInspector();
            if (_spawnerToolGraphController == null)
                _spawnerToolGraphController = new SpawnerToolGraphController(_spawnerToolEditorTest2, spawnerGraph);
            if(_playground == null)
                CreatePlayGround();

            _spawnerToolGraphController.HasLostReference(_spawnerToolEditorTest2);
            _playground.HasLostReference(_spawnerToolEditorTest2);
        }

        private static void LoadEditorSettings()
        {
            EditorSettings = ProjectConfiguration.Instance.GetSpawnerToolEditorSettings();
            SpawnerBlock.Texture = EditorSettings.whiteTexture;
        }

        private static void CreateInspector()
        {
            _spawnerToolInspector = new SpawnerToolInspector();
        }

        private static void CreatePlayGround()
        {
            _playground = new Playground(_spawnerToolEditorTest2);
        }

        #endregion

        private void OnGUI()
        {
            _spawnerToolEditorTest2 = this;
            CheckConfiguration();

            if (_spawnerToolGraphController.IsGraphNull())
            {
                GraphIsNull();
                return;
            }

            WindowSize = new Vector2(position.width, position.height);

            _playground.Width = _playgroundSize.x * CellXPercentatge * _playground.Columns;
            _playground.Height = _playgroundSize.y * CellXPercentatge * _playground.Rows;

            /*
            //Input
            UserInput();

            //Update
            UpdateSelectedBlock();*

            DrawTool();
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

        void DrawTool()
        {

            //Draw
            RoundField();
            /* TotalTimeField();
            TracksField();
            Bin();
            ColorPicker();*
            _playground.Draw();
            /* GridMagnet();
             EnemyBlock();*
        }
        
        void RoundField()
        {
            Rect _rectRoundTextField = new Rect(2, 2, 52, 41);
            Rect _rectRoundTitle = new Rect(12, 42, 50, 18);
            Rect _rectRoundBackground = new Rect(-3, -2, 64, 63); 
            
            GUIStyle titles = new GUIStyle();
            titles.fontSize = 11;
            titles.normal.textColor = Color.black;

            GUI.Box(_rectRoundBackground, EditorSettings.whiteTexture);
            GUI.Label(_rectRoundTitle, "Round", titles);

            GUIStyle roundField = new GUIStyle(GUIStyle.none);
            roundField.normal.textColor = Color.black;
            roundField.fontSize = 25;
            roundField.alignment = TextAnchor.MiddleCenter;
            roundField.fontStyle = FontStyle.Bold;

            string roundText = CurrentRound.ToString();
            roundText = GUI.TextField(_rectRoundTextField, roundText, EditorSettings.maxCharactersRounds, roundField);
            roundText = Regex.Replace(roundText, @"[^0-9]", "");
            
            int round;
            if (!int.TryParse(roundText, out round))
                return;
            
            if (round > EditorSettings.maxRounds)
            {
                round = EditorSettings.maxRounds;
                CurrentRound = EditorSettings.maxRounds;
            }

            if (round != _lastRound)
            {
                _spawnerToolGraphController.SaveRound(_lastRound);
                _spawnerToolGraphController.LoadRound(round);
                _lastRound = round;
            }
        }
    }
    */
}