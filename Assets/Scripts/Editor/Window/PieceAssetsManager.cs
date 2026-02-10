using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Window
{
    public class PieceAssetsManager : EditorWindow
    {
        private readonly List<PieceInfo> _allPieces = new();
        private readonly string[] _tableNamesToValidate = { "piece_name", "piece_skill", "piece_skill_description" };
        private readonly string[] _toolbarStrings = { "Manage Pieces", "Validate Localization" };
        private bool _hasScannedForOrphans;
        private Vector2 _manageScrollPos;
        private readonly List<OrphanedKey> _orphanedKeys = new();
        private int _toolbarIndex;
        private Vector2 _validateScrollPos;

        private void OnEnable()
        {
            FindAllPieceInfos();
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
        
        [MenuItem("Tools/Piece Manager")]
        public static void ShowWindow()
        {
            GetWindow<PieceAssetsManager>("Piece Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("PieceInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all PieceInfo assets in your project.", MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllPieceInfos();
            if (GUILayout.Button("Reimport List")) SyncWithCentralData();
            if (GUILayout.Button("Generate Factory Code")) GenerateFactoryCode();

            EditorGUILayout.Space();
            
            _manageScrollPos = EditorGUILayout.BeginScrollView(_manageScrollPos);
            foreach (var piece in _allPieces)
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
            _allPieces.Clear();
            var guids = AssetDatabase.FindAssets("t:PieceInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var piece = AssetDatabase.LoadAssetAtPath<PieceInfo>(path);
                if (piece) _allPieces.Add(piece);
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
            centralData.piecesData.AddRange(_allPieces);
            
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
            FindAllPieceInfos();

            foreach (var piece in _allPieces.Where(piece => !string.IsNullOrEmpty(piece.key)))
            {
                validKeys.Add(piece.key);
                if (!piece.hasSkill) continue;
                validKeys.Add(piece.key + "_skill");
                validKeys.Add(piece.key + "_skill_description");
            }
            
            foreach (var tableName in _tableNamesToValidate)
            {
                var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (!tableCollection)
                {
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
        
        private const string FactoryFilePath = "Assets/Scripts/Game/Piece/PieceLogic/Commons/PieceFactory.cs";
        private const string FactoryClassName = "PieceFactory";
        private const string BaseLogicClass = "PieceLogic";
        private const string TargetNamespace = "Game.Piece.PieceLogic.Commons";
        
        public static void GenerateFactoryCode()
        {
            var guids = AssetDatabase.FindAssets("t:PieceInfo");
            var pieces = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<PieceInfo>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(p => p)
                .ToList();

            var fileContent = BuildFileContent(pieces);

            File.WriteAllText(FactoryFilePath, fileContent);
            AssetDatabase.Refresh();
        }

        private static string BuildMethod(List<PieceInfo> pieces)
        {
            var sb = new StringBuilder();

            sb.AppendLine("        public static " + BaseLogicClass + " CreateLogicInstance(string key, PieceConfig cfg)");
            sb.AppendLine("        {");
            sb.AppendLine("            return key switch");
            sb.AppendLine("            {");
            
            foreach (var piece in pieces.OrderBy(p => p.key))
            {
                if (string.IsNullOrEmpty(piece.key) || string.IsNullOrEmpty(piece.logicClassName))
                {
                    Debug.LogWarning($"Skipping piece: {piece.name} due to missing key or LogicClassName.");
                    continue;
                }

                sb.AppendLine($"                \"{piece.key}\" => new {piece.logicClassName}(cfg),");
            }
            
            sb.AppendLine($"                _ => null");
            sb.AppendLine("            };");
            sb.AppendLine("        }");

            return sb.ToString();
        }

        private static string BuildFileContent(List<PieceInfo> pieces)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"namespace {TargetNamespace}");
            sb.AppendLine("{");

            sb.AppendLine($"    public static class {FactoryClassName}");
            sb.AppendLine("    {");

            sb.Append(BuildMethod(pieces));

            sb.AppendLine("    }");

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}