using System.Linq;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class RelicLocalizer : AssetPostprocessor
    {
        private const string RelicsManagerPath = "Assets/Data/Collections/RelicsData.asset";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var centralData = LoadCentralDataManager();
            var collectionChanged = false;

            foreach (var path in importedAssets)
            {
                var relicInfo = AssetDatabase.LoadAssetAtPath<RelicInfo>(path);

                if (!relicInfo || string.IsNullOrEmpty(relicInfo.key)) continue;

                if (centralData && centralData.relicsData.All(p => p.key != relicInfo.key))
                {
                    centralData.relicsData.Add(relicInfo);
                    Debug.Log($"CentralDataManager: Added new RelicInfo '{relicInfo.key}' to the master list.");
                    collectionChanged = true;
                }

                UpdateLocalizationTables(relicInfo);
            }

            if (!collectionChanged) return;

            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();
        }

        private static RelicsData LoadCentralDataManager()
        {
            var centralData = AssetDatabase.LoadAssetAtPath<RelicsData>(RelicsManagerPath);
            if (!centralData) Debug.LogError($"Central Data Manager asset not found at: {RelicsManagerPath}.");
            return centralData;
        }

        private static void UpdateLocalizationTables(RelicInfo relicInfo)
        {
            var key = relicInfo.key;

            AddKeyToTableCollection("relic_name", key);
            AddKeyToTableCollection("relic_description", key + "_description");
        }

        private static void AddKeyToTableCollection(string collectionName, string key)
        {
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection(collectionName);

            if (!tableCollection)
            {
                Debug.LogWarning($"Localization: Could not find String Table Collection named '{collectionName}'.");
                return;
            }

            var sharedData = tableCollection.SharedData;

            if (sharedData.GetEntry(key) != null) return;
            sharedData.AddKey(key);

            EditorUtility.SetDirty(sharedData);

            Debug.Log($"Localization: Added new key '{key}' to table '{collectionName}'.");
        }
    }
}