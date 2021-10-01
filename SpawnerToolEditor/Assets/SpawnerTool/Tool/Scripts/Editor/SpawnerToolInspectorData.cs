using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "SpawnerInspectorData", menuName = "SpawnerTool/Settings/SpawnerInspectorData", order = 2500)]
    public class SpawnerToolInspectorData : ScriptableObject
    {
        public SpawnEnemyData spawnEnemyData = new SpawnEnemyData();
        public bool init = false;
        public List<EnemyInfo> enemyInfo = new List<EnemyInfo>();

        public Color GetEnemyColor(string enemyName)
        {
            foreach (var enemyInfo in enemyInfo)
            {
                if (enemyInfo.name == enemyName)
                {
                    return enemyInfo.blockColor;
                }
            }
            
            return Color.white;
        }

        public void SetEnemyColor(string enemyName, Color enemyBlockColor)
        {
            for (int i = 0; i < enemyInfo.Count; i++)
            {
                EnemyInfo temp = enemyInfo[i];
                if (temp.name == enemyName)
                {
                    temp.blockColor = enemyBlockColor;
                    enemyInfo[i] = temp;
                }
            }
        }

        public string CheckEnemyName(string enemyName)
        {
            bool exists = false;
            foreach (var enemy in enemyInfo)
            {
                if (enemyName == enemy.name)
                {
                    exists = true;
                }              
            }

            if (!exists)
                enemyName = SpawnerToolInspectorDataEditor.Unnamed;

            return enemyName;
        }
    }

    [Serializable]
    public struct EnemyInfo
    {
        public string name;
        public Color blockColor;

        public EnemyInfo(string name, Color color)
        {
            this.name = name;
            this.blockColor = color;
        }
    }
}