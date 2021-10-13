using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class BlockController
    {
        [SerializeField] private SpawnerToolEditor _spawnerToolEditor;
        
        [SerializeField] private List<SpawnerBlock> _blocks;
        public List<SpawnerBlock> Blocks => _blocks;
        [SerializeField] private SpawnerBlock _selectedBlock;
        public SpawnerBlock GetSelectedBlock => _selectedBlock;

        private bool _movingBlock = false;
        private bool _wantsToClone;
        private SpawnerBlock _copyPasteBlock;
        private bool _copyPasting;
        private bool _pressedControl;

        public BlockController(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
            _blocks = new List<SpawnerBlock>();
        }

        public void NewBlock(Vector2 position)
        {
            SpawnerBlock spawnerBlock = new SpawnerBlock(position - new Vector2(50, 50), new Vector2(100, 100));
            Select(spawnerBlock);
            _movingBlock = true;
            _blocks.Add(_selectedBlock);
            _spawnerToolEditor.Repaint();
        }

        public SpawnerBlock DuplicateBlock(SpawnerBlock blockToCopy)
        {
            SpawnerBlock block = new SpawnerBlock(blockToCopy);
            _blocks.Add(block);
            _spawnerToolEditor.Repaint();
            return block;
        }
        
        void MoveBlock(Event e)
        {
            Vector2 movePosition = _selectedBlock.GetRect().position;
            Vector2 mouse = e.mousePosition - Playground.MarginToPlaygroundSize + _spawnerToolEditor.PlayGround.ScrollPosition;
            float y = (Mathf.CeilToInt((mouse.y) / 100.0f) - 1) * 100;
            float x = (Mathf.CeilToInt((mouse.x) / 20.0f) - 1) * 20;

            movePosition.x = _spawnerToolEditor.PlayGround.GridMagnet ? x : e.mousePosition.x - Playground.MarginToPlaygroundSize .x + _spawnerToolEditor.PlayGround.ScrollPosition.x;
            movePosition.y = y;

            _selectedBlock.Update(movePosition, movePosition.x, (Mathf.CeilToInt((mouse.y) / 100.0f) - 1));
            _spawnerToolEditor.Repaint();
        }
        
        void Select(SpawnerBlock spawnerBlock)
        {
            _selectedBlock?.Select(false);
            _selectedBlock = spawnerBlock;
            _selectedBlock.Select(true);
            _spawnerToolEditor.Inspector.UpdateInspector();
        }

        void UnSelect()
        {
            _selectedBlock?.Select(false);
            _selectedBlock = null;
        }

        public bool SomethingSelected()
        {
            return _selectedBlock != null;
        }
        
        void RemoveBlock(SpawnerBlock removeSpawnerBlock)
        {
            _blocks.Remove(removeSpawnerBlock);
            _selectedBlock = null;
            _spawnerToolEditor.Repaint();
        }

        public void RemoveSelectedBlock()
        {
            RemoveBlock(_selectedBlock);
        }

        public void Input(Event e)
        {
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Delete)
                {
                    RemoveSelectedBlock();
                }

                if (e.keyCode == KeyCode.LeftControl)
                {
                    _pressedControl = true;
                }

                if (_pressedControl)
                {
                    if (e.keyCode == KeyCode.C)
                    {
                        if (_selectedBlock != null)
                        {
                            _copyPasting = true;
                            _copyPasteBlock = _selectedBlock;
                        }
                            
                        
                    }
                }

                if (_pressedControl)
                {
                    if (e.keyCode == KeyCode.V)
                    {
                        if (_copyPasting)
                        {
                            SpawnerBlock newBlock = DuplicateBlock(_copyPasteBlock);
                            Select(newBlock);
                            MoveBlock(e);  
                        }
                    }
                }
            }
            
            if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.LeftControl)
                {
                    _pressedControl = false;
                }
            }
            
            if (_movingBlock)
            {
                if (e.type == EventType.MouseDrag)
                {
                    if (_selectedBlock is null)
                        return;

                    if (_movingBlock)
                    {
                        MoveBlock(e);
                    }
                }

                if (e.type == EventType.MouseUp)
                {
                    if (!_spawnerToolEditor.PlayGround.IsPositionInsidePlayground(_spawnerToolEditor.PlayGround.GetMousePositionInsidePlayground(e.mousePosition)))
                    {
                        RemoveSelectedBlock();
                    }
                    _movingBlock = false;
                }

                if (e.type == EventType.MouseLeaveWindow)
                {
                    if(SomethingSelected())
                        RemoveSelectedBlock();
                }
            }
            
            if (e.type == EventType.MouseDown)
            {
                bool selected = false;
                for (int i = 0; i < _blocks.Count; i++)
                {
                    if (_blocks[i].Contains(_spawnerToolEditor.PlayGround.GetMousePositionInsidePlayground(e.mousePosition)))
                    {
                        if (_blocks[i] != _selectedBlock)
                        {
                            Select(_blocks[i]);
                        }

                        selected = true;
                        _movingBlock = true;
                    }
                }
                
                if(selected == false && _movingBlock == false && _spawnerToolEditor.PlayGround.IsPositionInsidePlayground(e.mousePosition))
                    UnSelect();
            }
            
            if(e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.LeftAlt)
                {
                    _wantsToClone = true;
                }
            }
            
            if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.LeftAlt)
                {
                    _wantsToClone = false;
                }
            }

            if (e.type == EventType.MouseDown && _wantsToClone)
            {
                if (_selectedBlock != null)
                {
                    if (_selectedBlock.Contains(
                        _spawnerToolEditor.PlayGround.GetMousePositionInsidePlayground(e.mousePosition)))
                    {
                        Select(DuplicateBlock(_selectedBlock));
                    }
                }
            }
        }
        
        public void Update()
        {
            if (_selectedBlock != null)
            {
                SaveInspectorValues();
                _selectedBlock.UpdateTime(_spawnerToolEditor.Inspector.InspectorWindow.spawnEnemyData.TimeToStartSpawning);
                _selectedBlock.UpdateSize();
            }

            foreach (var block in _blocks)
            {
                block.spawnEnemyData.EnemyType = _spawnerToolEditor.Inspector.InspectorWindow.CheckEnemyName(block.spawnEnemyData.EnemyType);
                block.SetColor(ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(block.spawnEnemyData.EnemyType));
            }

            for (int i = _blocks.Count-1; i >= 0; i--)
            {
                if (_blocks[i] == _selectedBlock && _movingBlock)
                    continue;
                /*if (!_spawnerToolEditor.PlayGround.IsPositionInsidePlayground(_blocks[i].GetPosition))
                {
                    RemoveBlock(_blocks[i]);
                }*/
            }
        }
        
        void SaveInspectorValues()
        {
            if (_selectedBlock == null)
                return;

            if (_selectedBlock.spawnEnemyData.EnemyType != _spawnerToolEditor.Inspector.InspectorWindow.spawnEnemyData.EnemyType)
            {
                ChangeColor(_selectedBlock, ProjectConfiguration.Instance.GetProjectSettings().GetEnemyColor(_spawnerToolEditor.Inspector.InspectorWindow.spawnEnemyData.EnemyType));
            }

            _selectedBlock.spawnEnemyData = _spawnerToolEditor.Inspector.InspectorWindow.spawnEnemyData;
        }

        void ChangeColor(SpawnerBlock block, Color color)
        {
            block.SetColor(color);
        }

        public void OnDisable()
        {
            _wantsToClone = false;
            UnSelect();
        }

        public void ExitsWindow()
        {
            _wantsToClone = false;
        }

        public void Draw()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {   
                _blocks[i].Draw();
            }
        }
    }
}