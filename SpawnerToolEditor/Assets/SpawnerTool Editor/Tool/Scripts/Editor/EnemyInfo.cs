using System;
using UnityEngine;

namespace SpawnerTool.EditorScripts
{
    [Serializable]
    public struct EnemyInfo
    {
        public string name;
        public Color blockColor;

        public EnemyInfo(string name, Color color)
        {
            this.name = name;
            this.blockColor = color;
        }
    }
}