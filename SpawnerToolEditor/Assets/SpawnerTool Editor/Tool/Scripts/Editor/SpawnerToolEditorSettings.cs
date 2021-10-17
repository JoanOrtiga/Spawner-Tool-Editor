using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnerTool.EditorScripts
{
    [CreateAssetMenu(fileName = "SpawnerToolEditorSettings", menuName = "SpawnerTool/Settings/Internal/EditorSettings", order = 0)]
    public class SpawnerToolEditorSettings : ScriptableObject
    {
        [Header("Rounds"), Header("Configuration Settings")] 
        [Min(0), Tooltip("Max rounds you can set on the editor")] public int maxRounds;
        [Min(2), Tooltip("Max characters you can write in round field.")] public int maxCharactersRounds = 3;
        
        [Header("TotalTime")]
        [Min(1.0f), Tooltip("Min total time a round should have")] public float minTotalTime = 1;
        [Min(1.0f), Tooltip("Default time when creating a round")] public float DefaultTotalTime = 25.0f;
        [Min(1.0f), Tooltip("Max total time a round can have")] public float maxTotalTime;
        [Min(5), Tooltip("Max characters you can write in Total time field")] public int maxCharactersTotalTime = 6;
        
        [Header("Tracks")]
        [Min(5), Tooltip("Min tracks a round can have")] public int minTracks = 5;
        [Min(5), Tooltip("Default tracks when creating a round")] public int DefaultTracks = 5;
        [Min(5), Tooltip("Max tracks a round can have")] public int maxTracks = 100;
        [Min(2), Tooltip("Max characters you can write in tracks field")] public int maxCharactersTracks = 3;
        
        [Header("Colors for enemies")]
        [SerializeField, ColorUsage(false,false)] private Color blockSelectionColor = Color.yellow;
        private static Color s_blockSelectionColor;
        
        
        [Header("Textures")]
        [Header("You shouldn't modify this")]
        public Texture2D backgroundTexture;
        public Texture2D whiteTexture;
        public Texture2D bin;

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