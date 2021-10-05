using System.Text.RegularExpressions;
using UnityEngine;

namespace SpawnerTool
{
    public class RoundField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _roundTextField = new Rect(2, 2, 52, 41);
        private Rect _roundTitle = new Rect(12, 42, 50, 18);
        private Rect _roundBackground = new Rect(-3, -2, 64, 63);

        private int _lastRound = 0;

        private GUIStyle _titlesStyle = new GUIStyle
        {
            fontSize = 11,
            normal =
            {
                textColor = Color.black
            }
        };

        private GUIStyle _roundFieldStyle = new GUIStyle(GUIStyle.none)
        {
            normal =
            {
                textColor = Color.black
            },
            fontSize = 25,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        
        public RoundField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }
        
        public void Draw()
        {
            GUI.Box(_roundBackground, _spawnerToolEditor.EditorSettings.whiteTexture);
            GUI.Label(_roundTitle, "Round", _titlesStyle);

            string roundText = _spawnerToolEditor.CurrentRound.ToString();
            roundText = GUI.TextField(_roundTextField, roundText, _spawnerToolEditor.EditorSettings.maxCharactersRounds, _roundFieldStyle);
            roundText = Regex.Replace(roundText, @"[^0-9]", "");
            
            int round;
            if (!int.TryParse(roundText, out round))
                return;
            
            if (round > _spawnerToolEditor.EditorSettings.maxRounds)
            {
                round = _spawnerToolEditor.EditorSettings.maxRounds;
                _spawnerToolEditor.CurrentRound = _spawnerToolEditor.EditorSettings.maxRounds;
            }

            if (round != _lastRound)
            {
                _spawnerToolEditor.GraphController.SaveRound(_lastRound);
                _spawnerToolEditor.GraphController.LoadRound(round);
                _lastRound = round;
            }
        }
    }
}