using UnityEngine;
using UnityEditor;

namespace SpawnerTool.EditorScripts.Readme
{
    [CustomEditor(typeof(SimpleReadme))]
    public class SimpleReadmeCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            SimpleReadme simpleReadme = target as SimpleReadme;

            GUIStyle image = new GUIStyle(GUI.skin.label);
            image.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(new GUIContent(simpleReadme.Icon, "Undefined Behaviour"), image, GUILayout.Height(250));
            //EditorGUILayout.LabelField(new GUIContent(generalInfo.TopImage, "Undefined Behaviour"), GUILayout.Width(100), GUILayout.Height(100));
            GUIStyle label = new GUIStyle(GUI.skin.label);
            label.alignment = TextAnchor.MiddleCenter;
            label.fontStyle = FontStyle.Bold;
            label.fontSize = 18;
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Created by Joan Ortiga", label, GUILayout.Height(25));
            EditorGUILayout.Space(3);
            label.fontStyle = FontStyle.Bold;
            label.fontSize = 14;
            EditorGUILayout.LabelField("Contact me here if you need it: joanorba@gmail.com", label);
            EditorGUILayout.Space(10);
            
            GUIStyle style = new GUIStyle("label")
            {
                wordWrap = true
            };
            GUILayout.Label(simpleReadme.Description, style);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = new RectOffset(buttonStyle.margin.left, 15, buttonStyle.margin.top,
                buttonStyle.margin.bottom);

            DrawUILine(Color.white, 2, 15);
            foreach (Link link in simpleReadme.Links)
            {
                if (GUILayout.Button(link.name, buttonStyle, GUILayout.Height(30)))
                    Application.OpenURL(link.link);
            }
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= padding + 5;
            r.width += padding + 10;
            EditorGUI.DrawRect(r, color);
        }
    }
}