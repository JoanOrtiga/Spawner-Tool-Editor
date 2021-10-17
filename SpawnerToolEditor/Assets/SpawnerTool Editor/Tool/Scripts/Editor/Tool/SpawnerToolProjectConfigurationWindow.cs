using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    public class SpawnerToolProjectConfigurationWindow : EditorWindow
    {
        [SerializeField] private SpawnerToolProjectConfigurationWindow _projectConfigurationWindow;
        [SerializeField] private ProjectConfiguration _projectConfiguration;
        
        [MenuItem("Tools/SpawnerTool/ProjectConfiguration", priority = 50)]
        private static void ShowWindow()
        {
            InitializeWindow();
        }

        private static void InitializeWindow()
        {
            SpawnerToolProjectConfigurationWindow projectConfigurationWindow;
            if (CheckIfWindowAlreadyExists(out projectConfigurationWindow))
            {
                if (projectConfigurationWindow._projectConfiguration == null)
                {
                    projectConfigurationWindow._projectConfiguration = LoadProjectConfiguration();
                }

                return;
            }

            CreateWindow(ref projectConfigurationWindow);
            projectConfigurationWindow._projectConfiguration = LoadProjectConfiguration();
        }

        private static void CreateWindow(ref SpawnerToolProjectConfigurationWindow sp)
        {
            UnityEditor.EditorWindow.CreateWindow<SpawnerToolProjectConfigurationWindow>("ProjectConfiguration");
            sp = UnityEditor.EditorWindow.GetWindow<SpawnerToolProjectConfigurationWindow>(false, "ProjectConfiguration", true);
            sp._projectConfigurationWindow = sp;

            sp.minSize = new Vector2(180, 110);
        }

        private static bool CheckIfWindowAlreadyExists(out SpawnerToolProjectConfigurationWindow projectConfigurationWindow)
        {
            UnityEditor.EditorWindow.FocusWindowIfItsOpen<SpawnerToolProjectConfigurationWindow>();
            if (focusedWindow == null)
            {
                projectConfigurationWindow = null;
                return false;
            }

            if (focusedWindow.GetType() == typeof(SpawnerToolProjectConfigurationWindow))
            {
                projectConfigurationWindow = focusedWindow as SpawnerToolProjectConfigurationWindow;
                return true;
            }

            projectConfigurationWindow = null;
            return false;
        }

        public static ProjectConfiguration LoadProjectConfiguration()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ProjectConfiguration));
            if (guids.Length == 0)
            {
                Debug.LogError(
                    "SPAWNERTOOL: No ProjectConfiguration.asset found. Make sure there's only 1 and original ProjectConfiguration.asset");
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning(
                    "SPAWNERTOOL: More than 1 ProjectConfiguration.asset found. That may cause problems. Make sure only original 'ProjectConfiguration.asset' is in the project.");
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            ProjectConfiguration projectConfiguration = AssetDatabase.LoadAssetAtPath(path, typeof(ProjectConfiguration)) as ProjectConfiguration;

            if (projectConfiguration == null)
            {
                Debug.LogError("SPAWNERTOOL: ProjectConfiguration.asset not found");
            }
            else
            {
                projectConfiguration.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            
            return projectConfiguration;
        }
        
        private void OnGUI()
        {
            GUIStyle title = new GUIStyle("label")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };
            GUILayout.Space(15);
            GUILayout.Label("Project Configuration", title);
            GUILayout.Space(15);

            Editor projectConfigurationEditor = Editor.CreateEditor(_projectConfiguration);
            projectConfigurationEditor.DrawDefaultInspectorWithoutScriptField();
        }
    }
}