using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "SpawnerToolEditorSettings", menuName = "SpawnerTool/EditorSettings", order = 1)]
    public class SpawnerToolEditorSettings : ScriptableObject
    {
        [Header("Configuration Settings")] 
        public int maxRounds;
        public int maxCharactersRounds = 3;
        public float maxTotalTime;
        public int maxCharactersTotalTime = 6;
        public int minTracks = 5;
        public int maxTracks = 100;
        public int maxCharactersTracks = 3;
        
        [Header("Textures")]
        public Texture2D backgroundTexture;
        public Texture2D whiteTexture;
        public Texture2D bin;

        [Header("Colors for enemies")] 
        private List<Color> enemyColor;
        
        
        [System.Serializable]
        public struct EnemyBlockColor
        {
            public string enemyType;
            public Color enemyBlockColor;

            public EnemyBlockColor(string enemyType, Color enemyBlockColor = new Color())
            {
                this.enemyType = enemyType;
                this.enemyBlockColor = enemyBlockColor;
            }
        }

        public List<EnemyBlockColor> enemyBlockColors = new List<EnemyBlockColor>();
        
        public void RandomizeColors()
        {
            for (int i = 0; i < enemyBlockColors.Count; i++)
            {
                EnemyBlockColor enemyBlockColor = enemyBlockColors[i];
                enemyBlockColor.enemyBlockColor = new Color(Random.value, Random.value, Random.value);
                enemyBlockColors[i] = enemyBlockColor;
            }
        }

        public void DeleteAllColors()
        {
            enemyBlockColors.Clear();
        }
    }
    
    [CustomEditor(typeof(SpawnerToolEditorSettings))]
    public class SpawnerToolEditorSettingsInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var spawnerTool = (target as SpawnerToolEditorSettings);

            if (GUILayout.Button("Randomize All Colors"))
            {
                spawnerTool.RandomizeColors();
            }
            if (GUILayout.Button("Clear all colors & enemies"))
            {
                spawnerTool.DeleteAllColors();
            }
        }
    }
}