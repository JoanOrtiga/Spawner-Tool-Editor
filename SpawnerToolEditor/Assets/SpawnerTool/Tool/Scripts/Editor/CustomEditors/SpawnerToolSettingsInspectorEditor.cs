using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnerToolEditorSettings))]
    public class SpawnerToolSettingsInspectorEdito : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}