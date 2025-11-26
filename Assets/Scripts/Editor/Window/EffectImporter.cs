using System.Collections.Generic;
using System.Linq;
using Game.ScriptableObjects;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Window
{ 
    public class EffectImporter : EditorWindow
    {
        private readonly string[] tableNamesToValidate = { "effect_name", "effect_description" };
        
        private readonly List<EffectInfo> allEffects = new();
        private bool hasScannedForOrphans;
        private Vector2 manageScrollPos;
        private readonly List<OrphanedKey> orphanedKeys = new();
        private int toolbarIndex;
        private readonly string[] toolbarStrings = { "Manage Effects", "Validate Localization" };
        private Vector2 validateScrollPos;

        private void OnEnable()
        {
            FindAllEffectInfos();
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

        
        [MenuItem("Tools/Effect Manager")]
        public static void ShowWindow()
        {
            GetWindow<EffectImporter>("Effect Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("EffectInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all EffectInfo assets in your project.",
                MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllEffectInfos();

            EditorGUILayout.Space();

            // --- List of all effects ---
            manageScrollPos = EditorGUILayout.BeginScrollView(manageScrollPos);
            foreach (var effect in allEffects)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                EditorGUILayout.ObjectField(effect, typeof(EffectInfo), false);
                
                if (GUILayout.Button("Find", GUILayout.Width(50))) EditorGUIUtility.PingObject(effect);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        
        private void FindAllEffectInfos()
        {
            allEffects.Clear();
            var guids = AssetDatabase.FindAssets("t:EffectInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                EffectInfo effect = AssetDatabase.LoadAssetAtPath<EffectInfo>(path);
                if (effect != null) allEffects.Add(effect);
            }
        }

        private void DrawValidateTab()
        {
            EditorGUILayout.LabelField("Localization Key Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool finds localization keys that do not match any existing EffectInfo asset.", MessageType.Info);

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
                        orphanedKeys.RemoveAt(i); // Remove from our list
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
            FindAllEffectInfos();

            foreach (var effect in allEffects.Where(effect => !string.IsNullOrEmpty(effect.key)))
            {
                validKeys.Add(effect.key);
                validKeys.Add(effect.key + "_description");
            }
            
            foreach (var tableName in tableNamesToValidate)
            {
                var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (!tableCollection)
                {
                    Debug.LogWarning($"Could not find String Table Collection: {tableName}");
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
            EditorUtility.SetDirty(sharedData); // Mark the asset as modified
            Debug.Log($"Removed key '{key}' from table '{tableName}'");
        }

        // --- Tab 2: Validate Localization ---
        private struct OrphanedKey
        {
            public string Key;
            public string TableName;
        }
    }
}