using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpawnerTool.EditorScripts
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
            //EditorGUILayout.LabelField("Marina Chavarria & Joan Ortiga", label);
            EditorGUILayout.Space(10);

            GUILayout.Label(simpleReadme.Description);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = new RectOffset(buttonStyle.margin.left, 15, buttonStyle.margin.top,
                buttonStyle.margin.bottom);

            DrawUILine(Color.white, 2, 15);
            /*foreach (Link link in generalInfo.ProjectLinks)
            {
                if (GUILayout.Button(link.Name, buttonStyle, GUILayout.Height(30)))
                    Application.OpenURL(link.URL);
            }

            DrawUILine(Color.white, 2, 15);
            foreach (Link link in generalInfo.UndefinedBehaviourLinks)
            {
                if (GUILayout.Button(link.Name, buttonStyle, GUILayout.Height(30)))
                    Application.OpenURL(link.URL);
            }*/
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