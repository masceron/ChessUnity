using System.Collections.Generic;
using System.Linq;
using Game.ScriptableObjects;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Window
{
    public class RelicManagerWindow: EditorWindow
    {
        private readonly List<RelicInfo> allRelics = new();
        private readonly string[] tableNamesToValidate = { "relic_name", "relic_description" };
        private readonly string[] toolbarStrings = { "Manage Relics", "Validate Localization" };
        private bool hasScannedForOrphans;
        private Vector2 manageScrollPos;
        private readonly List<OrphanedKey> orphanedKeys = new();
        private int toolbarIndex;
        private Vector2 validateScrollPos;

        private void OnEnable()
        {
            FindAllRelicInfos();
        }

        private void OnGUI()
        {
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);

            switch (toolbarIndex)
            {
                case 0:
                    DrawManageTab();
                    break;
                case 1:
                    DrawValidateTab();
                    break;
            }
        }
        
        [MenuItem("Tools/Relic Manager")]
        public static void ShowWindow()
        {
            GetWindow<RelicManagerWindow>("Relic Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("RelicInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all RelicInfo assets in your project.", MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllRelicInfos();

            EditorGUILayout.Space();
            
            manageScrollPos = EditorGUILayout.BeginScrollView(manageScrollPos);
            foreach (var relic in allRelics)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                EditorGUILayout.ObjectField(relic, typeof(RelicInfo), false);
                
                if (GUILayout.Button("Find", GUILayout.Width(50))) EditorGUIUtility.PingObject(relic);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        
        private void FindAllRelicInfos()
        {
            allRelics.Clear();
            var guids = AssetDatabase.FindAssets("t:RelicInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var relic = AssetDatabase.LoadAssetAtPath<RelicInfo>(path);
                if (relic) allRelics.Add(relic);
            }
        }
        
        private void DrawValidateTab()
        {
            EditorGUILayout.LabelField("Localization Key Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool finds localization keys in your tables that do not match any existing RelicInfo asset. This helps you clean up keys from deleted or renamed relics.",
                MessageType.Info);

            if (GUILayout.Button("Find Orphaned Keys", GUILayout.Height(30))) FindOrphanedKeys();

            if (!hasScannedForOrphans) return;

            EditorGUILayout.Space();

            if (orphanedKeys.Count == 0)
            {
                EditorGUILayout.HelpBox("No orphaned keys found. All keys are valid.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField($"Found {orphanedKeys.Count} orphaned keys:", EditorStyles.boldLabel);
            
            validateScrollPos = EditorGUILayout.BeginScrollView(validateScrollPos);
            
            for (var i = orphanedKeys.Count - 1; i >= 0; i--)
            {
                var orphan = orphanedKeys[i];
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent(orphan.Key, "Table: " + orphan.TableName));

                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    if (EditorUtility.DisplayDialog(
                            "Remove Key?",
                            $"Are you sure you want to permanently remove the key '{orphan.Key}' from the table '{orphan.TableName}'?",
                            "Remove",
                            "Cancel"))
                    {
                        RemoveKeyFromTable(orphan.Key, orphan.TableName);
                        orphanedKeys.RemoveAt(i);
                    }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void FindOrphanedKeys()
        {
            hasScannedForOrphans = true;
            orphanedKeys.Clear();
            
            var validKeys = new HashSet<string>();
            FindAllRelicInfos();

            foreach (var relic in allRelics.Where(relic => !string.IsNullOrEmpty(relic.key)))
            {
                validKeys.Add(relic.key);
                validKeys.Add(relic.key + "_description");
            }
            
            foreach (var tableName in tableNamesToValidate)
            {
                var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (!tableCollection)
                {
                    continue;
                }

                var sharedData = tableCollection.SharedData;
                foreach (var entry in sharedData.Entries.Where(entry => !validKeys.Contains(entry.Key)))
                    orphanedKeys.Add(new OrphanedKey { Key = entry.Key, TableName = tableName });
            }
        }

        private static void RemoveKeyFromTable(string key, string tableName)
        {
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
            if (!tableCollection) return;

            var sharedData = tableCollection.SharedData;
            if (sharedData.GetEntry(key) == null) return;
            
            sharedData.RemoveKey(key);
            EditorUtility.SetDirty(sharedData);
            Debug.Log($"Removed key '{key}' from table '{tableName}'");
        }
        
        private struct OrphanedKey
        {
            public string Key;
            public string TableName;
        }
    }
}