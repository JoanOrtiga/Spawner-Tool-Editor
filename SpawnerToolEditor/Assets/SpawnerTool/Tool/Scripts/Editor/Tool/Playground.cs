using System;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class Playground : ToolEditorComponent
    {/*
        public SpawnerToolEditorTest2 SpawnerToolEditorTest2 { get; set; }

        private Vector2 _scrollPosition;

        public float Rows { get; set; } = 5f;
        public float Columns { get; set; } = 5f;
        public float Width { get; set; } = 500;
        public float Height { get; set; } = 500;
        
        public Playground(SpawnerToolEditorTest2 spawnerToolEditorTest2)
        {
            SpawnerToolEditorTest2 = spawnerToolEditorTest2;
        }

        public void Draw()
        {
            SpawnerToolEditorTest2 sp = SpawnerToolEditorTest2;
            
            Rect rectScrollView = new Rect(SpawnerToolEditorTest2.MarginToPlaygroundSize.x, SpawnerToolEditorTest2.MarginToPlaygroundSize.y,
                sp.WindowSize.x - SpawnerToolEditorTest2.MarginToPlaygroundSize.x,
                sp.WindowSize.y - SpawnerToolEditorTest2.MarginToPlaygroundSize.y);

            Vector2 _scrollSave = _scrollPosition;

            _scrollPosition = GUI.BeginScrollView(rectScrollView, _scrollPosition, 
                new Rect(0, 0, Width, Height));
            {
                Rect rectPlaygroundFill = new Rect(0, 0, Width, Height);

                GUI.DrawTextureWithTexCoords(rectPlaygroundFill, SpawnerToolEditorTest2.EditorSettings.backgroundTexture,
                    new Rect(1f, SpawnerToolEditorTest2.CellXPercentatge - Rows, Columns * SpawnerToolEditorTest2.CellXPercentatge, 
                        Rows * SpawnerToolEditorTest2.CellXPercentatge));
                
                for (int i = 0; i < sp.Blocks.Count; i++)
                {
                    sp.Blocks[i].Draw();

                    if (sp.SelectedSpawnerBlock != sp.Blocks[i])
                    {
                        if (!rectPlaygroundFill.Contains(sp.Blocks[i].GetRect().position))
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
             }*
        }


        public void HasLostReference(SpawnerToolEditor spawnerToolEditorTest2)
        {
            if (SpawnerToolEditorTest2 == null)
            {
                SpawnerToolEditorTest2 = spawnerToolEditorTest2;
            }
        }*/
    }
}