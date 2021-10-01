using UnityEngine;

namespace SpawnerTool
{
    [CreateAssetMenu(fileName = "README", menuName = "SpawnerTool/Settings/Readme/SimpleReadme", order = 3000)]
    public class SimpleReadme : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _icon;
    }    
}

