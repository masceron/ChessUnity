using System.Linq;
using System.Text.RegularExpressions;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class RelicImporter : AssetPostprocessor
    {
        private const string RelicsManagerPath = "Assets/Data/Collections/RelicsData.asset";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var centralData = LoadCentralDataManager();
            var collectionChanged = false;

            foreach (var path in importedAssets.Concat(movedAssets))
            {
                var relicInfo = AssetDatabase.LoadAssetAtPath<RelicInfo>(path);

                if (!relicInfo) continue;
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                var newKey = "relic_" + ToSnakeCase(fileName);

                if (string.IsNullOrEmpty(relicInfo.key))
                {
                    relicInfo.key = newKey;
                    EditorUtility.SetDirty(relicInfo);
                    Debug.Log($"Key for {relicInfo.key} auto-generated.");
                }
                else if (!relicInfo.key.StartsWith("effect_"))
                {
                    Debug.LogWarning(
                        $"{fileName}'s key '${relicInfo.key}' doesn't follow naming convention for Relic objects. Suggestion: ${newKey}");
                }

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
            return;

            string ToSnakeCase(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;

                text = text.Replace("'", "");
                text = Regex.Replace(text, @"\s+", "_");
                text = Regex.Replace(text, "([a-z0-9])([A-Z])", "$1_$2");

                return text.ToLower();
            }
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