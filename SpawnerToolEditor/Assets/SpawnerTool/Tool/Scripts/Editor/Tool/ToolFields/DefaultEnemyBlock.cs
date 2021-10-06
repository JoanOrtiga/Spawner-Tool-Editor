using System;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class DefaultEnemyBlock
    {
        private Rect _rectEnemyBlockDefault = new Rect(0, 120, 57, 57);

        private SpawnerToolEditor _spawnerToolEditor;

        public DefaultEnemyBlock(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Draw()
        {
            GUI.DrawTexture(_rectEnemyBlockDefault, _spawnerToolEditor.EditorSettings.whiteTexture);

            GUIStyle label = new GUIStyle("label");
            label.normal.textColor = Color.black;

            GUI.Label(new Rect(_rectEnemyBlockDefault.x + 2, _rectEnemyBlockDefault.y + 5, 50, 50), "Drag me", label);
        }
    }
}