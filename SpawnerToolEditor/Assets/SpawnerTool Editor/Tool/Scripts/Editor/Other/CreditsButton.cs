using UnityEditor;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    public class CreditsButton : EditorWindow
    {
        [MenuItem("Tools/SpawnerTool/Credits", priority = 102)]
        public static void OpenCreditsWindow()
        {
            CreditsButton window = EditorWindow.CreateInstance<CreditsButton>();
            Vector2 main = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            Rect pos = window.position;
            float centerWidth = (main.x - pos.width) * 0.5f;
            float centerHeight = (main.y - pos.height) * 0.5f;
            pos.x = main.x + centerWidth;
            pos.y = main.y + centerHeight;
            window.position = pos;
            window.name = "Spawner Tool Credits";
            window.titleContent = new GUIContent("Credits");
            window.minSize = new Vector2(300, 390);
            window.position = new Rect(window.position.position, new Vector2(300, 390));
            window.ShowModal();
        }
        
        void OnGUI()
        {
            GUIStyle style = new GUIStyle("label")
            {
                alignment = TextAnchor.MiddleCenter
            };

            GUIStyle nameStyle = new GUIStyle(style)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("This tool has been made by:", style);
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("JOAN ORTIGA BALCELLS", nameStyle);
            GUILayout.Space(30);
            
            if(GUILayout.Button("Write a Review!",GUILayout.Height(30)))
                Application.OpenURL("");
            GUILayout.Space(30);
            if(GUILayout.Button("Personal Page", GUILayout.Height(30)))
                Application.OpenURL("https://joanortiga.site/");
            if(GUILayout.Button("LinkedIN", GUILayout.Height(30)))
                Application.OpenURL("www.linkedin.com/in/joanortigabalcells");
            if(GUILayout.Button("Twitter", GUILayout.Height(30)))
                Application.OpenURL("https://twitter.com/joanortigadev");
            if(GUILayout.Button("GitHub", GUILayout.Height(30)))
                Application.OpenURL("https://github.com/JoanOrtiga");
            if(GUILayout.Button("Itch.io",GUILayout.Height(30)))
                Application.OpenURL("https://joanstark.itch.io/");
            GUILayout.Space(30);
            if (GUILayout.Button("Agree!", GUILayout.Height(30))) 
                this.Close();
        }
    }
}