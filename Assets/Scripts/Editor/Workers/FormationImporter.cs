using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Workers
{
    public class FormationImporter : AssetPostprocessor
    {
        private const string FormationManagerPath = "Assets/Data/Collections/FormationData.asset";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var centralData = LoadCentralDataManager();
            var collectionChanged = false;

            foreach (var path in importedAssets.Concat(movedAssets))
            {
                var formationInfo = AssetDatabase.LoadAssetAtPath<FormationInfo>(path);

                if (!formationInfo) continue;
                var fileName = Path.GetFileNameWithoutExtension(path);
                var newKey = "formation_" + ToSnakeCase(fileName);

                if (string.IsNullOrEmpty(formationInfo.key))
                {
                    formationInfo.key = newKey;
                    EditorUtility.SetDirty(formationInfo);
                    Debug.Log($"Key for {formationInfo.key} auto-generated.");
                }
                else if (!formationInfo.key.StartsWith("formation_") ||
                         !Regex.IsMatch(formationInfo.key, "^[a-z]+(_[a-z]+)*$"))
                {
                    Debug.LogWarning(
                        $"{fileName}'s key '{formationInfo.key}' doesn't follow naming convention for Formation objects. Suggestion: {newKey}");
                }

                if (formationInfo.type != 0 && centralData &&
                    centralData.formationsData.Values.All(p => p.key != formationInfo.key))
                {
                    centralData.formationsData.Add(formationInfo.type, formationInfo);
                    Debug.Log(
                        $"CentralDataManager: Added new FormationInfo '{formationInfo.key}' to the master list.");
                    collectionChanged = true;
                }

                UpdateLocalizationTables(formationInfo);
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

        private static FormationsData LoadCentralDataManager()
        {
            var centralData = AssetDatabase.LoadAssetAtPath<FormationsData>(FormationManagerPath);
            if (!centralData) Debug.LogError($"Central Data Manager asset not found at: {FormationManagerPath}.");

            return centralData;
        }

        private static void UpdateLocalizationTables(FormationInfo formationInfo)
        {
            var key = formationInfo.key;

            AddKeyToTableCollection("formation_name", key);
            AddKeyToTableCollection("formation_description", key + "_description");
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