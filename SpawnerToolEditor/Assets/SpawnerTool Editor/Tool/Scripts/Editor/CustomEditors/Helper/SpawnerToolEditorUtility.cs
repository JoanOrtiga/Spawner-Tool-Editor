using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    public static class SpawnerToolEditorUtility
    {
        public static void ValidateValue(float value, float minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        public static void ValidateValue(int value, int minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        public static bool DrawDefaultInspectorWithoutScriptField(this Editor Inspector)
        {
            EditorGUI.BeginChangeCheck();

            Inspector.serializedObject.Update();

            SerializedProperty Iterator = Inspector.serializedObject.GetIterator();

            Iterator.NextVisible(true);

            while (Iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(Iterator, true);
            }

            Inspector.serializedObject.ApplyModifiedProperties();

            return (EditorGUI.EndChangeCheck());
        }
    }
}
