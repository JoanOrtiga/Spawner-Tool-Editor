using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

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
                enemyName = SpawnerToolEditorInspector.unnamed;

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

    [CustomEditor(typeof(SpawnerToolInspectorData))]
    public class SpawnerToolEditorInspector : Editor
    {
        private SpawnerToolInspectorData sp;
        int selected = 0;

        public const string unnamed = "No type";

        private void OnEnable()
        {
            sp = target as SpawnerToolInspectorData;

            if (sp == null)
                return;

            ValidateValue(ref sp.spawnEnemyData.howManyEnemies, 1);
            ValidateValue(ref sp.spawnEnemyData.spawnPointID, 0);
            ValidateValue(ref sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
            ValidateValue(ref sp.spawnEnemyData.timeToStartSpawning, 0.0f);
        }

        public override void OnInspectorGUI()
        {
            if (Event.current.type == EventType.Used || sp.init)
            {
                sp.init = false;
                sp.spawnEnemyData.enemyType = unnamed;
                ValidateValue(ref sp.spawnEnemyData.howManyEnemies, 1);
                ValidateValue(ref sp.spawnEnemyData.spawnPointID, 0);
                ValidateValue(ref sp.spawnEnemyData.timeBetweenSpawn, 0.01f);
                ValidateValue(ref sp.spawnEnemyData.timeToStartSpawning, 0.0f);
            }

            DrawEnemyData();
            DrawToolSettings();
        }

        private void DrawEnemyData()
        {
            sp.spawnEnemyData.spawnPointID = EditorGUILayout.IntField("Spawn point ID", sp.spawnEnemyData.spawnPointID);
            sp.spawnEnemyData.howManyEnemies =
                EditorGUILayout.IntField("How many enemies", sp.spawnEnemyData.howManyEnemies);

            sp.spawnEnemyData.timeBetweenSpawn =
                EditorGUILayout.FloatField("Time between spawns", sp.spawnEnemyData.timeBetweenSpawn);
            sp.spawnEnemyData.timeToStartSpawning =
                EditorGUILayout.FloatField("Time to start Spawning", sp.spawnEnemyData.timeToStartSpawning);

            List<string> enemyNames = new List<string>();
            enemyNames.Add("Not Defined");
            bool x = false;
            foreach (var enemy in sp.enemyInfo)
            {
                enemyNames.Add(enemy.name); 
                
                if (enemy.name == sp.spawnEnemyData.enemyType)
                {
                    selected = enemyNames.IndexOf(enemy.name);
                    x = true;
                }
            }
            if (!x)
                selected = 0;

            selected = EditorGUILayout.Popup("EnemyType", selected, enemyNames.ToArray());

            if (sp.enemyInfo.Count > 0)
            {
                if (selected > 0)
                {
                    sp.spawnEnemyData.enemyType = enemyNames[selected];
                }
                else
                {
                    sp.spawnEnemyData.enemyType = unnamed;
                }
            }
        }

        private void DrawToolSettings()
        {
            GUIStyle title = new GUIStyle(GUI.skin.label);
            title.fontSize = 13;
            title.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField(new GUIContent("SpawnerTool Project Settings", "Here is a tooltip"), title,
                GUILayout.Height(20));
            EditorGUILayout.Space();

            var enemyInfo = serializedObject.FindProperty("enemyInfo");
            EditorGUILayout.PropertyField(enemyInfo, new GUIContent("Enemy list"), true);
            serializedObject.Update();

            CheckEnemyNames();
        }

        void CheckEnemyNames()
        {
            bool wrong = false;
            foreach (EnemyInfo enemyInfo in sp.enemyInfo)
            {
                if (enemyInfo.name == string.Empty)
                    wrong = true;
            }

            if (wrong)
            {
                EditorGUILayout.HelpBox("There's one empty name. Please delete or change it. Things might not work.",
                    MessageType.Error);
                EditorGUILayout.Space(10);
            }

            wrong = sp.enemyInfo.GroupBy(n => n).Any(g => g.Count() > 1);
            if (wrong)
            {
                EditorGUILayout.HelpBox("There are duplicated names.", MessageType.Error);
                EditorGUILayout.Space(10);
            }
        }

        private void ValidateValue(ref float value, float minValue)
        {
            value = Mathf.Max(value, minValue);
        }

        private void ValidateValue(ref int value, int minValue)
        {
            value = Mathf.Max(value, minValue);
        }
    }
}