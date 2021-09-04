using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "SpawnerTool/Settings/ProjectSettings")]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private List<string> enemyProjects = new List<string>();
    }
}

