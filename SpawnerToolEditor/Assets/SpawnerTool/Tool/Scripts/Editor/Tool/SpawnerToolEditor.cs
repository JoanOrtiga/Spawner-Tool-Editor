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

        #region Static

        private static readonly float _scrollbarSize = 13;
        private static readonly float _scrollBarPixelSize = 13;
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
        [SerializeField] private SpawnerToolGraphController _graphController;
        public SpawnerToolGraphController GraphController => _graphController;
        [SerializeField] private Playground _playground;
        public Playground PlayGround => _playground;
        [SerializeField] private SpawnerToolEditorSettings _editorSettings;
        public SpawnerToolEditorSettings EditorSettings => _editorSettings;

        //Fields:
        [SerializeField] private RoundField _roundField;
        [SerializeField] private RoundTotalTimeField _roundTotalTimeField;
        [SerializeField] private TracksField _tracksField;
        [SerializeField] private GridMagnetField _gridMagnetField;
        [SerializeField] private DefaultEnemyBlock _defaultEnemyBlock;

        #endregion
        
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

        [SerializeField] private int _currentRound;
        public int CurrentRound
        {
            get => _currentRound;
            set => _currentRound = value;
        }
        
        #endregion

        #region WindowOpening

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
            if (_roundField == null || _roundTotalTimeField == null || _tracksField == null || _gridMagnetField == null || _defaultEnemyBlock == null)
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
            _gridMagnetField = new GridMagnetField(this);
            _defaultEnemyBlock = new DefaultEnemyBlock(this);
        }
        #endregion
        
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
            Event e = Event.current;
            
            _roundTotalTimeField.Input(e);
            _roundField.Input(e);
            _tracksField.Input(e);
            
           /* if (e.type == EventType.MouseDown)
            {
                bool inInputField = false;
                for (int i = 0; i < inputFields.Count; i++)
                {
                    if (inputFields[i].Contains(mousePosition))
                    {
                        inInputField = true;
                    }
                }

                if (!inInputField)
                {
                    GUI.FocusControl("");
                }

                if (!_rectRoundBackground.Contains(mousePosition))
                {
                    if (_round != String.Empty)
                    {
                        if (int.Parse(_round) < 0)
                        {
                            _round = "0";
                        }
                    }
                    else
                    {
                        _round = "0";
                    }
                }

                Repaint();
            }
            
            /*else if (e.type == EventType.ScrollWheel)
            {
                if (_rectRoundBackground.Contains(mousePosition))
                {
                    if (e.delta.y > 0)
                    {
                        if (_round == String.Empty)
                        {
                            _round = "0";
                        }

                        string lastRound = _round;

                        _round = (int.Parse(_round) - 1).ToString();
                        if (int.Parse(_round) < 0)
                        {
                            _round = "0";
                        }

                    }
                    else
                    {
                        if (_round == String.Empty)
                        {
                            _round = "0";
                        }

                        string lastRound = _round;

                        _round = (int.Parse(_round) + 1).ToString();
                        if (int.Parse(_round) > _editorSettings.maxRounds)
                        {
                            _round = _editorSettings.maxRounds.ToString();
                        }
                    }

                    Repaint();
                }
                else if (_rectTotalTimeTextField.Contains(mousePosition))
                {
                    if (e.delta.y > 0)
                    {
                        _totalTime = (float.Parse(_totalTime) - 1.0f).ToString();
                        if (float.Parse(_totalTime) < 1.0f)
                        {
                            _totalTime = "1f";
                        }
                    }
                    else
                    {
                        _totalTime = (float.Parse(_totalTime) + 1).ToString();
                        if (float.Parse(_totalTime) > _editorSettings.maxTotalTime)
                        {
                            _totalTime = _editorSettings.maxTotalTime.ToString();
                        }
                    }

                    Repaint();
                }
                else if (_rectTracksTextField.Contains(mousePosition))
                {
                    if (e.delta.y > 0)
                    {
                        _tracks = (int.Parse(_tracks) - 1).ToString();
                        if (int.Parse(_tracks) < _editorSettings.minTracks)
                        {
                            _tracks = _editorSettings.minTracks.ToString();
                        }
                    }
                    else
                    {
                        _tracks = (int.Parse(_tracks) + 1).ToString();
                        if (int.Parse(_tracks) > _editorSettings.maxTracks)
                        {
                            _tracks = _editorSettings.maxTracks.ToString();
                        }
                    }

                    Repaint();
                }
                else if (_rectScrollView.Contains(mousePosition))
                {
                    if (_inputControlPressed is true)
                    {
                        _scrollingHorizontal = true;
                        _scrollPosition += new Vector2(e.delta.y * HorizontalScrollSpeed, 0);
                        _scrollPosition = new Vector2(Mathf.Clamp(_scrollPosition.x, 0, _width),
                            Mathf.Clamp(_scrollPosition.y, 0, _height));
                    }
                }
            }

            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.LeftControl || e.keyCode == KeyCode.RightControl)
                {
                    _inputControlPressed = true;
                }
            }
            else if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.LeftControl || e.keyCode == KeyCode.RightControl)
                {
                    _inputControlPressed = false;
                    _scrollingHorizontal = false;
                }
            }*/
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
            _gridMagnetField.Draw();
            /*
            Bin();
            ColorPicker();*/
            _playground.Draw();
            _defaultEnemyBlock.Draw();
        }
    }
}