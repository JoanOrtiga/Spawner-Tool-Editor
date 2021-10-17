using System;
using System.Collections.Generic;
using SpawnerTool.Data;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [Serializable]
    public class SpawnerToolEditor : EditorWindow
    {
        #region Variables

        #region Static

        private static readonly float _scrollbarSize = 13;
        public static readonly float _scrollBarPixelSize = 13;
        private static readonly float _unityWindowPixelBug = 6;

        //CELLS
        public static readonly Vector2 CellSize = new Vector2(100, 100);
        public static readonly float CellXPercentatge = 0.2f;
        public static readonly float SecondsXCell = 5f;

        #endregion

        #region References & objects

        [SerializeField] private SpawnerToolEditor _editorWindow;
        public SpawnerToolEditor EditorWindow => _editorWindow;
        [SerializeField] private SpawnerToolInspector _toolInspector;
        public SpawnerToolInspector Inspector => _toolInspector;
        [SerializeField] private SpawnerToolGraphController _graphController;
        public SpawnerToolGraphController GraphController => _graphController;
        [SerializeField] private Playground _playground;
        public Playground PlayGround => _playground;
        [SerializeField] private SpawnerToolEditorSettings _editorSettings;
        public SpawnerToolEditorSettings EditorSettings => _editorSettings;

        [SerializeField] private BlockController _blockController;
        public BlockController SpawnerBlockController => _blockController;

        //Fields:
        [SerializeField] private RoundField _roundField;
        [SerializeField] private RoundTotalTimeField _roundTotalTimeField;
        public RoundTotalTimeField GetRoundTotalTimeField => _roundTotalTimeField;
        [SerializeField] private TracksField _tracksField;
        public TracksField GetTracksField => _tracksField;
        [SerializeField] private GridMagnetField _gridMagnetField;
        [SerializeField] private DefaultEnemyBlock _defaultEnemyBlock;
        private BinField _binField;
        private GraphNameField _graphNameField;

        #endregion
        
        public Vector2 WindowSize { get; private set; }
        
        
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

        [SerializeField] private int _currentRound;
        public int CurrentRound
        {
            get => _currentRound;
            set => _currentRound = value;
        }
        
        #endregion

        #region WindowOpening

        [MenuItem("Tools/SpawnerTool/SpawnerToolEditor #&t", priority = 0)]
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

        #endregion

        #region ToolConfiguration
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
            if (_blockController == null)
                CreateBlockController();
            if (_roundField == null || _roundTotalTimeField == null || _tracksField == null || _gridMagnetField == null || _defaultEnemyBlock == null || _binField == null || _graphNameField == null)
                CreateFields();
        }

        private void LoadEditorSettings()
        {
            _editorSettings = ProjectConfiguration.Instance.GetSpawnerToolEditorSettings();
            SpawnerBlock.Texture = _editorSettings.whiteTexture;
        }

        private void CreateBlockController()
        {
            _blockController = new BlockController(this);
        }

        private void CreateInspector()
        {
            _toolInspector = new SpawnerToolInspector(this);
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
            _roundField = new RoundField(this, _currentRound);
            _roundTotalTimeField = new RoundTotalTimeField(this, _roundTotalTime);
            _tracksField = new TracksField(this, _roundTracks);
            _gridMagnetField = new GridMagnetField(this);
            _defaultEnemyBlock = new DefaultEnemyBlock(this);
            _binField = new BinField(this);
            _graphNameField = new GraphNameField(this);
        }
        #endregion
        
        private void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
            
            if(_editorSettings != null)
                SpawnerBlock.Texture = _editorSettings.whiteTexture;
        }
        
        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;

            Inspector.OnDisable();
            _blockController.OnDisable();
            _graphController.OnDisable();
        }
        
        
        private void OnDestroy()
        {
            OnDisable();
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

        private void Update()
        {
            
            if (UnityEditor.EditorWindow.focusedWindow is SpawnerToolEditor)
                return;

            if (_graphController.IsGraphNull())
                return;
            
            _blockController.Update();

            Repaint();
        }

        private void OnInspectorUpdate()
        {
            if (_graphController.IsGraphNull())
                return;

            _blockController.Update();

            Repaint();
        }

        private void InputTool()
        {
            Event e = Event.current;
            
            _roundTotalTimeField.Input(e);
            _roundField.Input(e);
            _tracksField.Input(e);
            _defaultEnemyBlock.Input(e);
            _blockController.Input(e);
            _binField.Input(e);
            _toolInspector.Input(e);
        }

        private void UpdateTool()
        {
            _playground.Update();
            _blockController.Update();
            _toolInspector.Update();
        }

        private void DrawTool()
        {
            _roundField.Draw(); 
            _roundTotalTimeField.Draw();
            _tracksField.Draw();
            _gridMagnetField.Draw();
            _binField.Draw();
            //ColorPicker();
            _playground.Draw();
            _defaultEnemyBlock.Draw();
            _graphNameField.Draw();
        }
        
        void SelectionChanged()
        {
            if (Selection.activeObject as SpawnerGraph != null)
            { 
                if(!_graphController.IsGraphNull())
                    _graphController.SaveRound(_currentRound);
                _graphController.ChangeGraph(Selection.activeObject as SpawnerGraph);
                Repaint();
            }
        }
    }
}