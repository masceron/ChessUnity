using System.Collections.Generic;
using System.Linq;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Window
{
    public class PieceManagerWindow : EditorWindow
    {
        private readonly List<PieceInfo> allPieces = new();
        private readonly string[] tableNamesToValidate = { "piece_name", "piece_skill", "piece_skill_description" };
        private readonly string[] toolbarStrings = { "Manage Pieces", "Validate Localization" };
        private bool hasScannedForOrphans;
        private Vector2 manageScrollPos;
        private readonly List<OrphanedKey> orphanedKeys = new();
        private int toolbarIndex;
        private Vector2 validateScrollPos;

        private void OnEnable()
        {
            FindAllPieceInfos();
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
        
        [MenuItem("Tools/Piece Manager")]
        public static void ShowWindow()
        {
            GetWindow<PieceManagerWindow>("Piece Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("PieceInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all PieceInfo assets in your project.", MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllPieceInfos();
            if (GUILayout.Button("Reimport List")) SyncWithCentralData();

            EditorGUILayout.Space();
            
            manageScrollPos = EditorGUILayout.BeginScrollView(manageScrollPos);
            foreach (var piece in allPieces)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                EditorGUILayout.ObjectField(piece, typeof(PieceInfo), false);
                
                if (GUILayout.Button("Find", GUILayout.Width(50))) EditorGUIUtility.PingObject(piece);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        
        private void FindAllPieceInfos()
        {
            allPieces.Clear();
            var guids = AssetDatabase.FindAssets("t:PieceInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var piece = AssetDatabase.LoadAssetAtPath<PieceInfo>(path);
                if (piece) allPieces.Add(piece);
            }
        }
        
        private void SyncWithCentralData()
        {
            var dataGuids = AssetDatabase.FindAssets("t:PiecesData");
            if (dataGuids.Length == 0)
            {
                Debug.LogError("No central data object for PieceInfo found.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(dataGuids[0]);
            var centralData = AssetDatabase.LoadAssetAtPath<PiecesData>(path);

            if (!centralData) return;
            
            centralData.piecesData ??= new List<PieceInfo>();
            
            centralData.piecesData.Clear();
            centralData.piecesData.AddRange(allPieces);
            
            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"PieceManager: Rebuilt PiecesData list. Total items: {centralData.piecesData.Count}");
        }
        
        private void DrawValidateTab()
        {
            EditorGUILayout.LabelField("Localization Key Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool finds localization keys in your tables that do not match any existing PieceInfo asset. This helps you clean up keys from deleted or renamed pieces.",
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
            FindAllPieceInfos();

            foreach (var piece in allPieces.Where(piece => !string.IsNullOrEmpty(piece.key)))
            {
                validKeys.Add(piece.key);
                if (!piece.hasSkill) continue;
                validKeys.Add(piece.key + "_skill");
                validKeys.Add(piece.key + "_skill_description");
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