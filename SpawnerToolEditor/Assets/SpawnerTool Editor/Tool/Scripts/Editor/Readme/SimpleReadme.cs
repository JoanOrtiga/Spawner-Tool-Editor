using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool.EditorScripts.Readme
{
    [CreateAssetMenu(fileName = "README", menuName = "SpawnerTool/Settings/Internal/Readme/SimpleReadme", order = 0)]
    public class SimpleReadme : ScriptableObject
    {
        public string Title;
        [Multiline, TextArea]public string Description;
        public List<Link> Links;
        public Texture2D Icon;
    }

    [Serializable]
    public struct Link
    {
        public string name;//
        public string link;
    }
}

