using UnityEngine;
using UnityEditor;

namespace SpawnerTool.EditorScripts
{
    public class DocumentationButton
    {
        public static string GeneralDocumentationLink = "https://joanorba.gitbook.io/spawnertool-editor/";
    
        [MenuItem("SpawnerTool/Documentation", priority = 11)]
        public static void OpenDocumentation()
        {
            Application.OpenURL(GeneralDocumentationLink);
        }
    }
}

