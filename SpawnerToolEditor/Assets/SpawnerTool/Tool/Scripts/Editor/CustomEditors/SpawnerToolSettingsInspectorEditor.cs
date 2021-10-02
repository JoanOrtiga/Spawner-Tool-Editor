using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnerToolEditorSettings))]
    public class SpawnerToolSettingsInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}