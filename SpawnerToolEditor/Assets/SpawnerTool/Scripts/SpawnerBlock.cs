using System;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    public class SpawnerBlock
    {
        public static Texture2D Texture { get; set; }
        
        public SpawnEnemyData spawnEnemyData;
        
        private Rect _rect;
        private Vector2 _size;
        private Color _color;
        private Color _highlightColor;
        
        private bool _selected;
        
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
            _selected = select;
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
            Color blockColor = _selected ? _highlightColor : _color;
            Color infoBlockColor = Color.Lerp(blockColor, Color.black, 0.5f);
            
            var l = 0.2126f * blockColor.r + 0.7152f * blockColor.g + 0.0722f * blockColor.b;
            Color enemyTitleColor = l < 0.5 ? Color.white : Color.black;

            l = 0.2126f * infoBlockColor.r + 0.7152f * infoBlockColor.g + 0.0722f * infoBlockColor.b;
            Color enemyInfoColor = l < 0.5 ? Color.white : Color.black;
            
            GUIStyle enemyTitle = new GUIStyle("label")
            {
                normal = {textColor = enemyTitleColor}, fontSize = 20, alignment = TextAnchor.MiddleLeft
            };
            GUIStyle enemyInfo = new GUIStyle(enemyTitle)
            {
                fontSize = 12, normal = {textColor = enemyInfoColor}
            };

            Rect infoRect = _rect;
            infoRect.x += 4;
            infoRect.y += 25;
            infoRect.width -= 8;
            infoRect.height -= 27;
            
            Rect titleRect = _rect;
            titleRect.y += 30;
            titleRect.height -= 30;
            titleRect.y -= 30;

            GUI.DrawTexture(_rect, Texture, ScaleMode.StretchToFill, true, 0.0f, blockColor, Vector4.zero, new Vector4(5f,5f,13f,13f));
            GUI.DrawTexture(infoRect, Texture, ScaleMode.StretchToFill, true, 0.0f, infoBlockColor, Vector4.zero, new Vector4(5f,5f,15f,15f));
            
            GUILayout.BeginArea(titleRect);
            {
                GUILayout.Label(spawnEnemyData.enemyType, enemyTitle);
            }
            GUILayout.EndArea();
            
            infoRect.x += 3;
            infoRect.y += 5;
            infoRect.width -= 3;
            infoRect.height -= 5;

            if (spawnEnemyData.enemyType != SpawnerToolEditorInspector.Unnamed)
            {
                GUILayout.BeginArea(infoRect);
                {
                    GUILayout.Label("Quantity: " + spawnEnemyData.howManyEnemies, enemyInfo);
                    GUILayout.Label("Frequency: " + spawnEnemyData.timeBetweenSpawn + "s", enemyInfo);
                    GUILayout.Label("SpawnID: " + spawnEnemyData.spawnPointID, enemyInfo);
                }
                GUILayout.EndArea();
            }
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

        public void ProcessInput()
        {
            
        }
    }
}