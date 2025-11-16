using System.Linq;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class EffectLocalizer : AssetPostprocessor
    {
        private const string EffectsManagerPath = "Assets/Data/Collections/EffectsData.asset";
        
        private static EffectsData LoadCentralDataManager()
        {
            var centralData = AssetDatabase.LoadAssetAtPath<EffectsData>(EffectsManagerPath);
            if (!centralData)
            {
                Debug.LogError($"Central Data Manager asset not found at: {EffectsManagerPath}.");
            }
            return centralData;
        }
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var centralData = LoadCentralDataManager();
            var collectionChanged = false;
            
            foreach (var path in importedAssets)
            {
                var effectInfo = AssetDatabase.LoadAssetAtPath<EffectInfo>(path);
                
                if (!effectInfo || string.IsNullOrEmpty(effectInfo.key)) continue;
                
                if (centralData && centralData.effectsData.All(p => p.key != effectInfo.key))
                {
                    centralData.effectsData.Add(effectInfo);
                    Debug.Log($"CentralDataManager: Added new EffectInfo '{effectInfo.key}' to the master list.");
                    collectionChanged = true;
                }
                
                UpdateLocalizationTables(effectInfo);
            }
            
            if (!collectionChanged) return;
            
            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();
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