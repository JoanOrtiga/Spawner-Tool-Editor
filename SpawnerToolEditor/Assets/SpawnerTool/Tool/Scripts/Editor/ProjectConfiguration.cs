using System;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [InitializeOnLoad]
    [CreateAssetMenu(fileName = "ProjectConfiguration", menuName = "SpawnerTool/Settings/ProjectConfiguration")]
    public class ProjectConfiguration : ScriptableObject
    {
        [SerializeField] private ProjectSettings _projectSettings;
        [SerializeField] private SpawnerToolEditorSettings _spawnerToolEditorSettings;
        [SerializeField] private EnemyFactory _enemyFactory;

        private SpawnerToolInspectorData _spawnerToolInspectorData;

        private static ProjectConfiguration _instance;

        public static ProjectConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(ProjectConfiguration));
                    if (guids.Length == 0)
                    {
                        Debug.LogError(
                            "SPAWNERTOOL: No ProjectConfiguration.asset found. Make sure there's 1 ProjectConfiguration.asset");
                    }

                    if (guids.Length > 1)
                    {
                        Debug.LogWarning(
                            "SPAWNERTOOL: More than 1 ProjectConfiguration.asset found. That may cause problems. Make sure only original 'SpawnerInspectorData.asset' is in the project.");
                    }

                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance =
                        AssetDatabase.LoadAssetAtPath(path, typeof(ProjectConfiguration)) as ProjectConfiguration;

                    if (_instance == null)
                    {
                        Debug.LogError("SPAWNERTOOL: ProjectConfiguration.asset not found");
                    }
                    else
                    {
                        _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    }
                }

                return _instance;
            }
            set { _instance = value; }
        }

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
                    Debug.LogError(
                        "SPAWNERTOOL: No spawner inspector data found. Make sure original 'SpawnerInspectorData.asset' is in the project.");
                    return null;
                }

                if (guids.Length > 1)
                {
                    Debug.LogWarning(
                        "SPAWNERTOOL: More than one spawner inspector data found. That may cause problems. Make sure only original 'SpawnerInspectorData.asset' is in the project.");
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