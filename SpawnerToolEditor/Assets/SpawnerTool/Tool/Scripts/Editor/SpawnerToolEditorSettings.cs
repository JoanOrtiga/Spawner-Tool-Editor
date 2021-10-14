using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "SpawnerToolEditorSettings", menuName = "SpawnerTool/Settings/EditorSettings", order = 2500)]
    public class SpawnerToolEditorSettings : ScriptableObject
    {
        [Header("Rounds"), Header("Configuration Settings")] 
        public int maxRounds;
        public int maxCharactersRounds = 3;
        
        [Header("TotalTime")]
        [Min(1.0f)] public float minTotalTime = 1;
        public float DefaultTotalTime = 25.0f;
        public float maxTotalTime;
        public int maxCharactersTotalTime = 6;
        
        [Header("Tracks")]
        public int minTracks = 5;
        public int DefaultTracks = 5;
        public int maxTracks = 100;
        public int maxCharactersTracks = 3;
        
        [Header("Textures")]
        public Texture2D backgroundTexture;
        public Texture2D whiteTexture;
        public Texture2D bin;
        
        [Header("Colors for enemies")]
        [SerializeField, ColorUsage(false,false)] private Color blockSelectionColor = Color.yellow;
        private static Color s_blockSelectionColor;

        private void OnEnable()
        {
            s_blockSelectionColor = blockSelectionColor;
        }

        private void OnValidate()
        {
            s_blockSelectionColor = blockSelectionColor;
        }

        public static Color GetBlockSelectionColor()
        {
            return s_blockSelectionColor;
        }
    }
}