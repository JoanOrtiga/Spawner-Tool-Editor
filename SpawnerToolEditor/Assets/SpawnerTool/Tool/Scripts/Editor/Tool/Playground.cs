using System;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class Playground
    {
        public static Vector2 PlaygroundSize { get; } = new Vector2(500, 500);
        public static Vector2 MarginToPlaygroundSize { get; } = new Vector2(57, 58);

        [SerializeField] private SpawnerToolEditor _spawnerToolEditor;
        [SerializeField] private Vector2 _scrollPosition;

        [SerializeField] private float _rows = 5f;
        public float Rows
        {
            get => _rows;
            set => _rows = value;
        }
        [SerializeField] private float _columns = 5f;
        public float Columns
        {
            get => _columns;
            set => _columns = value;
        }

        [SerializeField] private float _width = 500;
        [SerializeField] private float _height = 500;
        
        public Playground(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Update()
        {
            _width = PlaygroundSize.x * SpawnerToolEditor.CellXPercentatge * _columns;
            _height = PlaygroundSize.y * SpawnerToolEditor.CellXPercentatge * _rows;
        }
        
        public void Draw()
        {
            SpawnerToolEditor sp = _spawnerToolEditor;
            
            Rect rectScrollView = new Rect(MarginToPlaygroundSize.x, MarginToPlaygroundSize.y,
                sp.WindowSize.x - MarginToPlaygroundSize.x,
                sp.WindowSize.y - MarginToPlaygroundSize.y);

            Vector2 _scrollSave = _scrollPosition;

            _scrollPosition = GUI.BeginScrollView(rectScrollView, _scrollPosition, 
                new Rect(0, 0, _width, _height));
            {
                Rect rectPlaygroundFill = new Rect(0, 0, _width, _height);

                GUI.DrawTextureWithTexCoords(rectPlaygroundFill, _spawnerToolEditor.EditorSettings.backgroundTexture,
                    new Rect(1f, SpawnerToolEditor.CellXPercentatge - _rows, _columns * SpawnerToolEditor.CellXPercentatge, 
                        _rows * SpawnerToolEditor.CellXPercentatge));
                
                for (int i = 0; i < sp.GetBlocks.Count; i++)
                {
                    sp.GetBlocks[i].Draw();

                    if (sp.GetSelectedBlock != sp.GetBlocks[i])
                    {
                        if (!rectPlaygroundFill.Contains(sp.GetBlocks[i].GetRect().position))
                        {
                            //RemoveBlock(Blocks[i]);
                        }
                    }
                }
            }
            GUI.EndScrollView();
            
            /* if (_scrollingHorizontal)
             {
                 _scrollPosition = _scrollSave;
             }*/
        }
    }
}