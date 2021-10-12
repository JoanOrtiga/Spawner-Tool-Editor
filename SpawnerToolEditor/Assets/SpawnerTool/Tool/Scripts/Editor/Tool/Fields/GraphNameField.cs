using UnityEngine;

namespace SpawnerTool
{
    public class GraphNameField
    {
        private SpawnerToolEditor _spawnerToolEditor;

        private Rect _name = new Rect(457, 0, 100, 60);

        private GUIStyle _graphNameStyle = new GUIStyle("box")
        {
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(5,5,5,5),
            normal =
            {
                textColor = Color.white
            },
            hover =
            {
                textColor = Color.white
            }
            
        };
        
        public GraphNameField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Draw()
        {
            GUI.Box(_name, _spawnerToolEditor.GraphController.GetGraphName(), _graphNameStyle);
        }
    }
}