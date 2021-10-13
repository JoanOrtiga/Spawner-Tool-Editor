using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
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
    }
}