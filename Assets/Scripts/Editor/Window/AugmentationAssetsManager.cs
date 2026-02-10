using System.Collections.Generic;
using System.Linq;
using Game.Augmentation;
using Game.Common;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Window
{
    public class AugmentationAssetsManager: EditorWindow
    {
        private readonly string[] _tableNamesToValidate = { "augmentation_name", "augmentation_description" };
        
        private readonly List<AugmentationInfo> _allAugments = new();
        private bool _hasScannedForOrphans;
        private Vector2 _manageScrollPos;
        private readonly List<OrphanedKey> _orphanedKeys = new();
        private int _toolbarIndex;
        private readonly string[] _toolbarStrings = { "Manage Augmentations", "Validate Localization" };
        private Vector2 _validateScrollPos;

        private void OnEnable()
        {
            FindAllAugmentationInfos();
        }

        private void OnGUI()
        {
            _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbarStrings);

            switch (_toolbarIndex)
            {
                case 0:
                    DrawManageTab();
                    break;
                case 1:
                    DrawValidateTab();
                    break;
            }
        }

        
        [MenuItem("Tools/Augmentation Manager")]
        public static void ShowWindow()
        {
            GetWindow<AugmentationAssetsManager>("Augmentation Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("AugmentationInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all AugmentationInfo assets in your project.",
                MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllAugmentationInfos();
            if (GUILayout.Button("Reimport List")) SyncWithCentralData();

            EditorGUILayout.Space();
            
            _manageScrollPos = EditorGUILayout.BeginScrollView(_manageScrollPos);
            foreach (var augment in _allAugments)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                EditorGUILayout.ObjectField(augment, typeof(AugmentationInfo), false);
                
                if (GUILayout.Button("Find", GUILayout.Width(50))) EditorGUIUtility.PingObject(augment);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        
        private void FindAllAugmentationInfos()
        {
            _allAugments.Clear();
            var guids = AssetDatabase.FindAssets("t:AugmentationInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var augment = AssetDatabase.LoadAssetAtPath<AugmentationInfo>(path);
                if (augment) _allAugments.Add(augment);
            }
            
        }
        
        private void SyncWithCentralData()
        {
            var dataGuids = AssetDatabase.FindAssets("t:AugmentationData");
            if (dataGuids.Length == 0)
            {
                Debug.LogError("No central data object for AugmentationInfo found.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(dataGuids[0]);
            var centralData = AssetDatabase.LoadAssetAtPath<AugmentationData>(path);

            if (!centralData) return;
            centralData.augmentationsData ??= new UDictionary<AugmentationName, AugmentationInfo>();

            centralData.augmentationsData.Clear();
            foreach (var augment in _allAugments)
            {
                centralData.augmentationsData.Add(augment.Name, augment);
            }
            
            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();

            Debug.Log($"AugmentationManager: Rebuilt AugmentationData list. Total items: {centralData.augmentationsData.Count}");
        }

        private void DrawValidateTab()
        {
            EditorGUILayout.LabelField("Localization Key Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool finds localization keys that do not match any existing AugmentationInfo asset.", MessageType.Info);

            if (GUILayout.Button("Find Orphaned Keys", GUILayout.Height(30))) FindOrphanedKeys();
            if (GUILayout.Button("Delete all orphaned keys", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog(
                        "Remove All Orphaned Keys?",
                        "Are you sure you want to permanently remove all orphaned keys from the table?",
                        "Remove",
                        "Cancel"))
                {
                    DeleteAllOrphanedKeys();
                    FindOrphanedKeys();
                }
            }

            if (!_hasScannedForOrphans) return;

            EditorGUILayout.Space();

            if (_orphanedKeys.Count == 0)
            {
                EditorGUILayout.HelpBox("No orphaned keys found. All keys are valid.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField($"Found {_orphanedKeys.Count} orphaned keys:", EditorStyles.boldLabel);
            
            _validateScrollPos = EditorGUILayout.BeginScrollView(_validateScrollPos);
            
            for (var i = _orphanedKeys.Count - 1; i >= 0; i--)
            {
                var orphan = _orphanedKeys[i];
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
                        _orphanedKeys.RemoveAt(i);
                    }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DeleteAllOrphanedKeys()
        {
            foreach (var orphanedKey in _orphanedKeys)
            {
                RemoveKeyFromTable(orphanedKey.Key, orphanedKey.TableName);
            }
        }

        private void FindOrphanedKeys()
        {
            _hasScannedForOrphans = true;
            _orphanedKeys.Clear();
            
            var validKeys = new HashSet<string>();
            FindAllAugmentationInfos();

            foreach (var augment in _allAugments.Where(aug => !string.IsNullOrEmpty(aug.Key)))
            {
                validKeys.Add(augment.Key);
                validKeys.Add(augment.Key + "_description");
            }
            
            foreach (var tableName in _tableNamesToValidate)
            {
                var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (!tableCollection)
                {
                    Debug.LogWarning($"Could not find String Table Collection: {tableName}");
                    continue;
                }

                var sharedData = tableCollection.SharedData;
                foreach (var entry in sharedData.Entries.Where(entry => !validKeys.Contains(entry.Key)))
                    _orphanedKeys.Add(new OrphanedKey { Key = entry.Key, TableName = tableName });
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