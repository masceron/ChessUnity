using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.Validators
{
    [InitializeOnLoad]
    public class ActionManagerGuard : IPreprocessBuildWithReport
    {
        private const string TargetMethod = "ExecuteImmediately";
        private const string TargetClass = "ActionManager";
        private const string AssemblyPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";

        static ActionManagerGuard()
        {
            EditorApplication.delayCall += () => ScanViolations();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (ScanViolations())
                throw new BuildFailedException(
                    "⛔ Architecture Violation: 'ExecuteImmediately' detected in illegal locations. Check console for details.");
        }

        private static bool ScanViolations()
        {
            AssemblyDefinition assembly;
            try
            {
                assembly = AssemblyDefinition.ReadAssembly(AssemblyPath);
            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("⚠️ Could not find Assembly-CSharp.dll. Skipping architecture check.");
                return false;
            }

            var violationFound = false;

            foreach (var type in assembly.MainModule.Types)
            {
                if (type.Name.StartsWith("<") || type.Namespace.StartsWith("Unity")) continue;

                foreach (var method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    foreach (var instruction in method.Body.Instructions.Where(instruction =>
                                 instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt))
                    {
                        if (instruction.OpCode != OpCodes.Call && instruction.OpCode != OpCodes.Callvirt) continue;

                        if (instruction.Operand is not MethodReference methodRef) continue;

                        if (methodRef.Name != TargetMethod || methodRef.DeclaringType.Name != TargetClass) continue;
                        if (IsUsageAllowed(type, method)) continue;
                        LogViolation(type, method);
                        violationFound = true;
                    }
                }
            }

            assembly.Dispose();
            return violationFound;
        }

        private static bool IsUsageAllowed(TypeDefinition callerType, MethodDefinition callerMethod)
        {
            if (callerType.Name == "MatchManager") return true;

            if (InheritsFrom(callerType, "PieceLogic")) return callerMethod.Name == ".ctor";

            return false;
        }

        private static bool InheritsFrom(TypeDefinition type, string baseClassName)
        {
            TypeReference current = type;

            var safety = 0;
            while (current != null && safety++ < 20)
            {
                if (current.Name == baseClassName) return true;

                try
                {
                    if (current is TypeDefinition { BaseType: not null } def)
                        current = def.BaseType;
                    else
                        break;
                }
                catch
                {
                    break;
                }
            }

            return false;
        }

        private static void LogViolation(TypeDefinition type, MethodDefinition method)
        {
            Debug.LogError("ERROR: " + $"Caller: <b>{type.Name}</b>::<b>{method.Name}</b>\n" +
                           $"In calling <b>{TargetClass}.{TargetMethod}</b>. This function is strictly limited to PieceLogic Constructors only.");
        }
    }
}