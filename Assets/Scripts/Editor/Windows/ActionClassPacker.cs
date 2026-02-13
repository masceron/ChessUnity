using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Game.Action.Internal;
using UnityEditor;
using UnityEngine;
using Action = Game.Action.Action;

namespace Editor.Windows
{
    public class ActionClassPacker : EditorWindow
    {
        [MenuItem("Tools/Action System Tools/Fix Action definitions")]
        private static void RunFixer()
        {
            var actionTypes = TypeCache.GetTypesDerivedFrom<Action>()
                .Where(t => !t.IsAbstract)
                .Where(t => !typeof(IInternal).IsAssignableFrom(t))
                .ToList();

            var fixedCount = actionTypes.Count(FixClass);

            if (fixedCount > 0)
            {
                AssetDatabase.Refresh();
                Debug.Log($"<color=green>Successfully updated {fixedCount} Action classes!</color>");
            }
            else
            {
                Debug.Log("All Action classes are already correct.");
            }
        }

        private static bool FixClass(Type type)
        {
            var guids = AssetDatabase.FindAssets("t:MonoScript " + type.Name);
            var path = (from guid in guids
                select AssetDatabase.GUIDToAssetPath(guid)
                into p
                let script = AssetDatabase.LoadAssetAtPath<MonoScript>(p)
                where script && script.GetClass() == type
                select p).FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"Could not find source file for class: {type.Name}");
                return false;
            }

            var content = File.ReadAllText(path);
            var modified = false;

            if (!content.Contains("using MemoryPack;"))
            {
                content = "using MemoryPack;\n" + content;
                modified = true;
            }

            var classRegex =
                new Regex($@"(public|internal|private)\s+(abstract\s+)?(sealed\s+)?(partial\s+)?class\s+{type.Name}\b");
            var match = classRegex.Match(content);

            if (match.Success)
            {
                if (!content.Contains("[MemoryPackable]"))
                {
                    const string insertStr = "[MemoryPackable]\n    ";
                    content = content.Insert(match.Index, insertStr);
                    modified = true;

                    match = classRegex.Match(content);
                }

                if (!match.Value.Contains("partial"))
                {
                    var oldValue = match.Value;
                    var newValue = oldValue.Replace("class", "partial class");

                    content = content.Remove(match.Index, oldValue.Length).Insert(match.Index, newValue);
                    modified = true;

                    match = classRegex.Match(content);
                }

                var ctorRegex = new Regex($@"(public|protected|internal|private)\s+{type.Name}\s*\(\s*\)");

                if (!ctorRegex.IsMatch(content))
                {
                    var openBraceIndex = content.IndexOf('{', match.Index + match.Length);
                    if (openBraceIndex != -1)
                    {
                        var ctorCode = $"\n        [MemoryPackConstructor]\n        private {type.Name}() {{ }}\n";
                        content = content.Insert(openBraceIndex + 1, ctorCode);
                        modified = true;
                    }
                }
            }

            var fieldRegex = new Regex(
                @"(?<indent>^\s*)private\s+(?!static|const|void)(?:readonly\s+)?[\w.<>\[\]?]+\s+\w+\s*(?:=.*?)?;",
                RegexOptions.Multiline);

            var contentCopy = content;
            content = fieldRegex.Replace(content, m =>
            {
                var precedingText = contentCopy[..m.Index].TrimEnd();
                var hasAttribute = precedingText.EndsWith("MemoryPackInclude]") ||
                                   precedingText.EndsWith("MemoryPackIncludeAttribute]");

                var text = m.Value;

                var hasReadonly = text.Contains("readonly");

                if (hasAttribute && !hasReadonly) return m.Value;

                modified = true;

                if (hasReadonly) text = Regex.Replace(text, @"\breadonly\s+", "");

                if (hasAttribute) return text;
                var indent = m.Groups["indent"].Value;
                return $"{indent}[MemoryPackInclude]\n{text}";
            });

            if (!modified) return false;
            File.WriteAllText(path, content);
            return true;
        }

        [MenuItem("Tools/Action System Tools/Generate Network Unions")]
        public static void GenerateUnions()
        {
            var baseType = typeof(Action);

            var subTypes = TypeCache.GetTypesDerivedFrom<Action>()
                .Where(t => !t.IsAbstract && !t.IsGenericType)
                .Where(t => !typeof(IInternal).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("using MemoryPack;");
            sb.AppendLine("using Game.Action.Captures;");
            sb.AppendLine("using Game.Action.Quiets;");
            sb.AppendLine("using Game.Action.Skills;");
            sb.AppendLine("using Game.Action.Relics;");
            sb.AppendLine("");
            sb.AppendLine("namespace Game.Action");
            sb.AppendLine("{");

            for (var i = 0; i < subTypes.Count; i++)
                sb.AppendLine($"    [MemoryPackUnion({i}, typeof({subTypes[i].Name}))]");

            sb.AppendLine($"    public abstract partial class {baseType.Name}");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            var path = Path.Combine(Application.dataPath, "Scripts/Game/Action/ActionSerializer.cs");
            File.WriteAllText(path, sb.ToString());

            AssetDatabase.Refresh();
            Debug.Log($"Generated {subTypes.Count} unions for {baseType.Name}.");
        }
    }
}