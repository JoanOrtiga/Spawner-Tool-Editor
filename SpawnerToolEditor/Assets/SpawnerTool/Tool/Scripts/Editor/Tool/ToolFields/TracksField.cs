using System.Text.RegularExpressions;
using UnityEngine;

namespace SpawnerTool
{
    public class TracksField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _tracksTitle = new Rect(160, 10, 77, 15);
        private Rect _tracksTextField = new Rect(155, 30, 70, 20);

        public TracksField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }
        
        public void Draw()
        {
            GUI.Label(_tracksTitle, "Tracks");

            string _tracks = _spawnerToolEditor.RoundTracks.ToString();
            
            _tracks = GUI.TextField(_tracksTextField, _tracks, _spawnerToolEditor.EditorSettings.maxCharactersTracks);
            _tracks = Regex.Replace(_tracks, @"[^0-9]", "");

            if (_tracks == string.Empty)
                return;

            if (int.Parse(_tracks) >  _spawnerToolEditor.EditorSettings.maxTracks)
            {
                _tracks =  _spawnerToolEditor.EditorSettings.maxTracks.ToString();
            }
            else if (int.Parse(_tracks) <  _spawnerToolEditor.EditorSettings.minTracks)
            {
                _tracks =  _spawnerToolEditor.EditorSettings.minTracks.ToString();
            }

            _spawnerToolEditor.PlayGround.Rows = int.Parse(_tracks);
            _spawnerToolEditor.RoundTracks = int.Parse(_tracks);
        }
    }
}