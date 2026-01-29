using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Third_Party.Autotiles3D.Scripts.Utility
{
    public class Autotiles3DSettings : ScriptableObject
    {
        private static Autotiles3DSettings _settings;
        private const string SettingsPath = "Assets/Third Party/Autotiles3D/Content/Autotiles3D_Settings.asset";

        [FormerlySerializedAs("_useUndoAPI")]
        [FormerlySerializedAs("UseUndoAPI")]
        [SerializeField]
        private bool useUndoAPI; //recommend off, because Unitys Undo API is incredible slow and inefficient.
        public bool UseUndoAPI => useUndoAPI;
        [FormerlySerializedAs("SuppressTileAmountWarning")] public bool suppressTileAmountWarning;

        public void SetUndoAPI(bool value)
        {
            useUndoAPI = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        //for editor use
        public static bool IsLocked;

#if UNITY_EDITOR


        public static Autotiles3DSettings EditorInstance
        {
            get
            {
                if (_settings) return _settings;
                var settings = AssetDatabase.LoadAssetAtPath<Autotiles3DSettings>(SettingsPath);
                if (!settings)
                {
                    settings = CreateInstance<Autotiles3DSettings>();
                    AssetDatabase.CreateAsset(settings, SettingsPath);
                    AssetDatabase.SaveAssets();
                }
                _settings = settings;
                return _settings;
            }
        }
        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(EditorInstance);
        }
#endif
    }



}
