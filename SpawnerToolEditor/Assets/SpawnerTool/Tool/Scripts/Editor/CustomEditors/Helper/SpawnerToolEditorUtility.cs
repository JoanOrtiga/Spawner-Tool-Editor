using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    public static class SpawnerToolEditorUtility
    {
        public static void ListToGUI(Editor editor, SerializedProperty names, Dictionary<string, Color> dictionary, string itemType,
            ref bool visible)
        {
            visible = EditorGUILayout.Foldout(visible, itemType);
            if (visible)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                for (int i = 0; i < names.arraySize; i++)
                {
                    string enemyName = names.GetArrayElementAtIndex(i).stringValue;
                    if (!dictionary.ContainsKey(enemyName))
                    {
                        dictionary.Add(enemyName, Color.black);
                        editor.Repaint();
                    }

                    dictionary[enemyName] = EditorGUILayout.ColorField(new GUIContent(enemyName),
                        dictionary[enemyName], true, false, false);
                }

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
        
        public static void ValidateValue(ref float value, float minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        public static void ValidateValue(ref int value, int minValue)
        {
            value = Mathf.Max(value, minValue);
        }
    }
}