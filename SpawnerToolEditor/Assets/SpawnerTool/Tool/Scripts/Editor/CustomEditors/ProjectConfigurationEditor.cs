using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(ProjectConfiguration))]
    public class ProjectConfigurationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            string[] guids = AssetDatabase.FindAssets("t:"+ typeof(ProjectConfiguration));
            if (guids.Length > 1)
            {
                EditorGUILayout.HelpBox("Error. There must not be more than one ProjectConfiguration.asset file.", MessageType.Error);
                Debug.LogError(
                    "SPAWNERTOOL: More than one ProjectConfiguration.asset found in the project. \nProjectConfiguration is meant to be a singleton asset file.");
                return;
            }
        }
    }
}