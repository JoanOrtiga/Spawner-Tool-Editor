using System;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [Serializable]
    public class SpawnerToolInspector
    {
        [SerializeField] private SpawnerToolEditor _spawnerToolEditor;
        [SerializeField] private SpawnerToolInspectorData _inspectorData;
        [SerializeField] private SpawnerToolInspectorData _inspectorWindow;
        public SpawnerToolInspectorData InspectorWindow
        {
            get
            {
                if (_inspectorWindow == null)
                {
                    CreateInspectorWindow();
                }
                
                return _inspectorWindow;
            }
        }

        public SpawnerToolInspector(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
            _inspectorData = ProjectConfiguration.Instance.GetSpawnerToolInspectorData();
            CreateInspectorWindow();
        }

        private void CreateInspectorWindow()
        {
            _inspectorWindow = ScriptableObject.CreateInstance<SpawnerToolInspectorData>();
            LoadInspectorData();
            Selection.activeObject = _inspectorWindow;
        }
        
        public void UpdateInspector()
        {
            if (_spawnerToolEditor.SpawnerBlockController.GetSelectedBlock != null)
            {
                _inspectorWindow.spawnEnemyData = _spawnerToolEditor.SpawnerBlockController.GetSelectedBlock.spawnEnemyData;
            }
        }

        public void SaveInspectorData()
        {
            _inspectorData.spawnEnemyData = _inspectorWindow.spawnEnemyData;
        }
        
        private void LoadInspectorData()
        {
            _inspectorWindow.spawnEnemyData = _inspectorData.spawnEnemyData;
        }

        public void OnDisable()
        {
            SaveInspectorData();
        }

        public void Input(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                Selection.activeObject = _inspectorWindow;
            }
        }

        public void Update()
        {
            _inspectorWindow.selected = _spawnerToolEditor.SpawnerBlockController.SomethingSelected();
        }
    }
}