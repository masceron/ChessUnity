using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AutoLODImporter : EditorWindow
    {
        [MenuItem("Tools/Enable LOD for All FBX")]
        private static void EnableLoDs()
        {
            var guids = AssetDatabase.FindAssets("t:Model");
            var count = 0;
            Debug.Log(guids.Length);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var importer = AssetImporter.GetAtPath(path) as ModelImporter;
                if (!importer || importer.generateMeshLods) continue;
                
                importer.generateMeshLods = true;
                importer.SaveAndReimport();
                count++;
            }

            Debug.Log($"✅ Enabled LOD generation for {count} .fbx files.");
        }
    }
}
