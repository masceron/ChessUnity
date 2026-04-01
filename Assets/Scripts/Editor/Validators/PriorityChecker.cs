using System;
using System.Linq;
using System.Reflection;
using Game.Triggers;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Editor.Validators
{
    [InitializeOnLoad]
    public class PriorityChecker : IPreprocessBuildWithReport
    {
        private static readonly Type[] TargetTypes =
        {
            typeof(IAfterPieceActionTrigger), typeof(IAfterRelicActionTrigger), typeof(IBeforeApplyEffectTrigger),
            typeof(IBeforePieceActionTrigger), typeof(IBeforeRelicActionTrigger), typeof(IDeadTrigger),
            typeof(IEffectStatModifierTrigger), typeof(IEndTurnTrigger), typeof(IMoveRangeModifierTrigger),
            typeof(IOnApplyTrigger), typeof(IOnMoveGenTrigger), typeof(IOnPieceSpawnedTrigger),
            typeof(IOnRemoveTrigger), typeof(ISkillStatModifierTrigger), typeof(IStartTurnTrigger)
        };

        static PriorityChecker()
        {
            CheckPriorities();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (CheckPriorities()) throw new BuildFailedException("Trigger architecture violations found.");
        }

        private static bool CheckPriorities()
        {
            var gameAssembly = Assembly.Load("Assembly-CSharp");

            var implementingTypes = gameAssembly.GetTypes()
                .Where(t => TargetTypes.Any(type => type.IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract);

            var hasError = false;

            foreach (var type in implementingTypes)
            {
                var property = type.GetProperty("Priority",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                if (property == null) continue;

                var backingFieldName = $"<{property.Name}>k__BackingField";
                var backingField = type.GetField(backingFieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (backingField == null) continue;
                Debug.LogError(
                    $"Trigger priority for '{type.Name}' not set. Use expression body (=>) to set the value.");
                hasError = true;
            }

            return hasError;
        }
    }
}