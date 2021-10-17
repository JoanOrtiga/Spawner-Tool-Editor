using System.Collections.Generic;
using SpawnerTool.Data;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [CreateAssetMenu(fileName = "SpawnerInspectorData", menuName = "SpawnerTool/Settings/Internal/SpawnerInspectorData", order = 0)]
    public class SpawnerToolInspectorData : ScriptableObject
    {
        public SpawnEnemyData spawnEnemyData = new SpawnEnemyData();
        public bool init = false;
        public bool selected = false;
        
      /*  public List<EnemyInfo> enemyInfo = new List<EnemyInfo>();

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
*/
        
      
        public string CheckEnemyName(string enemyName)
        {
            bool exists = false;
            foreach (var enemy in ProjectConfiguration.Instance.GetProjectSettings().GetEnemyNames())
            {
                if (enemyName == enemy)
                {
                    exists = true;
                }              
            }

            if (!exists)
                enemyName = SpawnerToolInspectorDataEditor.Unnamed;

            return enemyName;
        }
    }
}