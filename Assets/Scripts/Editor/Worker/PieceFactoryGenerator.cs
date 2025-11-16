using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor.Worker
{
    public static class PieceFactoryGenerator
    {
        private const string FactoryFilePath = "Assets/Scripts/Game/Piece/PieceLogic/Commons/PieceFactory.cs";
        private const string FactoryClassName = "PieceFactory";
        private const string BaseLogicClass = "PieceLogic";
        private const string TargetNamespace = "Game.Piece.PieceLogic.Commons";

        [MenuItem("Tools/PieceLogic Map Generation")]
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
            Debug.Log(
                $"Successfully OVERWROTE {FactoryFilePath} with {pieces.Count} piece entries inside the '{TargetNamespace}' namespace.");
        }

        private static string BuildMethod(List<PieceInfo> pieces)
        {
            var sb = new StringBuilder();

            sb.AppendLine("    public static " + BaseLogicClass + " CreateLogicInstance(string key, PieceConfig cfg)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch (key)");
            sb.AppendLine("            {");
            
            foreach (var piece in pieces.OrderBy(p => p.key))
            {
                if (string.IsNullOrEmpty(piece.key) || string.IsNullOrEmpty(piece.logicClassName))
                {
                    Debug.LogWarning($"Skipping piece: {piece.name} due to missing key or LogicClassName.");
                    continue;
                }

                sb.AppendLine($"                case \"{piece.key}\":");
                sb.AppendLine($"                    return new {piece.logicClassName}(cfg);");
            }
            
            sb.AppendLine("                default:");
            sb.AppendLine("                    Debug.LogError($\"Unknown Piece ID: {key}\");");
            sb.AppendLine("                    return null;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            return sb.ToString();
        }

        private static string BuildFileContent(List<PieceInfo> pieces)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("");

            sb.AppendLine($"namespace {TargetNamespace}");
            sb.AppendLine("{");

            sb.AppendLine($"    public static class {FactoryClassName}");
            sb.AppendLine("    {");

            sb.AppendLine(BuildMethod(pieces));

            sb.AppendLine("    }");

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}