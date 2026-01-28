using System.Linq;
using System.Text.RegularExpressions;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Worker
{
    public class PieceImporter : AssetPostprocessor
    {
        private const string PiecesManagerPath = "Assets/Data/Collections/PiecesData.asset";
        
        private static PiecesData LoadCentralDataManager()
        {
            var centralData = AssetDatabase.LoadAssetAtPath<PiecesData>(PiecesManagerPath);
            if (!centralData)
            {
                Debug.LogError($"Central Data Manager asset not found at: {PiecesManagerPath}.");
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
                var pieceInfo = AssetDatabase.LoadAssetAtPath<PieceInfo>(path);

                if (!pieceInfo) continue;

                if (string.IsNullOrEmpty(pieceInfo.key))
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                    var newKey = "piece_" + ToSnakeCase(fileName);
                    
                    pieceInfo.key = newKey;
                    EditorUtility.SetDirty(pieceInfo);
                    Debug.Log($"Key for {pieceInfo.key} auto-generated.");
                }

                if (centralData && centralData.piecesData.All(p => p.key != pieceInfo.key))
                {
                    centralData.piecesData.Add(pieceInfo);
                    Debug.Log($"CentralDataManager: Added new PieceInfo '{pieceInfo.key}' to the master list.");
                    collectionChanged = true;
                }

                UpdateLocalizationTables(pieceInfo);
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

        private static void UpdateLocalizationTables(PieceInfo pieceInfo)
        {
            var key = pieceInfo.key;

            AddKeyToTableCollection("piece_name", key);

            if (!pieceInfo.hasSkill) return;

            AddKeyToTableCollection("piece_skill", key + "_skill");
            AddKeyToTableCollection("piece_skill_description", key + "_skill_description");
        }

        /// <summary>
        ///     Helper method to find a String Table Collection and add a key if it doesn't exist.
        /// </summary>
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