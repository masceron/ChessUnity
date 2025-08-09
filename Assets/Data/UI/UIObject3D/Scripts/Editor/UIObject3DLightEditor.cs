/*
 * TODO:
 * 1) Change to 'targets' and allow multi object editing
 */
#region Namespace Imports

using System.Linq;
using Data.UI.UIObject3D.Scripts;
using UnityEditor;

#endregion

namespace UI.UIObject3D.Scripts.Editor
{
    [CustomEditor(typeof(UIObject3DLight)), CanEditMultipleObjects]
    public class UIObject3DLightEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (!EditorGUI.EndChangeCheck()) return;

            targets.Cast<UIObject3DLight>()
                   .ToList()
                   .ForEach((l) =>
                   {
                       l.UpdateLight(true);
                   });
        }
    }
}
