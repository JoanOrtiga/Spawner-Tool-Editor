using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class SpawnerToolEditor : EditorWindow
    {
        #region Variables

        private static readonly float _scrollbarSize = 13;
        private static readonly float _scrollBarPixelSize = 13;
        private static readonly float _unityWindowPixelBug = 6;

        //CELLS
        public static readonly Vector2 CellSize = new Vector2(100, 100);
        public static readonly float CellXPercentatge = 0.2f;
        public static readonly float SecondsXCell = 5f;

        [SerializeField] private SpawnerToolEditor _editorWindow;
        public SpawnerToolEditor EditorWindow => _editorWindow;
        [SerializeField] private SpawnerToolInspector _toolInspector;
        [SerializeField] private SpawnerToolGraphController _graphController;
        public SpawnerToolGraphController GraphController => _graphController;
        [SerializeField] private Playground _playground;
        public Playground PlayGround => _playground;
        [SerializeField] private SpawnerToolEditorSettings _editorSettings;
        public SpawnerToolEditorSettings EditorSettings => _editorSettings;

        //Fields:
        private RoundField _roundField;
        private RoundTotalTimeField _roundTotalTimeField;
        private TracksField _tracksField;

        [SerializeField] private int _currentRound;

        public int CurrentRound
        {
            get => _currentRound;
            set => _currentRound = value;
        }

        public Vector2 WindowSize { get; private set; }


        //Blocks
        [SerializeField] private List<SpawnerBlock> getBlocks = new List<SpawnerBlock>();
        public List<SpawnerBlock> GetBlocks => getBlocks;
        [SerializeField] private SpawnerBlock _selectedBlock;
        public SpawnerBlock GetSelectedBlock => _selectedBlock;


        [SerializeField] private int _roundTracks;
        public int RoundTracks
        {
            get => _roundTracks;
            set => _roundTracks = value;
        }

        [SerializeField] private float _roundTotalTime;
        public float RoundTotalTime
        {
            get => _roundTotalTime;
            set => _roundTotalTime = value;
        }

        #endregion

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
            SpawnerToolEditor spawnerToolEditor;
            if (CheckIfWindowAlreadyExists(out spawnerToolEditor))
            {
                if (spawnerGraph != null)
                {
                    spawnerToolEditor._graphController.ChangeGraph(spawnerGraph);
                }

                return;
            }

            CreateWindow(ref spawnerToolEditor);
            spawnerToolEditor.CheckConfiguration();
            if (spawnerGraph != null)
            {
                spawnerToolEditor._graphController.ChangeGraph(spawnerGraph);
            }
        }

        private static void CreateWindow(ref SpawnerToolEditor sp)
        {
            UnityEditor.EditorWindow.CreateWindow<SpawnerToolEditor>("SpawnerTool");
            sp = UnityEditor.EditorWindow.GetWindow<SpawnerToolEditor>(false, "Spawner Tool", true);
            sp._editorWindow = sp;

            sp.minSize = Playground.PlaygroundSize + Playground.MarginToPlaygroundSize +
                new Vector2(_scrollbarSize, _scrollbarSize) - new Vector2(0, _unityWindowPixelBug);
            sp.wantsMouseEnterLeaveWindow = true;
            sp.wantsMouseMove = true;
        }

        private static bool CheckIfWindowAlreadyExists(out SpawnerToolEditor spawnerToolEditor)
        {
            UnityEditor.EditorWindow.FocusWindowIfItsOpen<SpawnerToolEditor>();
            if (focusedWindow == null)
            {
                spawnerToolEditor = null;
                return false;
            }

            if (focusedWindow.GetType() == typeof(SpawnerToolEditor))
            {
                spawnerToolEditor = focusedWindow as SpawnerToolEditor;
                return true;
            }

            spawnerToolEditor = null;
            return false;
        }

        private void CheckConfiguration(SpawnerGraph spawnerGraph = null)
        {
            if (_editorSettings == null)
                LoadEditorSettings();
            if (_toolInspector == null)
                CreateInspector();
            if (_graphController == null)
                CreateGraphController(spawnerGraph);
            if (_playground == null)
                CreatePlayGround();
            if (_roundField == null || _roundTotalTimeField == null || _tracksField == null)
                CreateFields();
        }

        private void LoadEditorSettings()
        {
            _editorSettings = ProjectConfiguration.Instance.GetSpawnerToolEditorSettings();
            SpawnerBlock.Texture = _editorSettings.whiteTexture;
        }

        private void CreateInspector()
        {
            _toolInspector = new SpawnerToolInspector();
        }

        private void CreatePlayGround()
        {
            _playground = new Playground(_editorWindow);
        }

        private void CreateGraphController(SpawnerGraph spawnerGraph)
        {
            _graphController = new SpawnerToolGraphController(_editorWindow, spawnerGraph);
        }

        private void CreateFields()
        {
            _roundField = new RoundField(this);
            _roundTotalTimeField = new RoundTotalTimeField(this);
            _tracksField = new TracksField(this);
        }

        private void OnGUI()
        {
            CheckConfiguration();

            if (_graphController.IsGraphNull())
            {
                _graphController.GraphIsNull();
                return;
            }

            WindowSize = new Vector2(position.width, position.height);

            InputTool();
            UpdateTool();
            DrawTool();
        }

        private void InputTool()
        {
        }

        private void UpdateTool()
        {
            _playground.Update();
        }

        private void DrawTool()
        {
            _roundField.Draw();
            _roundTotalTimeField.Draw();
            _tracksField.Draw();
            /*
            Bin();
            ColorPicker();*/
            _playground.Draw();
            /* GridMagnet();
             EnemyBlock();*/ /////
        }
    }
}