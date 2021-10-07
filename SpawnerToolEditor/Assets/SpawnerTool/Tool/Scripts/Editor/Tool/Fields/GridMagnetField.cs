using System;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class GridMagnetField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _gridMagnet = new Rect(235, 30, 100, 18);

        public GridMagnetField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Draw()
        {
            _spawnerToolEditor.PlayGround.GridMagnet = GUI.Toggle(_gridMagnet, _spawnerToolEditor.PlayGround.GridMagnet, "Grid magnet");
        }
    }
}