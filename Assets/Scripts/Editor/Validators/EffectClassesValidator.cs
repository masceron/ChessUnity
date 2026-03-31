using System.Linq;
using Game.Effects;
using Game.ScriptableObjects.Collections;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.Validators
{
    [InitializeOnLoad]
    public class EffectClassesValidator : IPreprocessBuildWithReport
    {
        static EffectClassesValidator()
        {
            ValidateEffects();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!ValidateEffects()) throw new BuildFailedException("Effect classes related errors found.");
        }

        private static bool ValidateEffects()
        {
            var dataGuids = AssetDatabase.FindAssets("t:EffectsData");
            if (dataGuids.Length == 0)
            {
                Debug.LogError("No central data object for EffectInfo found.");
                return false;
            }

            var path = AssetDatabase.GUIDToAssetPath(dataGuids[0]);
            var centralData = AssetDatabase.LoadAssetAtPath<EffectsData>(path);

            var idList = centralData.effectsData.ToDictionary(effectInfo => effectInfo.key, _ => false);

            var ok = true;

            var effectTypesByAssembly = TypeCache.GetTypesDerivedFrom<Effect>()
                .Where(t => !t.IsAbstract)
                .GroupBy(t => t.Assembly.Location);

            foreach (var assemblyGroup in effectTypesByAssembly)
            {
                using var assembly = AssemblyDefinition.ReadAssembly(assemblyGroup.Key);
                foreach (var type in assemblyGroup)
                {
                    var typeDef = assembly.MainModule.GetType(type.FullName);

                    var ctor = typeDef?.Methods.FirstOrDefault(m => m.IsConstructor && !m.IsStatic);
                    if (ctor is not { HasBody: true }) continue;

                    foreach (var loadedString in from instruction in ctor.Body.Instructions
                             where instruction.OpCode == OpCodes.Ldstr
                             select (string)instruction.Operand
                             into loadedString
                             where loadedString.StartsWith("effect_")
                             select loadedString)
                    {
                        if (idList.ContainsKey(loadedString)) continue;
                        ok = false;
                        LogViolation(
                            $"The ID {loadedString} in {type.Name} does not exist in EffectsData. Either re-check the ID or rebuild the list.");
                    }
                }
            }

            return ok;
        }

        private static void LogViolation(string msg)
        {
            Debug.LogError($"ERROR: {msg}");
        }
    }
}