using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Editor.Windows
{
    public class RelicAssetsManager : EditorWindow
    {
        private const string FactoryFilePath = "Assets/Scripts/Game/Relics/Commons/RelicFactory.cs";
        private const string FactoryClassName = "RelicFactory";
        private const string BaseLogicClass = "RelicLogic";
        private const string TargetNamespace = "Game.Relics.Commons";
        private readonly List<RelicInfo> _allRelics = new();
        private readonly List<OrphanedKey> _orphanedKeys = new();
        private readonly string[] _tableNamesToValidate = { "relic_name", "relic_description" };
        private readonly string[] _toolbarStrings = { "Manage Relics", "Validate Localization" };
        private bool _hasScannedForOrphans;
        private Vector2 _manageScrollPos;
        private int _toolbarIndex;
        private Vector2 _validateScrollPos;

        private void OnEnable()
        {
            FindAllRelicInfos();
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

        [MenuItem("Tools/Relic Manager")]
        public static void ShowWindow()
        {
            GetWindow<RelicAssetsManager>("Relic Manager");
        }

        private void DrawManageTab()
        {
            EditorGUILayout.LabelField("RelicInfo Asset Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This list shows all RelicInfo assets in your project.", MessageType.Info);

            if (GUILayout.Button("Refresh List")) FindAllRelicInfos();
            if (GUILayout.Button("Reimport List")) SyncWithCentralData();
            if (GUILayout.Button("Generate Factory Code")) GenerateFactoryCode();

            EditorGUILayout.Space();

            _manageScrollPos = EditorGUILayout.BeginScrollView(_manageScrollPos);
            foreach (var relic in _allRelics)
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
            _allRelics.Clear();
            var guids = AssetDatabase.FindAssets("t:RelicInfo");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var relic = AssetDatabase.LoadAssetAtPath<RelicInfo>(path);
                if (relic) _allRelics.Add(relic);
            }
        }

        private void SyncWithCentralData()
        {
            var dataGuids = AssetDatabase.FindAssets("t:RelicsData");
            if (dataGuids.Length == 0)
            {
                Debug.LogError("No central data object for RelicInfo found.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(dataGuids[0]);
            var centralData = AssetDatabase.LoadAssetAtPath<RelicsData>(path);

            if (!centralData) return;

            centralData.relicsData ??= new List<RelicInfo>();

            centralData.relicsData.Clear();
            centralData.relicsData.AddRange(_allRelics);

            EditorUtility.SetDirty(centralData);
            AssetDatabase.SaveAssets();

            Debug.Log($"PieceManager: Rebuilt PiecesData list. Total items: {centralData.relicsData.Count}");
        }

        private void DrawValidateTab()
        {
            EditorGUILayout.LabelField("Localization Key Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This tool finds localization keys in your tables that do not match any existing RelicInfo asset. This helps you clean up keys from deleted or renamed relics.",
                MessageType.Info);

            if (GUILayout.Button("Find Orphaned Keys", GUILayout.Height(30))) FindOrphanedKeys();
            if (GUILayout.Button("Delete all orphaned keys", GUILayout.Height(30)))
                if (EditorUtility.DisplayDialog(
                        "Remove All Orphaned Keys?",
                        "Are you sure you want to permanently remove all orphaned keys from the table?",
                        "Remove",
                        "Cancel"))
                {
                    DeleteAllOrphanedKeys();
                    FindOrphanedKeys();
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
            foreach (var orphanedKey in _orphanedKeys) RemoveKeyFromTable(orphanedKey.Key, orphanedKey.TableName);
        }

        private void FindOrphanedKeys()
        {
            _hasScannedForOrphans = true;
            _orphanedKeys.Clear();

            var validKeys = new HashSet<string>();
            FindAllRelicInfos();

            foreach (var relic in _allRelics.Where(relic => !string.IsNullOrEmpty(relic.key)))
            {
                validKeys.Add(relic.key);
                validKeys.Add(relic.key + "_description");
            }

            foreach (var tableName in _tableNamesToValidate)
            {
                var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (!tableCollection) continue;

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

        public static void GenerateFactoryCode()
        {
            var guids = AssetDatabase.FindAssets("t:RelicInfo");
            var relics = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<RelicInfo>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(p => p)
                .ToList();

            var fileContent = BuildFileContent(relics);

            File.WriteAllText(FactoryFilePath, fileContent);
            AssetDatabase.Refresh();
        }

        private static string BuildMethod(List<RelicInfo> relics)
        {
            var sb = new StringBuilder();

            sb.AppendLine("        public static " + BaseLogicClass +
                          " CreateLogicInstance(string key, RelicConfig cfg)");
            sb.AppendLine("        {");
            sb.AppendLine("            return key switch");
            sb.AppendLine("            {");

            foreach (var relic in relics.OrderBy(p => p.key))
            {
                if (string.IsNullOrEmpty(relic.key) || string.IsNullOrEmpty(relic.logicClassName))
                {
                    Debug.LogWarning($"Skipping relic: {relic.name} due to missing key or LogicClassName.");
                    continue;
                }

                sb.AppendLine($"                \"{relic.key}\" => new {relic.logicClassName}(cfg),");
            }

            sb.AppendLine("                _ => null");
            sb.AppendLine("            };");
            sb.AppendLine("        }");

            return sb.ToString();
        }

        private static string BuildFileContent(List<RelicInfo> relics)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"namespace {TargetNamespace}");
            sb.AppendLine("{");

            sb.AppendLine($"    public static class {FactoryClassName}");
            sb.AppendLine("    {");

            sb.Append(BuildMethod(relics));

            sb.AppendLine("    }");

            sb.AppendLine("}");

            return sb.ToString();
        }

        private struct OrphanedKey
        {
            public string Key;
            public string TableName;
        }
    }
}