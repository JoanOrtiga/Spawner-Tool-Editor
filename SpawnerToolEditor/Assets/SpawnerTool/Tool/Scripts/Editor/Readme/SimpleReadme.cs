using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [CreateAssetMenu(fileName = "README", menuName = "SpawnerTool/Settings/Readme/SimpleReadme", order = 3000)]
    public class SimpleReadme : ScriptableObject
    {
        public string Title;
        public string Description;
        public Texture2D Icon;
    }    
}

