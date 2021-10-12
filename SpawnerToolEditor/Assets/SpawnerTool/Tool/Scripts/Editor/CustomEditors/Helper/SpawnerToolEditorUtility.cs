using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    public static class SpawnerToolEditorUtility
    {
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