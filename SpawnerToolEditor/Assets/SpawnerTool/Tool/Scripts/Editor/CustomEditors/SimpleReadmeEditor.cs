using System;
using UnityEditor;

namespace SpawnerTool
{
    [CustomEditor(typeof(SimpleReadme))]
    public class SimpleReadmeEditor : Editor
    {
        private SimpleReadme _simpleReadme;

        private void OnEnable()
        {
            _simpleReadme = target as SimpleReadme;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}

