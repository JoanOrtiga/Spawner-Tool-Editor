using System;
using UnityEditor;

namespace SpawnerTool
{
    [CustomEditor(typeof(SpawnerToolConfigurationExampleReadme))]
    public class SpawnerToolConfigurationExampleReadmeEditor : Editor
    {
        private SpawnerToolConfigurationExampleReadme _spawnerToolConfigurationExampleReadme;

        private void OnEnable()
        {
            _spawnerToolConfigurationExampleReadme = target as SpawnerToolConfigurationExampleReadme;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}

