using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace SpawnerTool
{
    /*
    public class SpawnerToolEditorCopy : EditorWindow
    {
        
        #region Variables

        public SpawnerGraph currentGraph;
        private static SpawnerToolInspectorData _spawnerInspector;
        private static SpawnerToolEditorSettings _editorSettings;
        private static SpawnerToolInspectorData _inspectorData;
        private static SpawnerToolEditor _window;

        private const int ScrollBarPixelSize = 13;
        private Vector2 _screenSize;
        private const int UnityWindowPixelsBug = 6;

        private static readonly Vector2 MarginToPlayField = new Vector2(57, 58);

        private Vector2 _scrollPosition;

        private string _round = "0";
        private string _totalTime = "25";
        private string _tracks = "5";

        private bool _gridMagnet = true;

        private bool _movingBlock = false;

        //CELLS
        public const float CellSize = 0.2f;
        private const float SecondsXCell = 5f;
        private float _rows = 5f;
        private float _columns = 5f;
        private float _width = 500;
        private float _height = 500;
        private static readonly Vector2 BackgroundSize = new Vector2(500, 500);
        public static readonly Vector2 CellPixelSize = new Vector2(100, 100);

        //RECTS
        private Rect _rectRoundTextField = new Rect(2, 2, 52, 41);
        private Rect _rectRoundTitle = new Rect(12, 42, 50, 18);
        private Rect _rectRoundBackground = new Rect(-3, -2, 64, 63);

        private Rect _rectTotalTimeTitle = new Rect(70, 10, 77, 15);
        private Rect _rectTotalTimeTextField = new Rect(75, 30, 70, 20);

        private Rect _rectTracksTitle = new Rect(160, 10, 77, 15);
        private Rect _rectTracksTextField = new Rect(155, 30, 70, 20);

        private Rect _rectEnemyBlockDefault = new Rect(0, 120, 57, 57);

        private Rect _rectGridMagnet = new Rect(235, 30, 100, 18);

        private Rect _rectColorPicker = new Rect(340, 30, 50, 18);

        private Rect _rectBin;
        private Rect _rectScrollView;
        private Rect _rectPlaygroundFill;

        //BLOCKS
        private SpawnerBlock _selectedSpawnerBlock = null;
        private List<SpawnerBlock> _blocks = new List<SpawnerBlock>();

        //INPUT
        private bool _inputControlPressed = false;
        private const float HorizontalScrollSpeed = 7f;
        private bool _scrollingHorizontal = false;

        private int _lastRound = 0;
        #endregion

        #region Default

        [MenuItem("SpawnerTool/Spawner")]
        public static void ShowWindow()
        {
            if (CheckIfWindowAlreadyExists())
                return;

            EditorWindow.CreateWindow<SpawnerToolEditor>("SpawnerTool");
            _window = EditorWindow.GetWindow(typeof(SpawnerToolEditor), false, "SpawnerTool", focusedWindow.hasFocus) as
                    SpawnerToolEditor;
            
            _window.minSize = new Vector2(500 + MarginToPlayField.x + ScrollBarPixelSize,
                500 + MarginToPlayField.y + ScrollBarPixelSize - UnityWindowPixelsBug);
            _window.wantsMouseEnterLeaveWindow = true;
            _window.wantsMouseMove = true;
            if (_editorSettings == null)
                EditorSettingsLoad();
            if (_inspectorData == null)
                InspectorDataLoad();
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

        public static void ShowWindow(SpawnerGraph spawnerGraph)
        {
            if (CheckIfWindowAlreadyExists())
            {
                _window.ChangeSpawnerGraph(spawnerGraph);
                return;
            }
            
            EditorWindow.CreateWindow<SpawnerToolEditor>("SpawnerTool");
            _window = EditorWindow.GetWindow(typeof(SpawnerToolEditor), false, "SpawnerTool", focusedWindow.hasFocus) as
                SpawnerToolEditor;
            
            _window.minSize = new Vector2(500 + MarginToPlayField.x + ScrollBarPixelSize,
                500 + MarginToPlayField.y + ScrollBarPixelSize - UnityWindowPixelsBug);
            _window.wantsMouseEnterLeaveWindow = true;
            _window.wantsMouseMove = true;
            if (_editorSettings == null)
                EditorSettingsLoad();
            if (_inspectorData == null)
                InspectorDataLoad();
            
            _window.ChangeSpawnerGraph(spawnerGraph);
        }

        private void OnEnable()
        {
            if (_editorSettings == null)
                EditorSettingsLoad();
            if (_inspectorData == null)
                InspectorDataLoad();

            Selection.selectionChanged += SelectionChanged;
        }

        private void OnDisable()
        {
            if (currentGraph != null)
            {
                if (!int.TryParse(_round, out var round))
                {
                    round = _lastRound;
                }
                Save(round);
            }

            SaveInspectorData();
            
            Selection.selectionChanged -= SelectionChanged;
        }

        private void OnFocus()
        {
            if (_inspectorData != null)
            {
                Selection.activeObject = _spawnerInspector;
            }
        }

        private void OnLostFocus()
        {
            if (_selectedSpawnerBlock != null)
            {
                Rect outOfBounds = _rectPlaygroundFill;

                if (!outOfBounds.Contains(_selectedSpawnerBlock.GetRect().position))
                {
                    _blocks.Remove(_selectedSpawnerBlock);
                    Repaint();
                }
            }
            
            SaveInspectorData();
        }

        private static void InspectorDataLoad()
        {
            _inspectorData = ProjectConfiguration.Instance.GetSpawnerToolInspectorData();

            _spawnerInspector = CreateInstance<SpawnerToolInspectorData>();
            LoadInspectorData();
            Selection.activeObject = _spawnerInspector;
        }

        private static void EditorSettingsLoad()
        {
            _editorSettings = ProjectConfiguration.Instance.GetSpawnerToolEditorSettings();
            SpawnerBlock.Texture = _editorSettings.whiteTexture;
        }

        void SelectionChanged()
        {
            if (Selection.activeObject as SpawnerGraph != null)
            {
                Save(int.Parse(_round));
                ChangeSpawnerGraph(Selection.activeObject as SpawnerGraph);
                Repaint();
            }
        }

        public void ChangeSpawnerGraph(SpawnerGraph spawnerGraph)
        {
            currentGraph = spawnerGraph;
            Load(int.Parse(_round));
        }

        #endregion

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
                ChangeSpawnerGraph(spawnerGraph);
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

        private void OnInspectorUpdate()
        {
            if (currentGraph is null)
                return;

            if (_selectedSpawnerBlock != null)
            {
                SaveInspectorValues();
                _selectedSpawnerBlock.UpdateTime(_spawnerInspector.spawnEnemyData.timeToStartSpawning);
                _selectedSpawnerBlock.UpdateSize();
            }

            foreach (var block in _blocks)
            {
                block.spawnEnemyData.enemyType = _spawnerInspector.CheckEnemyName(block.spawnEnemyData.enemyType);
                block.SetColor(ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(block.spawnEnemyData.enemyType));
            }

            Repaint();
        }

        void OnGUI()
        {
            if (currentGraph is null)
            {
                GraphIsNull();
                return;
            }

            if (_inspectorData == null)
            {
                Debug.Log("hola");
                InspectorDataLoad(); 
            }
                

            _screenSize = new Vector2(position.width, position.height - UnityWindowPixelsBug);

            _width = BackgroundSize.x * CellSize * _columns;
            _height = BackgroundSize.y * CellSize * _rows;

            //Input
            UserInput();

            //Update
            UpdateSelectedBlock();

            //Draw
            RoundField();
            TotalTimeField();
            TracksField();
            Bin();
            ColorPicker();
            PlayGround();
            GridMagnet();
            EnemyBlock();
        }

        private void UpdateSelectedBlock()
        {
            if (_selectedSpawnerBlock == null)
                return;

            UpdateInspector();
            //SaveInspectorValues();
            //_selectedBlock.UpdateSize();
        }

        #region DrawFunctions

        private Color _color;

        void ColorPicker()
        {
            if (_selectedSpawnerBlock == null)
                return;

            string enemyType = _selectedSpawnerBlock.spawnEnemyData.enemyType;

            _color = EditorGUI.ColorField(_rectColorPicker, new GUIContent(""), ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(enemyType),
                true, false, false);

            ProjectConfiguration.Instance.GetProjectSettings().SetEnemyColor(enemyType, _color);

            UpdateBlockColors();
        }

        void EnemyBlock()
        {
            GUI.DrawTexture(_rectEnemyBlockDefault, _editorSettings.whiteTexture);

            GUIStyle label = new GUIStyle("label");
            label.normal.textColor = Color.black;

            GUI.Label(new Rect(_rectEnemyBlockDefault.x + 2, _rectEnemyBlockDefault.y + 5, 50, 50), "Drag me", label);
        }

        void PlayGround()
        {
            _rectScrollView = new Rect(MarginToPlayField.x, MarginToPlayField.y, _screenSize.x - MarginToPlayField.x,
                _screenSize.y - MarginToPlayField.y + UnityWindowPixelsBug);

            Vector2 _scrollSave = _scrollPosition;

            _scrollPosition = GUI.BeginScrollView(_rectScrollView, _scrollPosition, new Rect(0, 0, _width, _height));
            {
                _rectPlaygroundFill = new Rect(0, 0, _width, _height);

                //Background
                GUI.DrawTextureWithTexCoords(_rectPlaygroundFill, _editorSettings.backgroundTexture,
                    new Rect(1f, CellSize - _rows, _columns * CellSize, _rows * CellSize));

                //Blocks
                for (int i = 0; i < _blocks.Count; i++)
                {
                    _blocks[i].Draw();

                    if (_selectedSpawnerBlock != _blocks[i])
                    {
                        if (!_rectPlaygroundFill.Contains(_blocks[i].GetRect().position))
                        {
                            RemoveBlock(_blocks[i]);
                        }
                    }
                }
            }
            GUI.EndScrollView();

            if (_scrollingHorizontal)
            {
                _scrollPosition = _scrollSave;
            }
        }

        void Bin()
        {
            if (_screenSize.y > MarginToPlayField.y + _height)
            {
                _rectBin = new Rect(-1, MarginToPlayField.y + _height - 60, 60, 60);
                GUI.Label(_rectBin, _editorSettings.bin);
            }
            else
            {
                _rectBin = new Rect(-1, _screenSize.y - 60, 60, 60);
                GUI.Label(_rectBin, _editorSettings.bin);
            }
        }

        void RoundField()
        {
            GUIStyle titles = new GUIStyle();
            titles.fontSize = 11;
            titles.normal.textColor = Color.black;

            GUI.Box(_rectRoundBackground, _editorSettings.whiteTexture);
            GUI.Label(_rectRoundTitle, "Round", titles);

            GUIStyle roundField = new GUIStyle(GUIStyle.none);
            roundField.normal.textColor = Color.black;
            roundField.fontSize = 25;
            roundField.alignment = TextAnchor.MiddleCenter;
            roundField.fontStyle = FontStyle.Bold;

            _round = GUI.TextField(_rectRoundTextField, _round, _editorSettings.maxCharactersRounds, roundField);
            _round = Regex.Replace(_round, @"[^0-9]", "");
            
            int round;
            if (!int.TryParse(_round, out round))
                return;
            
            if (round > _editorSettings.maxRounds)
            {
                round = _editorSettings.maxRounds;
                _round = _editorSettings.maxRounds.ToString();
            }

            if (round != _lastRound)
            {
                Save(_lastRound);
                Load(round);
                _lastRound = round;
            }
        }

        void TotalTimeField()
        {
            string workingTime = _totalTime;
            GUI.Label(_rectTotalTimeTitle, "Total time (s)");
            _totalTime = GUI.TextField(_rectTotalTimeTextField, _totalTime, _editorSettings.maxCharactersTotalTime);
            _totalTime = Regex.Replace(_totalTime, @"[^0-9,]", "");

            int totalComas = 0;
            for (int i = 0; i < _totalTime.Length; ++i)
            {
                if (_totalTime[i] == ',')
                {
                    totalComas++;
                }
            }

            if (totalComas >= 2)
            {
                _totalTime = workingTime;
            }

            if (_totalTime == String.Empty)
                return;

            if (float.Parse(_totalTime) > _editorSettings.maxTotalTime)
            {
                _totalTime = _editorSettings.maxTotalTime.ToString();
            }

            _columns = float.Parse(_totalTime);
            _columns /= SecondsXCell;
        }

        void TracksField()
        {
            GUI.Label(_rectTracksTitle, "Tracks");
            _tracks = GUI.TextField(_rectTracksTextField, _tracks, _editorSettings.maxCharactersTracks);
            _tracks = Regex.Replace(_tracks, @"[^0-9]", "");

            if (_tracks == String.Empty)
                return;

            if (int.Parse(_tracks) > _editorSettings.maxTracks)
            {
                _tracks = _editorSettings.maxTracks.ToString();
            }
            else if (int.Parse(_tracks) < _editorSettings.minTracks)
            {
                _tracks = _editorSettings.minTracks.ToString();
            }

            _rows = int.Parse(_tracks);
        }

        private void GridMagnet()
        {
            _gridMagnet = GUI.Toggle(_rectGridMagnet, _gridMagnet, "Grid magnet");
        }

        #endregion

        #region USERINPUT

        void UserInput()
        {
            List<Rect> inputFields = new List<Rect>();
            inputFields.Add(_rectRoundTextField);
            inputFields.Add(_rectTotalTimeTextField);
            inputFields.Add(_rectTracksTextField);

            Event e = Event.current;
            Vector2 mousePosition = e.mousePosition;

            BlocksInput(e, mousePosition);

            if (e.type == EventType.MouseDown)
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
            else if (e.type == EventType.ScrollWheel)
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
            }
        }

        #region BLOCK_INPUT

        void BlocksInput(Event e, Vector2 mousePosition)
        {
            Vector2 mousePositionInsidePlayground = mousePosition - MarginToPlayField + _scrollPosition;

            if (e.type == EventType.MouseDown)
            {
                if (_rectEnemyBlockDefault.Contains(mousePosition))
                {
                    NewBlock(mousePosition);
                }
                else
                {
                    SelectAnotherBlock(mousePositionInsidePlayground);
                }
            }
            else if (e.type == EventType.KeyDown)
            {
                //THIS IS FOR DEBUG
                if (e.keyCode == KeyCode.L)
                {
                    _blocks.Clear();
                    _selectedSpawnerBlock = null;
                    Repaint();
                }
            }

            if (e.type == EventType.MouseDrag)
            {
                if (_selectedSpawnerBlock is null)
                    return;

                if (_movingBlock)
                {
                    MoveBlock(e);
                }
            }

            if (e.type == EventType.MouseDown)
            {
                if (_selectedSpawnerBlock is null)
                    return;

                if (_selectedSpawnerBlock.Contains(mousePositionInsidePlayground))
                {
                    _movingBlock = true;
                }
            }

            if (e.type == EventType.MouseUp)
            {
                if (_movingBlock is false)
                    return;
                if (_selectedSpawnerBlock is null)
                    return;

                Rect outOfBounds = _rectPlaygroundFill;

                if (!outOfBounds.Contains(_selectedSpawnerBlock.GetRect().position - _scrollPosition))
                {
                    RemoveBlock(_selectedSpawnerBlock);
                }
            }

            if (e.type == EventType.MouseUp)
            {
                _movingBlock = false;
            }

            if (e.type == EventType.KeyDown)
            {
                if (_selectedSpawnerBlock is null)
                    return;

                if (e.keyCode == KeyCode.Delete)
                {
                    RemoveBlock(_selectedSpawnerBlock);
                }
            }

            if (e.type == EventType.MouseUp)
            {
                if (_selectedSpawnerBlock != null)
                    if (_rectBin.Contains(mousePosition))
                        RemoveBlock(_selectedSpawnerBlock);
            }

            if (e.type == EventType.MouseLeaveWindow)
            {
                if (_selectedSpawnerBlock is null)
                    return;

                Rect outOfBounds = new Rect(0, 0, _width, _height);

                if (!outOfBounds.Contains(_selectedSpawnerBlock.GetRect().position))
                {
                    RemoveBlock(_selectedSpawnerBlock);
                }
                else
                {
                    _movingBlock = false;
                }
            }
        }

        void NewBlock(Vector2 mousePosition)
        {
            SpawnerBlock spawnerBlock = new SpawnerBlock(mousePosition - new Vector2(50, 50), new Vector2(100, 100));
            Select(spawnerBlock);
            _movingBlock = true;
            _blocks.Add(_selectedSpawnerBlock);
            _spawnerInspector.init = true;
            Repaint();
        }

        void SelectAnotherBlock(Vector2 mousePositionInsidePlayground)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].Contains(mousePositionInsidePlayground))
                {
                    if (_blocks[i] != _selectedSpawnerBlock)
                        Select(_blocks[i]);
                }
            }
        }

        void Select(SpawnerBlock spawnerBlock)
        {
            _selectedSpawnerBlock?.Select(false);
            _selectedSpawnerBlock = spawnerBlock;
            _selectedSpawnerBlock.Select(true);
            UpdateInspector();
        }

        void MoveBlock(Event e)
        {
            Vector2 movePosition = _selectedSpawnerBlock.GetRect().position;
            Vector2 mouse = e.mousePosition - MarginToPlayField + _scrollPosition;
            float y = (Mathf.CeilToInt((mouse.y) / 100.0f) - 1) * 100;
            float x = (Mathf.CeilToInt((mouse.x) / 20.0f) - 1) * 20;

            movePosition.x = _gridMagnet ? x : e.mousePosition.x - MarginToPlayField.x + _scrollPosition.x;
            movePosition.y = y;

            _selectedSpawnerBlock.Update(movePosition, movePosition.x, (Mathf.CeilToInt((mouse.y) / 100.0f) - 1));
            Repaint();
        }

        void RemoveBlock(SpawnerBlock removeSpawnerBlock)
        {
            _blocks.Remove(removeSpawnerBlock);
            _selectedSpawnerBlock = null;
            Repaint();
        }

        #endregion

        #endregion

        #region Other

        public void UpdateBlockColors()
        {
            foreach (var block in _blocks)
            {
                block.SetColor(ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(block.spawnEnemyData.enemyType));
            }
        }

        #endregion

        #region SaveLoad

        public void Load(int round)
        {
            //First we clear the current window.
            _blocks.Clear();

            if (currentGraph.GetAllRounds() == null)
                return;

            if (currentGraph.GetAllRounds().Count <= round)
                return;

            //Second, if round exists, we load its settings.

            _rows = currentGraph.GetAllRounds()[round].totalTracks;
            _tracks = _rows.ToString();
            _columns = currentGraph.GetAllRounds()[round].totalRoundTime;
            _totalTime = _columns.ToString();


            //Lastly, we create blocks for each enemies data.
            foreach (var enemySpawnData in currentGraph.GetAllRounds()[round].spawningEnemiesData)
            {
                Rect newBlockRect = new Rect();
                newBlockRect.x = enemySpawnData.timeToStartSpawning * CellSize * CellPixelSize.x;
                newBlockRect.y = enemySpawnData.currentTrack * CellPixelSize.y;
                newBlockRect.width = CellPixelSize.x * CellSize *
                                     (enemySpawnData.howManyEnemies * enemySpawnData.timeBetweenSpawn);
                newBlockRect.height = 100f;
                SpawnerBlock spawnerBlock = new SpawnerBlock(newBlockRect, enemySpawnData);
                _blocks.Add(spawnerBlock);
            }
        }

        public void Save(int round)
        {
            if(currentGraph == null)
                return;

            if (currentGraph.GetAllRounds() == null)
            {
                var allRounds = currentGraph.GetAllRounds();
                    allRounds = new List<Round>();
            }
            
            while (currentGraph.GetAllRounds().Count <= round)
            {
                currentGraph.GetAllRounds().Add(new Round());
            }

            Round savedRound = new Round(new List<SpawnEnemyData>(), float.Parse(_totalTime), int.Parse(_tracks));

            foreach (SpawnerBlock block in _blocks)
            {
                savedRound.spawningEnemiesData.Add(block.spawnEnemyData);
            }
            
            EditorUtility.SetDirty(currentGraph); 
            
            currentGraph.GetAllRounds()[round] = savedRound;
        }

        #endregion

        #region Inspector

        void UpdateInspector() 
        {
            if (_selectedSpawnerBlock != null)
            {
                _spawnerInspector.spawnEnemyData = _selectedSpawnerBlock.spawnEnemyData;
            }
        }

        void SaveInspectorValues()
        {
            if (_selectedSpawnerBlock.spawnEnemyData.enemyType != _spawnerInspector.spawnEnemyData.enemyType)
            {
                _selectedSpawnerBlock.SetColor(ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(_spawnerInspector.spawnEnemyData.enemyType));
            }

            _selectedSpawnerBlock.spawnEnemyData = _spawnerInspector.spawnEnemyData;
        }

        static void SaveInspectorData()
        {
           // _inspectorData.init = _spawnerInspector.init;
           // _inspectorData.enemyInfo = _spawnerInspector.enemyInfo;
            _inspectorData.spawnEnemyData = _spawnerInspector.spawnEnemyData;
        }

        static void LoadInspectorData()
        {
          //  _spawnerInspector.init = _inspectorData.init;
           // _spawnerInspector.enemyInfo = _inspectorData.enemyInfo;
            _spawnerInspector.spawnEnemyData = _inspectorData.spawnEnemyData;
        }

        #endregion
    
    }
    */
}