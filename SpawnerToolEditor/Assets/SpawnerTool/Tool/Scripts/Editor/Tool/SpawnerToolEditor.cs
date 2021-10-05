using System;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    public class SpawnerToolEditor : EditorWindow
    {
        #region Variables

        private static readonly Vector2 _playgroundSize = new Vector2(500, 500);
        public static Vector2 MarginToPlaygroundSize { get; } = new Vector2(57, 58);
        private static readonly float _scrollbarSize = 13;
        private static readonly float _scrollBarPixelSize = 13;
        private static readonly float _unityWindowPixelBug = 6;

        //CELLS
        public static readonly float CellXPercentatge = 0.2f;
        public static readonly Vector2 CellSize = new Vector2(100, 100);
        
        public SpawnerToolEditor SpawnerToolEditorWindow { get; set; }
        private SpawnerToolInspector _spawnerToolInspector;
        private SpawnerToolGraphController _spawnerToolGraphController;
        private Playground _playground;
        public SpawnerToolEditorSettings EditorSettings { get; set; }

        #endregion
        
        public int CurrentRound { get; set; }


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
                    spawnerToolEditor._spawnerToolGraphController.ChangeGraph(spawnerGraph);
                }
                return;
            }

            CreateWindow(spawnerToolEditor);
        }
        
        private static void CreateWindow(SpawnerToolEditor sp)
        {
            EditorWindow.CreateWindow<SpawnerToolEditor>("SpawnerTool");
            sp._spawnerToolEditor = EditorWindow.GetWindow<SpawnerToolEditor>(false, "Spawner Tool", true);

            sp.minSize = _playgroundSize + MarginToPlaygroundSize +
                new Vector2(_scrollbarSize, _scrollbarSize)- new Vector2(0, _unityWindowPixelBug);
            sp.wantsMouseEnterLeaveWindow = true;
            sp.wantsMouseMove = true;
        }

        private static bool CheckIfWindowAlreadyExists(out SpawnerToolEditor spawnerToolEditor)
        {
            EditorWindow.FocusWindowIfItsOpen<SpawnerToolEditor>();
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
    }
}