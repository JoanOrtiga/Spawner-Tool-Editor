using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    public class CreditsButton : EditorWindow
    {
        [MenuItem("SpawnerTool/Credits", priority = 12)]
        public static void OpenCreditsWindow()
        {
            CreditsButton window = EditorWindow.CreateInstance<CreditsButton>();
            Rect main = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = window.position;
            float centerWidth = (main.width - pos.width) * 0.5f;
            float centerHeight = (main.height - pos.height) * 0.5f;
            pos.x = main.x + centerWidth;
            pos.y = main.y + centerHeight;
            window.position = pos;
            window.name = "Spawner Tool Credits";
            window.ShowModal();
        }
        
        void OnGUI()
        {
            GUIStyle style = new GUIStyle("label");
            style.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("This tool has been made by:", style);
            EditorGUILayout.LabelField("JOAN ORTIGA BALCELLS", style);
            GUILayout.Space(70);
            if (GUILayout.Button("Agree!")) this.Close();
        }
    }
}