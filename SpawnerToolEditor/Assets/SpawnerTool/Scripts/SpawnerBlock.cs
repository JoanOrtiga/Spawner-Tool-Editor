using System;
using UnityEngine;

namespace SpawnerTool
{
    public class SpawnerBlock
    {
        private Rect _rect;
        private Vector2 _size;
        private Color _color;
        private Color _highlightColor;
        public static Texture2D texture;

        private bool selected;

        public SpawnEnemyData spawnEnemyData;

        public SpawnerBlock(Vector2 position, Vector2 size, SpawnEnemyData sp = null)
        {
            spawnEnemyData = sp is null ? new SpawnEnemyData() : sp;

            _size.x = Mathf.Max(size.x, 20.0f);

            _size = size;
            _rect = new Rect(position.x, position.y, size.x, size.y);
            _color = Color.white;
            _highlightColor = Color.yellow;
        }

        public SpawnerBlock(Rect rect, SpawnEnemyData sp = null)
        {
            spawnEnemyData = sp is null ? new SpawnEnemyData() : sp;

            rect.width = Mathf.Max(rect.width, 20.0f);

            _size = rect.size;
            _rect = rect;
            _color = Color.white;
            _highlightColor = Color.yellow;
        }

        public void Update(Vector2 mouseDrag, float time, int track)
        {
            spawnEnemyData.currentTrack = track;
            spawnEnemyData.timeToStartSpawning =
                time / (SpawnerToolEditor.CellSize * SpawnerToolEditor.CellPixelSize.x);
            _rect = new Rect(mouseDrag.x, mouseDrag.y, _size.x, _size.y);
        }

        public void UpdateTime(float time)
        {
            spawnEnemyData.timeToStartSpawning = time;
            _rect.x = time * (SpawnerToolEditor.CellSize * SpawnerToolEditor.CellPixelSize.x);
        }

        public void ChangeSize(Vector2 size)
        {
            size.x = Mathf.Max(size.x, 20.0f);
            _size = size;
        }

        public void Select(bool select)
        {
            selected = select;
        }

        public bool Contains(Vector2 position)
        {
            if (_rect.Contains(position))
            {
                return true;
            }

            return false;
        }

        public void Draw()
        {
            Color guiColor = GUI.color;
            GUI.color = selected ? _highlightColor : _color;

            GUIStyle label = new GUIStyle("label");
            Color color = GUI.color;
            var l = 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
            color = l < 0.5 ? Color.white : Color.black;
            label.normal.textColor = color;
            label.wordWrap = true;
            label.fontSize = Mathf.RoundToInt(SpawnerToolsUtility.Remap(spawnEnemyData.enemyType.Length, 1, 20, 50, 10));

            GUI.DrawTexture(_rect, texture);
            GUI.color = guiColor;
            GUILayout.BeginArea(_rect);
            {
                GUILayout.Label(spawnEnemyData.enemyType, label);
            }
            GUILayout.EndArea();
        }
        


        public void SetColor(Color color)
        {
            color.a = 1;
            _color = color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public Rect GetRect()
        {
            return _rect;
        }

        public void UpdateSize()
        {
            _size.x = SpawnerToolEditor.CellPixelSize.x * SpawnerToolEditor.CellSize *
                      (spawnEnemyData.howManyEnemies * spawnEnemyData.timeBetweenSpawn);
            _size.x = Mathf.Max(_size.x, 20.0f);

            _size.y = 100f;

            _rect.size = _size;
        }
    }
}