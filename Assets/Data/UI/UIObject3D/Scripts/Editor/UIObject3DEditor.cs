/*
 * TODO:
 * 1) Change to 'targets' and allow multi object editing
 */
#region Namespace Imports

using System.Collections.Generic;
using System.Linq;
using Data.UI.UIObject3D.Scripts;
using UnityEditor;
using UnityEngine;

#endregion

namespace UI.UIObject3D.Scripts.Editor
{
    [CustomEditor(typeof(Data.UI.UIObject3D.Scripts.UIObject3D)), CanEditMultipleObjects]
    public class UIObject3DEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Dictionary<Data.UI.UIObject3D.Scripts.UIObject3D, Transform> targetPrefabs = new Dictionary<Data.UI.UIObject3D.Scripts.UIObject3D, Transform>();
            targetPrefabs = targets.ToDictionary(k => k as Data.UI.UIObject3D.Scripts.UIObject3D, v => (v as Data.UI.UIObject3D.Scripts.UIObject3D).ObjectPrefab);
            Dictionary<Data.UI.UIObject3D.Scripts.UIObject3D, float> renderScales = targets.ToDictionary(k => k as Data.UI.UIObject3D.Scripts.UIObject3D, v => (v as Data.UI.UIObject3D.Scripts.UIObject3D).RenderScale);

            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Force Render"))
            {
                foreach (var t in targetPrefabs)
                {
                    t.Key.HardUpdateDisplay();
                }
            }

            EditorGUILayout.Space();

            base.OnInspectorGUI();

            if (!EditorGUI.EndChangeCheck()) return;

            foreach (var t in targetPrefabs)
            {
                if (t.Key.ObjectPrefab != t.Value
                || !Mathf.Approximately(renderScales[t.Key], t.Key.RenderScale))
                {
                    t.Key.SetStarted();
                    t.Key.HardUpdateDisplay();
                    UIObject3DTimer.AtEndOfFrame(() => t.Key.UpdateDisplay(), t.Key);
                }
                else
                {
                    t.Key.UpdateDisplay(true);
                }
            }

        }
    }
}
