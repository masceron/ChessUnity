using Game.ScriptableObjects;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class EffectLocalizer : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                var effectInfo = AssetDatabase.LoadAssetAtPath<EffectInfo>(path);
                
                if (!effectInfo || string.IsNullOrEmpty(effectInfo.key)) continue;
                
                UpdateLocalizationTables(effectInfo);
            }
        }

        private static void UpdateLocalizationTables(EffectInfo effectInfo)
        {
            var key = effectInfo.key;
            
            AddKeyToTableCollection("effect_name", key);
            AddKeyToTableCollection("effect_description", key + "_description");
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