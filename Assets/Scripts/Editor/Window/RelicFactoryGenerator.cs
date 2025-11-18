using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor.Window
{
    public class RelicFactoryGenerator
    {
        private const string FactoryFilePath = "Assets/Scripts/Game/Relics/Commons/RelicFactory.cs";
        private const string FactoryClassName = "RelicFactory";
        private const string BaseLogicClass = "RelicLogic";
        private const string TargetNamespace = "Game.Relics.Commons";

        [MenuItem("Tools/RelicLogic Map Generation")]
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

            sb.AppendLine("        public static " + BaseLogicClass + " CreateLogicInstance(string key, RelicConfig cfg)");
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
            
            sb.AppendLine($"                _ => null");
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
    }
}