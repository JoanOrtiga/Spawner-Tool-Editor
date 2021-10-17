using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.Data
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/data/projectsettings")]
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "SpawnerTool/Settings/ProjectSettings", order = -100)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private List<string> enemyNames = new List<string>();
        [SerializeField] private List<Color> enemyColors = new List<Color>();
        
        private Dictionary<string, Color> enemyColorByType = new Dictionary<string, Color>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetEnemyNames()
        {
            return enemyNames;
        }
        
        public Dictionary<string, Color> EnemyColorByType
        {
            get => enemyColorByType;
            set => enemyColorByType = value;
        }

        /// <summary>
        /// Returns color given enemyType. If enemyType is not found, it returns White color.
        /// </summary>
        /// <param name="enemyType"></param>
        /// <returns>Color by enemy</returns>
        public Color GetEnemyColor(string enemyType)
        {
            if(!enemyColorByType.ContainsKey(enemyType)) 
                return Color.white;
            return enemyColorByType[enemyType];
        }
        
        /// <summary>
        /// Changes the given color for the given enemyType.
        /// </summary>
        /// <param name="enemyType">Type of enemy you want to change color</param>
        /// <param name="color">The color you want to assing to enemyType</param>
        public void SetEnemyColor(string enemyType, Color color)
        {
            if (enemyColorByType.ContainsKey(enemyType))
            {
                enemyColorByType[enemyType] = color;
                return;
            }
            
            Debug.LogError($"SPAWNERTOOL: {enemyType} does not exists in this ProjectSettings.asset ({this.name}).");
        }

        #region Tool
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
            for (int i = enemyColors.Count-1; i < enemyNames.Count-1; i++)
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

        private void OnDestroy()
        {
            SaveToList();
        }

        private void OnDisable()
        {
            SaveToList();
        }
        
        #endregion
    }
}