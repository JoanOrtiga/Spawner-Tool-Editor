using UnityEngine;

namespace SpawnerTool
{
    public class SpawnerToolEditorData : ScriptableObject
    {
        public SpawnerToolEditor SpawnerToolEditorWindow { get; set; }

        public void SaveSpawnerToolEditorState(SpawnerToolEditor spawnerToolEditor)
        {
            SpawnerToolEditorWindow = spawnerToolEditor.SpawnerToolEditorWindow;
        }
        
        public void LoadSpawnerToolEditorState(SpawnerToolEditor spawnerToolEditor)
        {
            spawnerToolEditor.SpawnerToolEditorWindow = SpawnerToolEditorWindow;
        }
    }
}