using UnityEngine;
using UnityEditor;

namespace SpawnerTool
{
    public static class SpawnerToolsUtility
    {
        public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
        {
            float t = Mathf.InverseLerp(oldLow, oldHigh, input);
            return Mathf.Lerp(newLow, newHigh, t);
        }
        
        public static void DrawString(string text, Vector3 worldPos, Color color, int fontSize = 20, float oX = 0, float oY = 0)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.BeginGUI();
        
        
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width ||
                screenPos.z < 0)
            {
                UnityEditor.Handles.EndGUI();
                return;
            }

            GUIStyle style = new GUIStyle();
            style.fontSize = fontSize;
            style.normal.textColor = color;
        

            UnityEditor.Handles.Label(TransformByPixel(worldPos, oX, oY), text, style);
        
            UnityEditor.Handles.EndGUI();
#endif
        }

        static Vector3 TransformByPixel(Vector3 position, float x, float y)
        {
            return TransformByPixel(position, new Vector3(x, y));
        }

        static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy)
        {
            Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
            if (cam)
                return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
            else
                return position;
        }
    }
}