using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "README", menuName = "SpawnerTool/Settings/Readme/SpawnerToolConfigurationExample", order = 3000)]
    public class SpawnerToolConfigurationExampleReadme : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _icon;
    }    
}

