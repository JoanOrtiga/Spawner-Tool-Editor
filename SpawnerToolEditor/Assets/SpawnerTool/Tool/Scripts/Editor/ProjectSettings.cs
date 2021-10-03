using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "SpawnerTool/Settings/ProjectSettings")]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private List<string> enemyNames = new List<string>();

        private Dictionary<string, Color> enemyColors = new Dictionary<string, Color>();

        public Color GetEnemyColor(string enemyType)
        {
            return Color.black;
        }

        public List<string> GetEnemyNames()
        {
            return enemyNames;
        }

        public Dictionary<string, Color> GetAllColors()
        {
            return enemyColors;
        }

        public void SetEnemyColor(string enemyType, Color color)
        {
            if (enemyColors.ContainsKey(enemyType))
            {
                enemyColors[enemyType] = color;
                return;
            }
            
            Debug.LogError($"SPAWNERTOOL: {enemyType} does not exists in this ProjectSettings.asset ({this.name}).");
        }
    }
}