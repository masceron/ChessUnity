using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Game.Action.Internal;
using UnityEditor;
using UnityEngine;
using Action = Game.Action.Action;

namespace Editor.Window
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
            
            var classRegex = new Regex($@"(public|internal|private)\s+(abstract\s+)?(sealed\s+)?class\s+{type.Name}\b");
            var match = classRegex.Match(content);

            if (match.Success)
            {
                if (!content.Contains("[MemoryPackable]"))
                {
                    const string insertStr = "[MemoryPackable]\n    ";
                    content = content.Insert(match.Index, insertStr);
                    
                    match = classRegex.Match(content);
                    modified = true;
                }

                var classIndex = match.Index;
                var classLine = content.Substring(classIndex, match.Length);

                if (!classLine.Contains("partial"))
                {
                    var newClassLine = classLine.Replace("class", "partial class");
                    content = content.Remove(classIndex, match.Length).Insert(classIndex, newClassLine);
                    modified = true;
                }
            }
            
            var fieldRegex = new Regex(@"(?<indent>^\s*)private\s+(?!static|const|void)(?:readonly\s+)?[\w.<>\[\]?]+\s+\w+\s*(?:=.*?)?;", RegexOptions.Multiline);
            
            var newContent = fieldRegex.Replace(content, (m) => 
            {
                var precedingText = content[..m.Index].TrimEnd();
                
                if (precedingText.EndsWith("MemoryPackInclude]") || precedingText.EndsWith("MemoryPackIncludeAttribute]"))
                {
                    return m.Value;
                }
                
                modified = true;
                var indent = m.Groups["indent"].Value;
                return $"{indent}[MemoryPackInclude]\n{m.Value}";
            });

            if (!modified) return false;
            File.WriteAllText(path, newContent);
            return true;
        }
        
        [MenuItem("Tools/Action System Tools/Create formatter")]
        private static void UpdateBaseClassUnions()
        {
            var baseType = typeof(Action);
            
            var guids = AssetDatabase.FindAssets("t:MonoScript " + baseType.Name);
            var path = (from guid in guids
                select AssetDatabase.GUIDToAssetPath(guid)
                into p
                let script = AssetDatabase.LoadAssetAtPath<MonoScript>(p)
                where script && script.GetClass() == baseType
                select p).FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"Could not find source file for class: {baseType.Name}");
                return;
            }
            
            var actionTypes = TypeCache.GetTypesDerivedFrom<Action>()
                .Where(t => !t.IsAbstract && !t.IsGenericType)
                .Where(t => !typeof(IInternal).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToList();

            var content = File.ReadAllText(path);
            
            var sb = new StringBuilder();
            sb.AppendLine("[MemoryPackable]");
            
            for (var i = 0; i < actionTypes.Count; i++)
            {
                var type = actionTypes[i];
                sb.AppendLine($"    [MemoryPackUnion({i}, typeof({type.Name}))]");
            }

            sb.Append("     ");
            
            var regex = new Regex(@"\[MemoryPackable\]([\s\S]*?)(?=\bpublic\s+(abstract\s+partial|partial\s+abstract)\s+class\s+Action\b)");

            if (!regex.IsMatch(content))
            {
                var classRegex = new Regex(@"(?=\bpublic\s+(abstract\s+partial|partial\s+abstract)\s+class\s+Action\b)");
                if (classRegex.IsMatch(content))
                {
                    content = classRegex.Replace(content, sb.ToString(), 1);
                    File.WriteAllText(path, content);
                    AssetDatabase.Refresh();
                    Debug.Log($"<color=cyan>Action.cs updated! Added {actionTypes.Count} unions.</color>");
                    return;
                }
                
                Debug.LogError("Could not find 'public abstract partial class Action' in the file. Check formatting.");
                return;
            }
            
            var newContent = regex.Replace(content, sb.ToString(), 1);

            if (content != newContent)
            {
                File.WriteAllText(path, newContent);
                AssetDatabase.Refresh();
                Debug.Log($"<color=cyan>Action.cs updated! Refreshed {actionTypes.Count} unions.</color>");
            }
            else
            {
                Debug.Log("Action.cs Unions are already up to date.");
            }
        }
    }
}