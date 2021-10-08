using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class RoundTotalTimeField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _totalTimeTitle = new Rect(70, 10, 77, 15);
        private Rect _totalTimeTextField = new Rect(75, 30, 70, 20);

        [SerializeField] private string _roundTotalTimeString = "25";
        private string _savedRoundTotalTimeString;

        private readonly string _controlName = "RoundTotalTime";

        public RoundTotalTimeField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }

        public void Input(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                bool inInputField = _totalTimeTextField.Contains(e.mousePosition);

                if (!inInputField)
                {
                    GUI.FocusControl("");
                    _spawnerToolEditor.Repaint();
                }
            }

            if (e.type == EventType.ScrollWheel)
            {
                if (_totalTimeTextField.Contains(e.mousePosition))
                {
                    _roundTotalTimeString = _savedRoundTotalTimeString;
                    
                    string totalTime = _roundTotalTimeString;

                    if (e.delta.y > 0)
                    {
                        totalTime = (float.Parse(totalTime) - 1.0f).ToString();
                        if (float.Parse(totalTime) < 1.0f)
                        {
                            totalTime = "1";
                            //EditorUtility.DisplayDialog("Error", "Time must be greater than 1 second.", "ok", DialogOptOutDecisionType.ForThisSession, "LessThanOne");
                            
                        }
                    }
                    else
                    {
                        totalTime = (float.Parse(totalTime) + 1).ToString();
                        if (float.Parse(totalTime) > _spawnerToolEditor.EditorSettings.maxTotalTime)
                        {
                            totalTime = _spawnerToolEditor.EditorSettings.maxTotalTime.ToString();
                        }
                    }
                    
                    _roundTotalTimeString = totalTime;
                    _spawnerToolEditor.RoundTotalTime = float.Parse(totalTime);
                    
                    _spawnerToolEditor.Repaint();
                }
            }
        }
        
        public void Draw()
        {
            GUI.Label(_totalTimeTitle, "Total time (s)");
            
            string roundTotalTime = _roundTotalTimeString;
            string workingTime = _roundTotalTimeString;

            GUI.SetNextControlName(_controlName);
            roundTotalTime = GUI.TextField(_totalTimeTextField, roundTotalTime, _spawnerToolEditor.EditorSettings.maxCharactersTotalTime);
            roundTotalTime = Regex.Replace(roundTotalTime, @"[^0-9,]", "");

            int totalComas = 0;
            for (int i = 0; i < roundTotalTime.Length; ++i)
            {
                if (roundTotalTime[i] == ',')
                {
                    totalComas++;
                }
            }


            if (totalComas >= 2)
            {
                roundTotalTime = workingTime;
            }
            
            

            float roundTime;
            if (float.TryParse(roundTotalTime, out roundTime) && roundTotalTime[roundTotalTime.Length-1] != ',')
            {
                if (roundTime > _spawnerToolEditor.EditorSettings.maxTotalTime)
                {
                    roundTime = _spawnerToolEditor.EditorSettings.maxTotalTime;
                }
                
                _spawnerToolEditor.PlayGround.Columns = roundTime;
                _spawnerToolEditor.PlayGround.Columns /= SpawnerToolEditor.SecondsXCell;

                _spawnerToolEditor.RoundTotalTime = roundTime;
                _roundTotalTimeString = roundTime.ToString();
                _savedRoundTotalTimeString = roundTime.ToString();

            }
            else
            {
                if (GUI.GetNameOfFocusedControl() == _controlName)
                {
                    _roundTotalTimeString = roundTotalTime;
                }
                else
                {
                    _roundTotalTimeString = _savedRoundTotalTimeString;
                    _spawnerToolEditor.RoundTotalTime = float.Parse(_savedRoundTotalTimeString);
                }
            }
        }

        public void ChangeTotalTime(float time)
        {
            _roundTotalTimeString = time.ToString();
        }
    }
}