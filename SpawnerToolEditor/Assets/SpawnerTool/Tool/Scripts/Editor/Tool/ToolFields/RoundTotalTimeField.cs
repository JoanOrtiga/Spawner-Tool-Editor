using System.Text.RegularExpressions;
using UnityEngine;

namespace SpawnerTool
{
    public class RoundTotalTimeField
    {
        private SpawnerToolEditor _spawnerToolEditor;
        
        private Rect _totalTimeTitle = new Rect(70, 10, 77, 15);
        private Rect _totalTimeTextField = new Rect(75, 30, 70, 20);

        public RoundTotalTimeField(SpawnerToolEditor spawnerToolEditor)
        {
            _spawnerToolEditor = spawnerToolEditor;
        }
        
        public void Draw()
        {
            string workingTime = _spawnerToolEditor.RoundTotalTime.ToString();
            string roundTotalTime = workingTime;
            GUI.Label(_totalTimeTitle, "Total time (s)");
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

            if (roundTotalTime == string.Empty)
                return;

            if (float.Parse(roundTotalTime) > _spawnerToolEditor.EditorSettings.maxTotalTime)
            {
                roundTotalTime = _spawnerToolEditor.EditorSettings.maxTotalTime.ToString();
            }

            _spawnerToolEditor.PlayGround.Columns = float.Parse(roundTotalTime);
            _spawnerToolEditor.PlayGround.Columns /= SpawnerToolEditor.SecondsXCell;

            _spawnerToolEditor.RoundTotalTime = float.Parse(roundTotalTime);
        }
    }
}