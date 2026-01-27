using System.Linq;
using System.Text.RegularExpressions;
using Game.Augmentation;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class AugmentationImporter: AssetPostprocessor
    {
        private const string AugmentationsManagerPath = "Assets/Data/Collections/AugmentationData.asset";
        
        private static AugmentationData LoadCentralDataManager()
        {
            var centralData = AssetDatabase.LoadAssetAtPath<AugmentationData>(AugmentationsManagerPath);
            if (!centralData)
            {
                Debug.LogError($"Central Data Manager asset not found at: {AugmentationsManagerPath}.");
            }
            return centralData;
        }
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var centralData = LoadCentralDataManager();
            var collectionChanged = false;
            
            foreach (var path in importedAssets.Concat(movedAssets))
            {
                var augmentationInfo = AssetDatabase.LoadAssetAtPath<AugmentationInfo>(path);
                
                if (!augmentationInfo) continue;
                
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                var newKey = "augmentation_" + ToSnakeCase(fileName);
                
                if (augmentationInfo.Key != newKey)
                {
                    augmentationInfo.Key = newKey;
                    EditorUtility.SetDirty(augmentationInfo);
                    Debug.Log($"Key for {augmentationInfo.Key} auto-generated.");
                }
                
                if (augmentationInfo.Name != 0 && centralData && centralData.augmentationsData.Values.All(p => p.Key != augmentationInfo.Key))
                {
                    centralData.augmentationsData.Add(augmentationInfo.Name, augmentationInfo);
                    Debug.Log($"CentralDataManager: Added new AugmentationInfo '{augmentationInfo.Key}' to the master list.");
                    collectionChanged = true;
                }
                
                UpdateLocalizationTables(augmentationInfo);
            }
            
            if (!collectionChanged) return;
            
            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();return;

            string ToSnakeCase(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;
                
                text = text.Replace("'", "");
                text = Regex.Replace(text, @"\s+", "_");
                text = Regex.Replace(text, "([a-z0-9])([A-Z])", "$1_$2");
                
                return text.ToLower();
            }
        }

        private static void UpdateLocalizationTables(AugmentationInfo augmentationInfo)
        {
            var key = augmentationInfo.Key;
            
            AddKeyToTableCollection("augmentation_name", key);
            AddKeyToTableCollection("augmentation_description", key + "_description");
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