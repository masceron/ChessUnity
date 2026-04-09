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
    public class MutatorValidator : IPreprocessBuildWithReport
    {
        static MutatorValidator()
        {
            Validate();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!Validate())
                throw new BuildFailedException("Architectural violation detected.");
        }

        private const string AssemblyPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";

        private static bool Validate()
        {
            if (!File.Exists(AssemblyPath))
                return true;

            var hasViolations = false;
            AssemblyDefinition assembly;
            var pdbPath = AssemblyPath.Replace(".dll", ".pdb");

            var readerParams = new ReaderParameters { InMemory = true };
            MemoryStream symbolStream = null;

            try
            {
                if (File.Exists(pdbPath))
                {
                    var pdbBytes = File.ReadAllBytes(pdbPath);
                    symbolStream = new MemoryStream(pdbBytes);

                    readerParams.ReadSymbols = true;
                    readerParams.SymbolStream = symbolStream;
                }

                assembly = AssemblyDefinition.ReadAssembly(AssemblyPath, readerParams);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Could not load symbols... Error: {e.Message}");
                assembly = AssemblyDefinition.ReadAssembly(AssemblyPath, new ReaderParameters { InMemory = true });
            }

            using (assembly)
            using (symbolStream)
            {
                foreach (var type in assembly.MainModule.Types)
                {
                    if (IsAllowedToMutate(type)) continue;

                    foreach (var method in type.Methods)
                    {
                        if (!method.HasBody) continue;

                        foreach (var instruction in method.Body.Instructions)
                        {
                            if (instruction.OpCode != OpCodes.Call ||
                                instruction.Operand is not MethodReference methodRef) continue;

                            if (methodRef.DeclaringType?.FullName != "Game.Common.BoardUtils") continue;

                            var resolvedMethod = SafeResolveMethod(methodRef);
                            if (resolvedMethod == null ||
                                resolvedMethod.CustomAttributes.All(a =>
                                    a.AttributeType.Name != "Mutator")) continue;

                            var sequencePoint = GetSequencePoint(method, instruction);
                            var linkText = sequencePoint != null
                                ? $"\n<a href=\"{sequencePoint.Document.Url}\" line=\"{sequencePoint.StartLine}\">Open</a>"
                                : "";

                            Debug.LogError(
                                $"[Architecture Violation] <b>{type.Name}.{method.Name}()</b> is attempting to call <b>{methodRef.Name}()</b>. " +
                                $"Only Internal Actions are permitted to mutate the GameState.{linkText}");

                            hasViolations = true;
                        }
                    }
                }
            }

            return !hasViolations;
        }

        private static bool IsAllowedToMutate(TypeDefinition type)
        {
            if (type.FullName == "Game.Common.BoardUtils") return true;

            var inheritsAction = false;
            var implementsIInternal = false;

            var currentType = type;
            while (currentType != null)
            {
                if (currentType.FullName == "Game.Action.Action")
                {
                    inheritsAction = true;
                }

                if (currentType.Interfaces.Any(i => i.InterfaceType.FullName == "Game.Action.Internal.IInternal"))
                {
                    implementsIInternal = true;
                }

                if (inheritsAction && implementsIInternal)
                    return true;

                if (currentType.BaseType == null) break;
                currentType = SafeResolveType(currentType.BaseType);
            }

            return false;
        }

        private static TypeDefinition SafeResolveType(TypeReference typeRef)
        {
            try
            {
                return typeRef?.Resolve();
            }
            catch
            {
                return null;
            }
        }

        private static MethodDefinition SafeResolveMethod(MethodReference methodRef)
        {
            try
            {
                return methodRef?.Resolve();
            }
            catch
            {
                return null;
            }
        }

        private static SequencePoint GetSequencePoint(MethodDefinition method, Instruction instruction)
        {
            var curr = instruction;
            while (curr != null)
            {
                var sp = method.DebugInformation?.GetSequencePoint(curr);
                if (sp != null) return sp;
                curr = curr.Previous;
            }

            return null;
        }
    }
}