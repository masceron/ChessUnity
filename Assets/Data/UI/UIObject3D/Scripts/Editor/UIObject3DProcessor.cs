#region Namespace Imports

using System.Linq;
using Data.UI.UIObject3D.Scripts;
using UnityEditor;
using UnityEngine;

#endregion

namespace UI.UIObject3D.Scripts.Editor
{
    public class UIObject3DProcessor: AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            CleanupAllObjects();

            if (!Application.isPlaying) DestroySceneContainers();
            
            return paths;
        }

        /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoadRuntimeMethod()
        {            
            var containers = GameObject.FindObjectsOfType<UIObject3DContainer>().ToList();

            // Error check - if, somehow, we have multiple scene containers,
            // cleanup and then continue
            if (containers.Count > 1)
            {
                DestroySceneContainers();
                CleanupAllObjects();
            }
        }*/

        private enum ePlayMode
        {
            Stopped,
            Playing,
            Paused            
        }
        private static ePlayMode PlayMode = ePlayMode.Stopped;

        [InitializeOnLoadMethod]
        static void HandlePlayModeSwitches()
        {
#if UNITY_2017_3_OR_NEWER
            EditorApplication.playModeStateChanged += a =>
#else
            EditorApplication.playmodeStateChanged += () =>
#endif
            {
                var previousMode = PlayMode;

                if (EditorApplication.isPaused) PlayMode = ePlayMode.Paused;
                else
                {
                    PlayMode = EditorApplication.isPlaying ? ePlayMode.Playing : ePlayMode.Stopped;
                }

                if (PlayMode == previousMode) return;
                if (PlayMode == ePlayMode.Paused) return;

                if (previousMode == ePlayMode.Paused) return;
                var containers = Object.FindObjectsByType<UIObject3DContainer>(FindObjectsSortMode.None).ToList();

                // Error check - if, somehow, we have multiple scene containers,
                // cleanup and then continue
                if (containers.Count > 1)
                {                        
                    DestroySceneContainers();
                    CleanupAllObjects();
                }
            };
        }        

        private static void CleanupAllObjects()
        {
            var objects = Object.FindObjectsByType<Data.UI.UIObject3D.Scripts.UIObject3D>(FindObjectsSortMode.None).ToList();

            foreach (var o in objects)
            {
                o.Cleanup();
            }
        }

        private static void DestroySceneContainers()
        {
            var containers = Object.FindObjectsByType<UIObject3DContainer>(FindObjectsSortMode.None).ToList();
            containers.ForEach(c => Object.DestroyImmediate(c.gameObject));            
        }
    }
}
