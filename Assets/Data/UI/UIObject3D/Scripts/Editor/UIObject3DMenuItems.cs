using System.Linq;
using Data.UI.UIObject3D.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UIObject3D.Scripts.Editor
{
    public class UIObject3DMenuItems
    {        
        [MenuItem("GameObject/UI/UIObject3D")]
        private static void NewUIObject3D()
        {                        
            var parent = Selection.activeTransform;     

            if (parent == null || !(parent is RectTransform))
            {
                parent = GetCanvasTransform();
            }

            var prefabGUID = AssetDatabase.FindAssets("UIObject3D t:GameObject").FirstOrDefault();
            if (prefabGUID == null) return;
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabGUID), typeof(GameObject));                

            var newUIObject3D = Object.Instantiate(prefab, parent, true);

            newUIObject3D.name = "UIObject3D";

            var transform = newUIObject3D.transform as RectTransform;

            transform.localPosition = Vector3.zero;
            transform.anchoredPosition3D = Vector3.zero;                
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
                
            transform.SetParent(parent);

            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;

            var uiObject3D = newUIObject3D.GetComponent<Data.UI.UIObject3D.Scripts.UIObject3D>();
              
            UIObject3DTimer.DelayedCall(0.01f, () => uiObject3D.HardUpdateDisplay(), uiObject3D);
        }

        private static Transform GetCanvasTransform()
        {
            Canvas canvas = null;
#if UNITY_EDITOR
            // Attempt to locate a canvas object parented to the currently selected object
            if (!Application.isPlaying && Selection.activeGameObject != null)
            {
                canvas = FindParentOfType<Canvas>(Selection.activeGameObject);                
            }
#endif

            if (!canvas)
            {
                // Attempt to find a canvas anywhere
                canvas = Object.FindAnyObjectByType<Canvas>();

                if (canvas) return canvas.transform;
            }

            // if we reach this point, we haven't been able to locate a canvas
            // ...So I guess we'd better create one

            var canvasGameObject = new GameObject("Canvas")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGameObject.AddComponent<CanvasScaler>();
            canvasGameObject.AddComponent<GraphicRaycaster>();

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(canvasGameObject, "Create Canvas");
#endif

            var eventSystem = Object.FindAnyObjectByType<EventSystem>();

            if (eventSystem != null) return canvas.transform;
            
            var eventSystemGameObject = new GameObject("EventSystem");
            eventSystemGameObject.AddComponent<EventSystem>();
            eventSystemGameObject.AddComponent<StandaloneInputModule>();

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                eventSystemGameObject.AddComponent<TouchInputModule>();
#endif

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(eventSystemGameObject, "Create EventSystem");
#endif

            return canvas.transform;
        }

        private static T FindParentOfType<T>(GameObject childObject)
            where T : Object
        {
            Transform t = childObject.transform;
            while (t.parent != null)
            {
                var component = t.parent.GetComponent<T>();

                if (component != null) return component;

                t = t.parent.transform;
            }

            // We didn't find anything
            return null;
        }
    }
}
