using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;

namespace Editor.Window
{
    public class LocalizationSync : EditorWindow
    {
        [MenuItem("Tools/Sync localizations/Pull")]
        public static void PullAll()
        {
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                foreach (var extension in collection.Extensions)
                {
                    if (extension is GoogleSheetsExtension googleExtension)
                    {
                        Pull(googleExtension);
                    }
                }
            }
        }

        [MenuItem("Tools/Sync localizations/Push")]
        public static void PushALl()
        {
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                foreach (var extension in collection.Extensions)
                {
                    if (extension is GoogleSheetsExtension googleExtension)
                    {
                        Push(googleExtension);
                    }
                }
            }
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