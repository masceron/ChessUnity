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
    public class FriendAccessValidator : IPreprocessBuildWithReport
    {
        private const string AssemblyPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";

        public int callbackOrder => 0;

        static FriendAccessValidator()
        {
            Validate();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!Validate()) throw new BuildFailedException("Architecture violations found.");
        }

        private static bool Validate()
        {
            var ok = true;

            var readerParams = new ReaderParameters { ReadSymbols = true };
            AssemblyDefinition assembly;
            try
            {
                assembly = AssemblyDefinition.ReadAssembly(AssemblyPath, readerParams);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(
                    $"Could not load symbols for Assembly-CSharp.dll. Validation will run without clickable links. Error: {e.Message}");
                assembly = AssemblyDefinition.ReadAssembly(AssemblyPath); // Fallback without symbols
            }

            var modules = assembly.Modules;

            foreach (var type in modules.SelectMany(m => m.Types))
            {
                var actualCallerType = type.DeclaringType ?? type;

                foreach (var method in type.Methods.Where(m => m.HasBody))
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        if (instruction.Operand is not MemberReference memberRef) continue;
                        if (memberRef.DeclaringType == null) continue;

                        var declaringType = SafeResolveType(memberRef.DeclaringType);
                        if (declaringType == null) continue;

                        var attr = declaringType.CustomAttributes
                            .FirstOrDefault(a => a.AttributeType.Name == "FriendAttribute");

                        if (attr == null)
                        {
                            var resolvedMember = SafeResolveMember(memberRef);
                            if (resolvedMember != null)
                            {
                                attr = resolvedMember.CustomAttributes
                                    .FirstOrDefault(a => a.AttributeType.Name == "FriendAttribute");
                            }
                        }

                        if (attr == null) continue;
                        if (actualCallerType.FullName == declaringType.FullName ||
                            actualCallerType.DeclaringType?.FullName == declaringType.FullName)
                        {
                            continue;
                        }

                        var allowedTypes = ((CustomAttributeArgument[])attr.ConstructorArguments[0].Value)
                            .Select(arg => (TypeReference)arg.Value)
                            .ToList();

                        if (allowedTypes.Any(at => at.FullName == actualCallerType.FullName)) continue;
                        var sequencePoint = GetSequencePoint(method, instruction);

                        var linkText = "";
                        if (sequencePoint != null)
                        {
                            linkText =
                                $"\n<a href=\"{sequencePoint.Document.Url}\" line=\"{sequencePoint.StartLine}\">Open</a>";
                        }

                        LogViolation($"Illegal Access: <b>{actualCallerType.Name}.{method.Name}</b> " +
                                     $"is accessing <b>{memberRef.Name}</b> (owned by {declaringType.Name}). " +
                                     $"This is restricted to specific friend classes.{linkText}");
                        ok = false;
                    }
                }
            }

            return ok;
        }

        private static void LogViolation(string msg)
        {
            Debug.LogError($"ERROR: {msg}");
        }

        private static TypeDefinition SafeResolveType(TypeReference typeRef)
        {
            try
            {
                return typeRef.Resolve();
            }
            catch
            {
                return null;
            }
        }

        private static IMemberDefinition SafeResolveMember(MemberReference memberRef)
        {
            try
            {
                return memberRef.Resolve();
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
                var sp = method.DebugInformation.GetSequencePoint(curr);
                if (sp != null) return sp;
                curr = curr.Previous;
            }

            return null;
        }
    }
}