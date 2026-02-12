using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UX.UI.Ingame;

namespace Editor.Validators
{
    [InitializeOnLoad]
    public class IngameUIValidator: IPreprocessBuildWithReport
    {
        static IngameUIValidator()
        {
            ScanUIViolations();
        }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (ScanUIViolations())
            {
                throw new BuildFailedException("Action architecture violations found.");
            }
        }

        private static bool ScanUIViolations()
        {
            var baseType = typeof(IngamePendingMenu);
            
            var childTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType));

            var foundErrors = false;
            
            foreach (var type in childTypes)
            {
                var illegalOnDisable = type.GetMethod("OnDisable", 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                
                if (illegalOnDisable == null) continue;
                Debug.LogError($"<b>ERROR:</b> The class <color=cyan>{type.Name}</color> inherits from {baseType.Name} and implements <b>OnDisable()</b>.");
                foundErrors = true;
            }

            return foundErrors;
        }

        public int callbackOrder => 0;
    }
}