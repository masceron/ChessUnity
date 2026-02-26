#if UNITY_2020_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Third_Party.Autotiles3D.Scripts.Utility.Editor
{
    public class Autotiles3D_SettingsProvider : SettingsProvider
    {
        private Autotiles3DSettings _settings;

        public Autotiles3D_SettingsProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope)
        {
        }

        public static bool IsSettingsAvailable()
        {
            var settings = Autotiles3DSettings.EditorInstance;
            return settings != null;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            _settings = Autotiles3DSettings.EditorInstance;
        }

        public override void OnGUI(string searchContext)
        {
            // Use IMGUI to display UI:
            if (_settings != null)
            {
                _settings.suppressTileAmountWarning = EditorGUILayout.Toggle(Styles.Suppress,
                    _settings.suppressTileAmountWarning, GUILayout.Width(200));
                EditorGUILayout.LabelField("(Experimental)");
                var useUndo = EditorGUILayout.Toggle(Styles.UndoAPI, _settings.UseUndoAPI);
                if (useUndo != _settings.UseUndoAPI)
                    _settings.SetUndoAPI(useUndo);
            }
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateAutotilesSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new Autotiles3D_SettingsProvider("Project/Autotiles3D", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        private class Styles
        {
            public static readonly GUIContent UndoAPI = new("Use Undo API");
            public static readonly GUIContent Suppress = new("Suppress high tile amount warning");
        }
    }
}
#endif