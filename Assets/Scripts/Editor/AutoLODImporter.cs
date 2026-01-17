using UnityEditor;
using UnityEngine;

public class AutoLODImporter : EditorWindow
{
    [MenuItem("Tools/Enable LOD for All FBX")]
    static void EnableLODs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");
        int count = 0;
        Debug.Log(guids.Length);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null && !importer.generateMeshLods)
            {
                importer.generateMeshLods = true;
                importer.SaveAndReimport();
                count++;
            }
        }

        Debug.Log($"✅ Enabled LOD generation for {count} .fbx files.");
    }
}
