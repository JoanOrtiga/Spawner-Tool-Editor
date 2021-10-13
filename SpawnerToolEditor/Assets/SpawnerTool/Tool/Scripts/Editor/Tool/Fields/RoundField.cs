using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
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
        
        private string _currentRoundString = "0";

        private string _savedCurrentRoundString;
        private readonly string _controlName = "RoundField";

        public RoundField(SpawnerToolEditor spawnerToolEditor, int round)
        {
            _spawnerToolEditor = spawnerToolEditor;
            _currentRoundString = round.ToString();
            _lastRound = round;
        }

        public void Input(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                bool inInputField = _roundBackground.Contains(e.mousePosition);

                if (!inInputField)
                {
                    GUI.FocusControl("");
                    _spawnerToolEditor.Repaint();
                }
            }
            
            if (e.type == EventType.MouseDown)
            {
                if (!_roundBackground.Contains(e.mousePosition))
                {
                    /*
                    if (round != String.Empty)
                    {
                        if (int.Parse(_round) < 0)
                        {
                            _round = "0";
                        }
                    }
                    else
                    {
                        _round = "0";
                    }*/
                }
            }
            
            if (e.type == EventType.ScrollWheel)
            {
                if (_roundBackground.Contains(e.mousePosition))
                {
                    _currentRoundString = _savedCurrentRoundString;
                    
                    if (e.delta.y > 0)
                    {
                        string round = _currentRoundString;
                        if (round == string.Empty)
                        {
                            round = "0";
                        }
                        
                        round = (int.Parse(round) - 1).ToString();
                        if (int.Parse(round) < 0)
                        {
                            round = "0";
                        }

                        _currentRoundString = round;
                        _spawnerToolEditor.CurrentRound = int.Parse(round);
                    }
                    else
                    {
                        string round = _currentRoundString;
                        if (round == string.Empty)
                        {
                            round = "0";
                        }

                        string lastRound = round;

                        round = (int.Parse(round) + 1).ToString();
                        if (int.Parse(round) > _spawnerToolEditor.EditorSettings.maxRounds)
                        {
                            round = _spawnerToolEditor.EditorSettings.maxRounds.ToString();
                        }
                        
                        _currentRoundString = round;
                        _spawnerToolEditor.CurrentRound = int.Parse(round);
                    }
                    
                    _spawnerToolEditor.Repaint();
                }
            }
        }
        
        public void Draw()
        {
            GUI.Box(_roundBackground, _spawnerToolEditor.EditorSettings.whiteTexture);
            GUI.Label(_roundTitle, "Round", _titlesStyle);

            string roundText = _currentRoundString;
            GUI.SetNextControlName(_controlName);
            roundText = GUI.TextField(_roundTextField, roundText, _spawnerToolEditor.EditorSettings.maxCharactersRounds, _roundFieldStyle);
            roundText = Regex.Replace(roundText, @"[^0-9]", "");

            int round;
            if (int.TryParse(roundText, out round))
            {
                if (round > _spawnerToolEditor.EditorSettings.maxRounds)
                {
                    round = _spawnerToolEditor.EditorSettings.maxRounds;
                    //_spawnerToolEditor.CurrentRound = _spawnerToolEditor.EditorSettings.maxRounds;
                }
                
                if (round != _lastRound)
                {
                    _spawnerToolEditor.GraphController.SaveRound(_lastRound);
                    _spawnerToolEditor.GraphController.LoadRound(round);
                    _lastRound = round;
                }

                _spawnerToolEditor.CurrentRound = round;
                _currentRoundString = round.ToString();
                _savedCurrentRoundString = round.ToString();
            }
            else
            {
                if (GUI.GetNameOfFocusedControl() == _controlName)
                {
                    _currentRoundString = roundText;
                }
                else
                {
                    _currentRoundString = _savedCurrentRoundString;
                    _spawnerToolEditor.CurrentRound = int.Parse(_savedCurrentRoundString);
                }
            }
        }
        
        
    }
}