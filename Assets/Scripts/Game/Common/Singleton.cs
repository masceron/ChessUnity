using UnityEngine;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Ins
        {
            get
            {
                if (_instance) return _instance;

                // Find singleton
                _instance = FindFirstObjectByType<T>();

                // Create new instance if one doesn't already exist.
                if (_instance) return _instance;

                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!_instance)
                _instance = this as T;
            else
                Debug.LogError($"Found two instances of {typeof(T)}, second instance will be ignored");
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}