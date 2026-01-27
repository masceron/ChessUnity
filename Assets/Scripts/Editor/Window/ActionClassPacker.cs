using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Game.Action.Internal;
using UnityEditor;
using UnityEngine;
using Action = Game.Action.Action;

namespace Editor.Window
{
    public class ActionClassPacker : EditorWindow
    {
        [MenuItem("Tools/Fix Action Classes")]
        public static void RunFixer()
        {
            var actionTypes = TypeCache.GetTypesDerivedFrom<Action>()
                .Where(t => !t.IsAbstract) // Skip abstract classes
                .Where(t => !typeof(IInternal).IsAssignableFrom(t)) // Skip IInternal
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
                where script != null && script.GetClass() == type
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

            if (!modified) return false;
            File.WriteAllText(path, content);
            return true;
        }
    }
}