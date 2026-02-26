using System;
using System.Linq;
using System.Reflection;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using MemoryPack;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Action = Game.Action.Action;

namespace Editor.Validators
{
    [InitializeOnLoad]
    public class ActionsValidator : IPreprocessBuildWithReport
    {
        static ActionsValidator()
        {
            ValidateArchitecture();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!ValidateArchitecture()) throw new BuildFailedException("Action architecture violations found.");
        }

        private static bool ValidateArchitecture()
        {
            var allGood = true;

            var actionTypes = TypeCache.GetTypesDerivedFrom<Action>()
                .Where(t => !t.IsAbstract)
                .ToList();

            foreach (var type in actionTypes)
            {
                var ns = type.Namespace ?? "";
                var isInternalNamespace = ns.StartsWith("Game.Action.Internal");
                var hasInternalInterface = typeof(IInternal).IsAssignableFrom(type);
                var isPendingNameSpace = ns.StartsWith("Game.Action.Internal.Pending");
                var hasPendingClass = typeof(PendingAction).IsAssignableFrom(type);

                switch (isPendingNameSpace)
                {
                    case true when !hasPendingClass:
                        LogViolation($"'{type.Name}' is in '{ns}' but not inherited from PendingAction.");
                        allGood = false;
                        break;
                    case false when hasPendingClass:
                        LogViolation($"'{type.Name}' has PendingAction but is NOT in Pending namespace.");
                        allGood = false;
                        break;
                }

                switch (isInternalNamespace)
                {
                    case true when !hasInternalInterface:
                        LogViolation($"'{type.Name}' is in '{ns}' but not inherited from IInternal.");
                        allGood = false;
                        break;
                    case false when hasInternalInterface:
                        LogViolation($"'{type.Name}' has IInternal but is NOT in Internal namespace.");
                        allGood = false;
                        break;
                }

                if (hasInternalInterface) continue;

                if (!ValidateFieldTypes(type)) allGood = false;
            }

            return allGood;
        }

        private static bool ValidateFieldTypes(Type actionType)
        {
            var clean = true;

            var fields = actionType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (field.IsStatic || field.IsLiteral) continue;
                if (field.GetCustomAttribute<MemoryPackIgnoreAttribute>() != null) continue;

                if (IsTypeNetworkSafe(field.FieldType, out var failureReason)) continue;

                LogViolation(
                    $"Action '{actionType.Name}' contains a forbidden type. Trace: {field.Name} -> {failureReason}");
                clean = false;
            }

            return clean;
        }

        private static bool IsTypeNetworkSafe(Type t, out string failureReason)
        {
            failureReason = null;

            if (t.IsPrimitive || t.IsEnum) return true;
            if (t == typeof(string) || t == typeof(Guid) || t == typeof(DateTime) || t == typeof(TimeSpan)) return true;

            if (t.IsArray)
            {
                var elementType = t.GetElementType();
                if (IsTypeNetworkSafe(elementType, out var elementFailure)) return true;
                failureReason = $"Array[{elementType.Name}] -> {elementFailure}";
                return false;
            }

            if (t.IsGenericType)
            {
                foreach (var arg in t.GetGenericArguments())
                {
                    if (IsTypeNetworkSafe(arg, out var genericFailure)) continue;
                    failureReason = $"Generic<{arg.Name}> -> {genericFailure}";
                    return false;
                }

                return true;
            }

            if (t.IsValueType)
            {
                var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    if (field.IsStatic || field.IsLiteral) continue;
                    if (field.GetCustomAttribute<MemoryPackIgnoreAttribute>() != null) continue;

                    if (IsTypeNetworkSafe(field.FieldType, out var structFieldFailure)) continue;
                    failureReason = $"{t.Name}.{field.Name} -> {structFieldFailure}";
                    return false;
                }

                return true;
            }

            failureReason = $"[FORBIDDEN: {t.Name}]";
            return false;
        }

        private static void LogViolation(string msg)
        {
            Debug.LogError($"ERROR: {msg}");
        }
    }
}