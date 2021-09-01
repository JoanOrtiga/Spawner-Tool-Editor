using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using System.Text.RegularExpressions;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UI;
using Vector2 = UnityEngine.Vector2;

namespace SpawnerTool
{
    public class Block
    {
        private Rect _rect;
        private Vector2 _size;
        private Color _color;
        private Color _highlightColor;
        public static Texture2D texture;

        private bool selected;

        public SpawnEnemyData spawnEnemyData;

        public Block(Vector2 position, Vector2 size, SpawnEnemyData sp = null)
        {
            spawnEnemyData = sp is null ? new SpawnEnemyData() : sp;

            _size.x = Mathf.Max(size.x, 20.0f);

            _size = size;
            _rect = new Rect(position.x, position.y, size.x, size.y);
            _color = Color.white;
            _highlightColor = Color.yellow;
        }

        public Block(Rect rect, SpawnEnemyData sp = null)
        {
            spawnEnemyData = sp is null ? new SpawnEnemyData() : sp;

            rect.width = Mathf.Max(rect.width, 20.0f);

            _size = rect.size;
            _rect = rect;
            _color = Color.white;
            _highlightColor = Color.yellow;
        }

        public void Update(Vector2 mouseDrag, float time, int track)
        {
            spawnEnemyData.currentTrack = track;
            spawnEnemyData.timeToStartSpawning =
                time / (SpawnerToolEditor.CellSize * SpawnerToolEditor.CellPixelSize.x);
            _rect = new Rect(mouseDrag.x, mouseDrag.y, _size.x, _size.y);
        }

        public void UpdateTime(float time)
        {
            spawnEnemyData.timeToStartSpawning = time;
            _rect.x = time * (SpawnerToolEditor.CellSize * SpawnerToolEditor.CellPixelSize.x);
        }

        public void ChangeSize(Vector2 size)
        {
            size.x = Mathf.Max(size.x, 20.0f);
            _size = size;
        }

        public void Select(bool select)
        {
            selected = select;
        }

        public bool Contains(Vector2 position)
        {
            if (_rect.Contains(position))
            {
                return true;
            }

            return false;
        }

        public void Draw()
        {
            Color guiColor = GUI.color;
            GUI.color = selected ? _highlightColor : _color;
            GUI.DrawTexture(_rect, texture);
            GUI.color = guiColor;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public Rect GetRect()
        {
            return _rect;
        }

        public void UpdateSize()
        {
            _size.x = SpawnerToolEditor.CellPixelSize.x * SpawnerToolEditor.CellSize *
                      (spawnEnemyData.howManyEnemies * spawnEnemyData.timeBetweenSpawn);
            _size.y = 100f;

            _rect.size = _size;
        }
    }

    public class SpawnerToolEditor : EditorWindow
    {
        #region Variables

        public SpawnerGraph currentGraph;
        private static SpawnerToolEditorSettings _editorSettings;
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
        private Block _selectedBlock = null;
        private List<Block> _blocks = new List<Block>();

        private static SpawnerToolInspectorData _spawnerToolInspectorData;

        //INPUT
        private bool _inputControlPressed = false;
        private const float HorizontalScrollSpeed = 7f;
        private bool _scrollingHorizontal = false;

        #endregion

        #region Default

        [MenuItem("SpawnerTool/Spawner")]
        public static void ShowWindow()
        {
            _window =
                EditorWindow.GetWindow(typeof(SpawnerToolEditor), false, "SpawnerTool", focusedWindow.hasFocus) as
                    SpawnerToolEditor;
            //_window = EditorWindow.GetWindow<SpawnerToolEditor>(false, "SpawnerTool", focusedWindow.hasFocus);
            // _window = EditorWindow.GetWindow(typeof(SpawnerToolEditor), true, "SpawnerTool", focusedWindow.hasFocus);
            _window.minSize = new Vector2(500 + MarginToPlayField.x + ScrollBarPixelSize,
                500 + MarginToPlayField.y + ScrollBarPixelSize - UnityWindowPixelsBug);

            _window.wantsMouseEnterLeaveWindow = true;
            _window.wantsMouseMove = true;
            EditorSettingsLoad();
        }

        private void OnEnable()
        {
            if (_editorSettings == null)
                EditorSettingsLoad();
            // ChangeSpawnerGraph((SpawnerGraph)EditorGUILayout.ObjectField(currentGraph, typeof(SpawnerGraph), false, GUILayout.Width(200)));
            //_window = GetWindow<SpawnerToolEditor>();
            //_window.titleContent.text = "SpawnerTool";

            //EditorSettingsLoad();
            Selection.selectionChanged += SelectionChanged;
        }

        private void OnDisable()
        {
            if (currentGraph != null)
            {
                Save(int.Parse(_round));
            }

            Selection.selectionChanged -= SelectionChanged;
            DestroyImmediate(_spawnerToolInspectorData);
        }

        private void OnFocus()
        {
            if (_spawnerToolInspectorData != null)
            {
                Selection.activeObject = _spawnerToolInspectorData;
            }
        }

        private void OnLostFocus()
        {
            if (_selectedBlock != null)
            {
                Rect outOfBounds = _rectPlaygroundFill;

                if (!outOfBounds.Contains(_selectedBlock.GetRect().position))
                {
                    _blocks.Remove(_selectedBlock);
                    Repaint();
                }
            }
        }

        void CreateInspector()
        {
            _spawnerToolInspectorData = CreateInstance<SpawnerToolInspectorData>();
            Selection.activeObject = _spawnerToolInspectorData;
        }

        private static void EditorSettingsLoad()
        {
            string[] guids =
                AssetDatabase.FindAssets("SpawnerToolEditorSettings t:" + typeof(SpawnerToolEditorSettings));
            if (guids.Length == 0)
            {
                Debug.LogError(
                    "SPAWNERTOOL: No editor settings found. Make sure original 'SpawnerToolEditorSettings.asset' is in the project.");
                return;
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning(
                    "SPAWNERTOOL: More than one editor settings found. That may cause problems. Make sure only original 'SpawnerToolEditorSettings.asset' is in the project.");
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _editorSettings =
                AssetDatabase.LoadAssetAtPath(path, typeof(SpawnerToolEditorSettings)) as SpawnerToolEditorSettings;
            if (_editorSettings == null)
            {
                Debug.LogError("Editor settings not found");
            }

            Block.texture = _editorSettings.whiteTexture;
        }

        void SelectionChanged()
        {
            if (Selection.activeObject as SpawnerGraph != null)
            {
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
            /*
                        if (_selectedBlock != null)
                        {
                            _selectedBlock.UpdateTime(_spawnerToolInspectorData.spawnEnemyData.timeToStartSpawning);
                        }
                        
                        Repaint();*/
        }

        void OnGUI()
        {
            if (currentGraph is null)
            {
                GraphIsNull();
                return;
            }

            if (_spawnerToolInspectorData == null)
                CreateInspector();

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
            if (_selectedBlock == null)
                return;

            //UpdateInspector();
            //SaveInspectorValues();
            //_selectedBlock.UpdateSize();
        }

        #region DrawFunctions

        private Color _color;

        void ColorPicker()
        {
            _color = EditorGUI.ColorField(_rectColorPicker, new GUIContent(""), _color, true, false, false);
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

                    if (_selectedBlock != _blocks[i])
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

            if (_round == String.Empty)
                return;

            if (int.Parse(_round) > _editorSettings.maxRounds)
            {
                _round = _editorSettings.maxRounds.ToString();
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
                    _selectedBlock = null;
                    Repaint();
                }
            }

            if (e.type == EventType.MouseDrag)
            {
                if (_selectedBlock is null)
                    return;

                if (_movingBlock)
                {
                    MoveBlock(e);
                }
            }

            if (e.type == EventType.MouseDown)
            {
                if (_selectedBlock is null)
                    return;

                if (_selectedBlock.Contains(mousePositionInsidePlayground))
                {
                    _movingBlock = true;
                }
            }

            if (e.type == EventType.MouseUp)
            {
                if (_movingBlock is false)
                    return;
                if (_selectedBlock is null)
                    return;

                Rect outOfBounds = _rectPlaygroundFill;

                if (!outOfBounds.Contains(_selectedBlock.GetRect().position - _scrollPosition))
                {
                    RemoveBlock(_selectedBlock);
                }
            }

            if (e.type == EventType.MouseUp)
            {
                _movingBlock = false;
            }

            if (e.type == EventType.KeyDown)
            {
                if (_selectedBlock is null)
                    return;

                if (e.keyCode == KeyCode.Delete)
                {
                    RemoveBlock(_selectedBlock);
                }
            }

            if (e.type == EventType.MouseUp)
            {
                if (_selectedBlock != null)
                    if (_rectBin.Contains(mousePosition))
                        RemoveBlock(_selectedBlock);
            }

            if (e.type == EventType.MouseLeaveWindow)
            {
                if (_selectedBlock is null)
                    return;

                Rect outOfBounds = new Rect(0, 0, _width, _height);

                if (!outOfBounds.Contains(_selectedBlock.GetRect().position))
                {
                    RemoveBlock(_selectedBlock);
                }
                else
                {
                    _movingBlock = false;
                }
            }
        }

        void NewBlock(Vector2 mousePosition)
        {
            Block block = new Block(mousePosition - new Vector2(50, 50), new Vector2(100, 100));
            Select(block);
            _movingBlock = true;
            _blocks.Add(_selectedBlock);
            _spawnerToolInspectorData.init = true;
            Repaint();
        }

        void SelectAnotherBlock(Vector2 mousePositionInsidePlayground)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].Contains(mousePositionInsidePlayground))
                {
                    if (_blocks[i] != _selectedBlock)
                        Select(_blocks[i]);
                }
            }
        }

        void Select(Block block)
        {
            _selectedBlock?.Select(false);
            _selectedBlock = block;
            _selectedBlock.Select(true);
            UpdateInspector();
        }

        void MoveBlock(Event e)
        {
            Vector2 movePosition = _selectedBlock.GetRect().position;
            Vector2 mouse = e.mousePosition - MarginToPlayField + _scrollPosition;
            float y = (Mathf.CeilToInt((mouse.y) / 100.0f) - 1) * 100;
            float x = (Mathf.CeilToInt((mouse.x) / 20.0f) - 1) * 20;

            movePosition.x = _gridMagnet ? x : e.mousePosition.x - MarginToPlayField.x + _scrollPosition.x;
            movePosition.y = y;

            _selectedBlock.Update(movePosition, movePosition.x, (Mathf.CeilToInt((mouse.y) / 100.0f) - 1));
            Repaint();
        }

        void RemoveBlock(Block removeBlock)
        {
            _blocks.Remove(removeBlock);
            Repaint();
        }

        #endregion

        #endregion

        #region SaveLoad

        public void Load(int round)
        {
            //First we clear the current window.
            _blocks.Clear();

            if (currentGraph.rounds == null)
                return;

            if (currentGraph.rounds.Count <= round)
                return;

            //Second, if round exists, we load its settings.

            _rows = currentGraph.rounds[round].totalTracks;
            _tracks = _rows.ToString();
            _columns = currentGraph.rounds[round].totalRoundTime;
            _totalTime = _columns.ToString();


            //Lastly, we create blocks for each enemies data.
            foreach (var enemySpawnData in currentGraph.rounds[round].spawningEnemiesData)
            {
                Rect newBlockRect = new Rect();
                newBlockRect.x = enemySpawnData.timeToStartSpawning * CellSize * CellPixelSize.x;
                newBlockRect.y = enemySpawnData.currentTrack * CellPixelSize.y;
                newBlockRect.width = CellPixelSize.x * CellSize *
                                     (enemySpawnData.howManyEnemies * enemySpawnData.timeBetweenSpawn);
                newBlockRect.height = 100f;
                Block block = new Block(newBlockRect, enemySpawnData);
                _blocks.Add(block);
            }
        }

        public void Save(int round)
        {
            if (currentGraph.rounds == null)
                currentGraph.rounds = new List<Round>();

            while (currentGraph.rounds.Count <= round)
            {
                currentGraph.rounds.Add(new Round());
            }

            Round savedRound = new Round(new List<SpawnEnemyData>(), float.Parse(_totalTime), int.Parse(_tracks));

            foreach (Block block in _blocks)
            {
                savedRound.spawningEnemiesData.Add(block.spawnEnemyData);
            }

            currentGraph.rounds[round] = savedRound;
        }

        #endregion

        #region Inspector

        void UpdateInspector()
        {
            if (_selectedBlock != null)
            {
                _spawnerToolInspectorData.spawnEnemyData = _selectedBlock.spawnEnemyData;
            }
        }

        void SaveInspectorValues()
        {
            if (_selectedBlock.spawnEnemyData.enemyType != _spawnerToolInspectorData.spawnEnemyData.enemyType)
            {
                _selectedBlock.SetColor(_spawnerToolInspectorData.GetEnemyColor(_spawnerToolInspectorData.spawnEnemyData.enemyType));
            }

            _selectedBlock.spawnEnemyData = _spawnerToolInspectorData.spawnEnemyData;
        }

        #endregion
    }

    [CustomEditor(typeof(SpawnerToolInspectorData))]
    public class SpawnerToolEditorInspector : Editor
    {
        int selected = 0;
        private bool enemyColorsFoldout = false;

        private void OnEnable()
        {
            SpawnerToolInspectorData sp = target as SpawnerToolInspectorData;

            if (sp == null)
                return;

            ValidateValue(ref sp.spawnEnemyData.howManyEnemies, 1);
            ValidateValue(ref sp.spawnEnemyData.spawnPointID, 0);
            ValidateValue(ref sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
            ValidateValue(ref sp.spawnEnemyData.timeToStartSpawning, 0.0f);
        }

        public override void OnInspectorGUI()
        {
            SpawnerToolInspectorData sp = target as SpawnerToolInspectorData;

            sp.spawnEnemyData.spawnPointID = EditorGUILayout.IntField("Spawn point ID", sp.spawnEnemyData.spawnPointID);
            sp.spawnEnemyData.howManyEnemies =
                EditorGUILayout.IntField("How many enemies", sp.spawnEnemyData.howManyEnemies);

            sp.spawnEnemyData.timeBetweenSpawn =
                EditorGUILayout.FloatField("Time between spawns", sp.spawnEnemyData.timeBetweenSpawn);
            sp.spawnEnemyData.timeToStartSpawning =
                EditorGUILayout.FloatField("Time to start Spawning", sp.spawnEnemyData.timeToStartSpawning);

            List<string> enemyNames = new List<string>();
            foreach (var enemy in sp.enemyInfo)
            {
                enemyNames.Add(enemy.name);
            }
           
            selected = EditorGUILayout.Popup("EnemyType", selected, enemyNames.ToArray());

            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 13;
            title.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField(new GUIContent("SpawnerTool Project Settings", "Here is a tooltip"), title,
                GUILayout.Height(20));
            EditorGUILayout.Space();

            var enemyInfo = serializedObject.FindProperty("enemyInfo");
            EditorGUILayout.PropertyField(enemyInfo, new GUIContent("Enemy list"), true);
            serializedObject.Update();

            CheckEnemyNames(sp);

            //enemyColorsFoldout = EditorGUILayout.Foldout(enemyColorsFoldout, new GUIContent("EnemyColors"));
            /* if (enemyColorsFoldout)
             {
                 if (sp.enemyNames.Count == 0)
                 {
                     EditorGUILayout.HelpBox("There aren't any Enemy Names.", MessageType.Info);
                 }
                 
                 foreach (string enemyName in sp.enemyNames)
                 {
                     string tempName;
                     if (enemyName == string.Empty)
                         tempName = "Unnamed";
                     else
                         tempName = enemyName;
                     
                     
                     sp.enemyColorBlocks[sp.enemyNames.IndexOf(enemyName)] = EditorGUILayout.ColorField(tempName, sp.enemyColorBlocks[sp.enemyNames.IndexOf(enemyName)]);
                 }
             }*/
            if (Event.current.type == EventType.Used || sp.init)
            {
                sp.init = false;
                ValidateValue(ref sp.spawnEnemyData.howManyEnemies, 1);
                ValidateValue(ref sp.spawnEnemyData.spawnPointID, 0);
                ValidateValue(ref sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
                ValidateValue(ref sp.spawnEnemyData.timeToStartSpawning, 0.0f);
            }
        }

        void CheckEnemyNames(SpawnerToolInspectorData sp)
        {
            bool wrong = false;
            foreach (EnemyInfo enemyInfo in sp.enemyInfo)
            {
                if (enemyInfo.name == string.Empty)
                    wrong = true;
            }

            if (wrong)
            {
                EditorGUILayout.HelpBox("There's one empty name. Please delete or change it. Things might not work.",
                    MessageType.Error);
                EditorGUILayout.Space(10);
            }

            wrong = sp.enemyInfo.GroupBy(n => n).Any(g => g.Count() > 1);
            if (wrong)
            {
                EditorGUILayout.HelpBox("There are duplicated names.", MessageType.Error);
                EditorGUILayout.Space(10);
            }
        }

        private void ValidateValue(ref float value, float minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        private void ValidateValue(ref int value, int minValue)
        {
            value = Mathf.Max(value, minValue);
        }
    }
}