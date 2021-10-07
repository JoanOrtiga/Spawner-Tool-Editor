using UnityEngine;

namespace SpawnerTool
{
    public class BinField
    {
        private SpawnerToolEditor _spawnerToolEditor;

        private Rect _rectBin;

        public BinField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Input(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if(_rectBin.Contains(e.mousePosition))
                    _spawnerToolEditor.SpawnerBlockController.RemoveSelectedBlock();
            }
            if (e.type == EventType.MouseUp)
            {
                if(_rectBin.Contains(e.mousePosition))
                    _spawnerToolEditor.SpawnerBlockController.RemoveSelectedBlock();
            }
        }
        
        public void Draw()
        {
            
            if (_spawnerToolEditor.WindowSize.y > Playground.MarginToPlaygroundSize.y + _spawnerToolEditor.PlayGround.Height)
            {
                _rectBin = new Rect(-1, Playground.MarginToPlaygroundSize.y + _spawnerToolEditor.PlayGround.Height - 60, 60, 60);
                GUI.Label(_rectBin, _spawnerToolEditor.EditorSettings.bin);
            }
            else
            {
                _rectBin = new Rect(-1, _spawnerToolEditor.WindowSize.y - 60  - SpawnerToolEditor._scrollBarPixelSize, 60, 60);
                GUI.Label(_rectBin, _spawnerToolEditor.EditorSettings.bin);
            }
        }
    }
}