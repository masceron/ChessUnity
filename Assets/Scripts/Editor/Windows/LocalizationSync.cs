using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEngine;

namespace Editor.Windows
{
    public class LocalizationSync : EditorWindow
    {
        [MenuItem("Tools/Localizations/Remove empty or duplicate keys")]
        private static void RemoveDuplicates()
        {
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            var totalRemoved = 0;

            foreach (var collection in stringTableCollections)
            {
                var sharedData = collection.SharedData;

                if (!sharedData) continue;

                var seenKeys = new HashSet<string>();
                var idsToRemove = (from entry in sharedData.Entries
                        where string.IsNullOrEmpty(entry.Key) || !seenKeys.Add(entry.Key)
                        select entry.Id)
                    .ToList();

                if (idsToRemove.Count <= 0) continue;
                Undo.RecordObject(sharedData, "Remove Duplicate Localization Keys");

                foreach (var id in idsToRemove)
                {
                    sharedData.RemoveKey(id);
                    totalRemoved++;
                }

                Debug.Log($"Removed {idsToRemove.Count} duplicate keys from collection: {collection.Group}");

                EditorUtility.SetDirty(sharedData);
            }

            if (totalRemoved > 0)
            {
                AssetDatabase.SaveAssets();
                Debug.Log($"<color=green>Success:</color> Removed a total of {totalRemoved}  keys.");
            }
            else
            {
                Debug.Log("No empty or duplicate key found.");
            }
        }

        [MenuItem("Tools/Localizations/Pull")]
        private static void PullAll()
        {
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            foreach (var extension in collection.Extensions)
                if (extension is GoogleSheetsExtension googleExtension)
                    Pull(googleExtension);
        }

        [MenuItem("Tools/Localizations/Push")]
        private static void PushALl()
        {
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            foreach (var extension in collection.Extensions)
                if (extension is GoogleSheetsExtension googleExtension)
                    Push(googleExtension);
        }

        private static void Push(GoogleSheetsExtension googleExtension)
        {
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider)
            {
                SpreadSheetId = googleExtension.SpreadsheetId
            };

            googleSheets.PushStringTableCollectionAsync(googleExtension.SheetId,
                googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns);
        }

        private static void Pull(GoogleSheetsExtension googleExtension)
        {
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider)
            {
                SpreadSheetId = googleExtension.SpreadsheetId
            };

            googleSheets.PullIntoStringTableCollection(googleExtension.SheetId,
                googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns);
        }
    }
}