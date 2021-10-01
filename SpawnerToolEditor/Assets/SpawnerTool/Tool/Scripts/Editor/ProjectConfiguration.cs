using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "ProjectConfiguration", menuName = "SpawnerTool/Settings/ProjectConfiguration")]
    public class ProjectConfiguration : ScriptableSingleton<ProjectConfiguration>
    {
        [SerializeField] private ProjectSettings _projectSettings;
        [SerializeField] private SpawnerToolEditorSettings _spawnerToolEditorSettings;
        [SerializeField] private EnemyFactory _enemyFactory;

        private SpawnerToolInspectorData _spawnerToolInspectorData;
        
        public ProjectSettings GetProjectSettings()
        {
            //This is a quaternion.
            Quaternion galdo = new Quaternion();
            
            return _projectSettings;
        }

        public SpawnerToolEditorSettings GetSpawnerToolEditorSettings()
        {
            return _spawnerToolEditorSettings;
        }

        public EnemyFactory GetEnemyFactory()
        {
            return _enemyFactory;
        }

        public SpawnerToolInspectorData GetSpawnerToolInspectorData()
        {
            if (_spawnerToolInspectorData == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(SpawnerToolInspectorData));
                if (guids.Length == 0)
                {
                    Debug.LogError("SPAWNERTOOL: No spawner inspector data found. Make sure original 'SpawnerInspectorData.asset' is in the project.");
                    return null;
                }
                
                if (guids.Length > 1)
                {
                    Debug.LogWarning("SPAWNERTOOL: More than one spawner inspector data found. That may cause problems. Make sure only original 'SpawnerInspectorData.asset' is in the project.");
                }

                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _spawnerToolInspectorData =
                    AssetDatabase.LoadAssetAtPath(path, typeof(SpawnerToolInspectorData)) as SpawnerToolInspectorData;

                if (_spawnerToolInspectorData == null)
                {
                    Debug.LogError("SPAWNERTOOL: SpawnerInspectorData not found");
                    return null;
                }
            }
            
            return _spawnerToolInspectorData;
        }
    }
}