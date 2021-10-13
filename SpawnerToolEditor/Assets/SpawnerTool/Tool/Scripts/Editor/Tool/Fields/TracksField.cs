using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class TracksField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _tracksTitle = new Rect(160, 10, 77, 15);
        private Rect _tracksTextField = new Rect(155, 30, 70, 20);

        private string _tracksString = "5";
        private string _savedTracks;
        private readonly string _controlName = "TracksField";

        public TracksField(SpawnerToolEditor spawnerToolEditor, int roundTracks)
        {
            _spawnerToolEditor = spawnerToolEditor;
            _tracksString = roundTracks.ToString();
            _savedTracks = _tracksString;
        }

        public void Input(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                bool inInputField = _tracksTextField.Contains(e.mousePosition);

                if (!inInputField)
                {
                    GUI.FocusControl("");
                    _spawnerToolEditor.Repaint();
                }
            }
            else if (e.type == EventType.ScrollWheel)
            {
                if (_tracksTextField.Contains(e.mousePosition))
                {
                    _tracksString = _savedTracks;
                    
                    if (e.delta.y > 0)
                    {
                        _tracksString = (int.Parse(_tracksString) - 1).ToString();
                        if (int.Parse(_tracksString) < _spawnerToolEditor.EditorSettings.minTracks)
                        {
                            _tracksString = _spawnerToolEditor.EditorSettings.minTracks.ToString();
                        }
                    }
                    else
                    {
                        _tracksString = (int.Parse(_tracksString) + 1).ToString();
                        if (int.Parse(_tracksString) > _spawnerToolEditor.EditorSettings.maxTracks)
                        {
                            _tracksString = _spawnerToolEditor.EditorSettings.maxTracks.ToString();
                        }
                    }

                    _spawnerToolEditor.RoundTracks = int.Parse(_tracksString);
                    _spawnerToolEditor.Repaint();
                }
            }
        }
        
        public void Draw()
        {
            GUI.Label(_tracksTitle, "Tracks");

            string tracks = _tracksString;
            
            GUI.SetNextControlName(_controlName);
            tracks = GUI.TextField(_tracksTextField, tracks, _spawnerToolEditor.EditorSettings.maxCharactersTracks);
            tracks = Regex.Replace(tracks, @"[^0-9]", "");

            int trackNumber;
            if (int.TryParse(tracks, out trackNumber))
            {
                if (trackNumber >  _spawnerToolEditor.EditorSettings.maxTracks)
                {
                    trackNumber =  _spawnerToolEditor.EditorSettings.maxTracks;
                }
                else if (trackNumber <  _spawnerToolEditor.EditorSettings.minTracks)
                {
                    trackNumber =  _spawnerToolEditor.EditorSettings.minTracks;
                }

                _spawnerToolEditor.PlayGround.Rows = trackNumber;
                
                _spawnerToolEditor.RoundTracks = trackNumber;
                _tracksString = trackNumber.ToString();
                _savedTracks = trackNumber.ToString();
            }
            else
            {
                if (GUI.GetNameOfFocusedControl() == _controlName)
                {
                    _tracksString = tracks;
                }
                else
                {
                    _tracksString = _savedTracks;
                    _spawnerToolEditor.CurrentRound = int.Parse(_savedTracks);
                }
            }
        }

        public void ChangeTracks(int tracks)
        {
            _tracksString = tracks.ToString();
        }
    }
}