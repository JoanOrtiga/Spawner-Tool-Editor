using UnityEngine;
using UnityEditor;

namespace SpawnerTool.EditorScripts
{
    public class DocumentationButton
    {
        public static string GeneralDocumentationLink = "https://joanorba.gitbook.io/spawnertool-editor/";
    
        [MenuItem("Tools/SpawnerTool/Documentation", priority = 101)]
        public static void OpenDocumentation()
        {
            Application.OpenURL(GeneralDocumentationLink);
        }
    }
}

