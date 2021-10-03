using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    public class SpawnerToolInspector
    {
        private SpawnerToolInspectorData _inspectorData;
        private SpawnerToolInspectorData _inspectorWindow;
        
        public SpawnerToolInspector()
        {
            _inspectorData = ProjectConfiguration.Instance.GetSpawnerToolInspectorData();
            CreateInspectorWindow();
        }

        private void CreateInspectorWindow()
        {
            _inspectorWindow = ScriptableObject.CreateInstance<SpawnerToolInspectorData>();
            LoadInspectorData();
            Selection.activeObject = _inspectorWindow;
        }

        public void SaveInspectorData()
        {
            _inspectorData.spawnEnemyData = _inspectorWindow.spawnEnemyData;
        }
        
        private void LoadInspectorData()
        {
            _inspectorWindow.spawnEnemyData = _inspectorData.spawnEnemyData;
        }
    }
}