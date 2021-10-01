using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CustomEditor(typeof(EnemyFactory))]
    public class EnemyFactorCustomInspector : Editor
    {
        private EnemyFactory _enemyFactory;
        private ProjectSettings _projectSettings;

        private void OnEnable()
        {
            _enemyFactory = target as EnemyFactory;
        }

        public override void OnInspectorGUI()
        {
            _projectSettings = EditorGUILayout.ObjectField(new GUIContent("Project Settings"), _projectSettings, typeof(ProjectSettings), true) as ProjectSettings;

            EditorGUILayout.Space(10);
            GUIStyle title = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15
            };

            EditorGUILayout.LabelField("Prefabs: ", title);
            if(_projectSettings != null)
            {
                foreach (var enemyName in _projectSettings.GetEnemyNames())
                {
                    if (_enemyFactory.GetIdToPrefab().ContainsKey(enemyName))
                    {
                        _enemyFactory.GetIdToPrefab()[enemyName] = EditorGUILayout.ObjectField(new GUIContent(enemyName), _enemyFactory.GetIdToPrefab()[enemyName], typeof(GameObject),true) as GameObject;
                        Repaint();
                    }
                    else
                    {
                        _enemyFactory.GetIdToPrefab().Add(enemyName, null);
                    }
                } 
            }
            
            serializedObject.Update();
        }
    }
}