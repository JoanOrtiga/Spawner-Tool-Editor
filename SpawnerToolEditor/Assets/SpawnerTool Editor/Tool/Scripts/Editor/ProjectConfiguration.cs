using System;
using SpawnerTool.Data;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [CreateAssetMenu(fileName = "ProjectConfiguration", menuName = "SpawnerTool/Settings/Internal/ProjectConfiguration", order = 0)]
    [HelpURL("https://joanorba.gitbook.io/spawnertool-editor/getting-started/first-steps")]
    public class ProjectConfiguration : ScriptableObject
    {
        [SerializeField, Tooltip("Drag your project settings here")] private ProjectSettings _projectSettings;
        [SerializeField, Tooltip("Drag your Enemy Factory here")] private EnemyFactory _enemyFactory;
        
        [SerializeField, HideInInspector] private SpawnerToolEditorSettings _spawnerToolEditorSettings;
        [SerializeField, HideInInspector] private SpawnerToolInspectorData _spawnerToolInspectorData;
        
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
            //Easter Egg: Quaternion galdo = new Quaternion();
            return _projectSettings;
        }

        public SpawnerToolEditorSettings GetSpawnerToolEditorSettings()
        {
            if (_spawnerToolEditorSettings == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(SpawnerToolEditorSettings));
                if (guids.Length == 0)
                {
                    Debug.LogError(
                        "SPAWNERTOOL: No SpawnerToolEditorSettings.asset found. Make sure original 'SpawnerToolEditorSettings.asset' is in the project.");
                    return null;
                }

                if (guids.Length > 1)
                {
                    Debug.LogWarning(
                        "SPAWNERTOOL: More than one SpawnerToolEditorSettings.asset found. That may cause problems. Make sure only original 'SpawnerToolEditorSettings.asset' is in the project.");
                }

                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _spawnerToolEditorSettings =
                    AssetDatabase.LoadAssetAtPath(path, typeof(SpawnerToolEditorSettings)) as SpawnerToolEditorSettings;

                if (_spawnerToolEditorSettings == null)
                {
                    Debug.LogError("SPAWNERTOOL: SpawnerToolEditorSettings.asset not found");
                    return null;
                }
            }

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