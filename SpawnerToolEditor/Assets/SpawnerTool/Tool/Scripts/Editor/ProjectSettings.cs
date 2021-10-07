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
        [SerializeField] private List<Color> enemyColors = new List<Color>();
        
        private Dictionary<string, Color> enemyColorByType = new Dictionary<string, Color>();
        
        public Color GetEnemyColor(string enemyType)
        {
            if(!enemyColorByType.ContainsKey(enemyType)) 
                return Color.white;
            return enemyColorByType[enemyType];//
        }

        public List<string> GetEnemyNames()
        {
            return enemyNames;
        }

        public Dictionary<string, Color> EnemyColorByType => enemyColorByType;
        
        public void SetEnemyColor(string enemyType, Color color)
        {
            if (enemyColorByType.ContainsKey(enemyType))
            {
                enemyColorByType[enemyType] = color;
                return;
            }
            
            Debug.LogError($"SPAWNERTOOL: {enemyType} does not exists in this ProjectSettings.asset ({this.name}).");
        }

        public void SaveToList()
        {
            enemyColors.Clear();
            foreach (var color in enemyColorByType.Values)
            {
                enemyColors.Add(color);
            }
        }

        public void LoadFromLists()
        {
            for (int i = enemyColors.Count-1; i < enemyNames.Count; i++)
            {
                enemyColors.Add(Color.black);
            }
            
            enemyColorByType.Clear();
            for (int i = 0; i < enemyNames.Count; i++)
            {
                enemyColorByType.Add(enemyNames[i], enemyColors[i]);
            }
        }

        private void OnEnable()
        {
            LoadFromLists();
        }

        private void OnDisable()
        {
            SaveToList();
        }
    }
}