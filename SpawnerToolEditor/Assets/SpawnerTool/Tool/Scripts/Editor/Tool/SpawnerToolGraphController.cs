using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class SpawnerToolGraphController
    {
        [SerializeField] private SpawnerToolEditor _spawnerToolEditor;
        [SerializeField] private SpawnerGraph _currentGraph;

        public SpawnerToolGraphController(SpawnerToolEditor spawnerToolEditor, SpawnerGraph spawnerGraph)
        {
            _spawnerToolEditor = spawnerToolEditor;
            _currentGraph = spawnerGraph;
        }

        public SpawnerGraph GetCurrentGraph()
        {
            return _currentGraph;
        }

        public bool IsGraphNull()
        {
            return _currentGraph == null;
        }

        public void ChangeGraph(SpawnerGraph newGraph)
        {
            if (_currentGraph != null)
            {
                SaveRound(_spawnerToolEditor.CurrentRound);
            }
            
            _currentGraph = newGraph;
        }

        
        public void SaveRound(int round)
        {
            if (_currentGraph == null)
            {
                Debug.LogError("SPAWNERTOOL: You are trying to save a round without a SpawnerGraph");
                return;
            }
            
            if (_currentGraph.GetAllRounds() == null)
            {
                var allRounds = _currentGraph.GetAllRounds();
                allRounds = new List<Round>();
            }
            
            while (_currentGraph.GetAllRounds().Count <= round)
            {
                _currentGraph.GetAllRounds().Add(new Round());
            }

            Round savedRound = new Round(new List<SpawnEnemyData>(), _spawnerToolEditor.RoundTotalTime, _spawnerToolEditor.RoundTracks);

            foreach (SpawnerBlock block in _spawnerToolEditor.GetBlocks)
            {
                savedRound.spawningEnemiesData.Add(block.spawnEnemyData);
            }
            
            _currentGraph.GetAllRounds()[round] = savedRound;
        }

        public void LoadRound(int round)
        {
            _spawnerToolEditor.GetBlocks.Clear();
            
            if (_currentGraph == null)
            {
                Debug.LogError("SPAWNERTOOL: You are trying to save a round without a SpawnerGraph");
                return;
            }

            if (_currentGraph.GetAllRounds() == null)
                return;

            if (_currentGraph.GetAllRounds().Count <= round)
                return;

            //Second, if round exists, we load its settings.

            _spawnerToolEditor.RoundTracks = _currentGraph.GetAllRounds()[round].totalTracks;
            _spawnerToolEditor.RoundTotalTime = _currentGraph.GetAllRounds()[round].totalRoundTime;
            
            //Lastly, we create blocks for each enemies data.
            foreach (var enemySpawnData in _currentGraph.GetAllRounds()[round].spawningEnemiesData)
            {
                float cellXPercentatge = SpawnerToolEditor.CellXPercentatge;
                Vector2 cellSize = SpawnerToolEditor.CellSize;
                
                Rect newBlockRect = new Rect
                {
                    x = enemySpawnData.timeToStartSpawning * cellXPercentatge * cellSize.x,
                    y = enemySpawnData.currentTrack * cellSize.y,
                    
                    width = cellSize.x * cellXPercentatge *
                            (enemySpawnData.howManyEnemies * enemySpawnData.timeBetweenSpawn),
                    height = cellSize.x
                };
                
                SpawnerBlock spawnerBlock = new SpawnerBlock(newBlockRect, enemySpawnData);
                _spawnerToolEditor.GetBlocks.Add(spawnerBlock);
            }
        }
        
        public void GraphIsNull()
        {
            GUIStyle createGraphStyle = new GUIStyle("button");
            createGraphStyle.fontSize = 16;
            createGraphStyle.fontStyle = FontStyle.Bold;

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Create SpawnerGraph asset in SpawnerGraphs folder", createGraphStyle))
            {
                if (!AssetDatabase.IsValidFolder("Assets/SpawnerGraphs"))
                {
                    AssetDatabase.CreateFolder("Assets", "SpawnerGraphs");
                }

                SpawnerGraph spawnerGraph = ScriptableObject.CreateInstance<SpawnerGraph>();

                int counter = 0;
                while (AssetDatabase.LoadAssetAtPath($"Assets/SpawnerGraphs/SpawnerGraph{counter}.asset",
                    typeof(SpawnerGraph)) != null)
                {
                    counter++;
                }

                AssetDatabase.CreateAsset(spawnerGraph, $"Assets/SpawnerGraphs/SpawnerGraph{counter}.asset");
                this.ChangeGraph(spawnerGraph);
                EditorGUIUtility.PingObject(spawnerGraph);
            }

            GUIStyle labels = new GUIStyle(GUI.skin.label);
            labels.fontSize = 16;
            labels.alignment = TextAnchor.MiddleLeft;
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("or", labels);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            labels.fontSize = 12;
            labels.fontStyle = FontStyle.Bold;
            GUILayout.Label("Select an existing graph from assets folder. (You can create one with create assets menu)",
                labels);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}