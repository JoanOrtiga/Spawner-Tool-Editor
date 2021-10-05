using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class SpawnerToolGraphController : ToolEditorComponent
    {
        public SpawnerToolEditor SpawnerToolEditor { get; set; }
        private SpawnerGraph _currentGraph;

        public SpawnerToolGraphController(SpawnerToolEditor spawnerToolEditor, SpawnerGraph spawnerGraph)
        {
            SpawnerToolEditor = spawnerToolEditor;
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
                SaveRound(SpawnerToolEditor.CurrentRound);
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

            Round savedRound = new Round(new List<SpawnEnemyData>(), SpawnerToolEditor.CurrentTotalRoundTime, SpawnerToolEditor.CurrentRoundTracks);

            foreach (SpawnerBlock block in SpawnerToolEditor.Blocks)
            {
                savedRound.spawningEnemiesData.Add(block.spawnEnemyData);
            }
            
            _currentGraph.GetAllRounds()[round] = savedRound;
        }

        public void LoadRound(int round)
        {
            SpawnerToolEditor.Blocks.Clear();
            
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

            SpawnerToolEditor.CurrentRoundTracks = _currentGraph.GetAllRounds()[round].totalTracks;
            SpawnerToolEditor.CurrentTotalRoundTime = _currentGraph.GetAllRounds()[round].totalRoundTime;
            
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
                SpawnerToolEditor.Blocks.Add(spawnerBlock);
            }
        }
    }
}