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
                
                _instance = FindFirstObjectByType<T>();
                
                if (_instance) return _instance;
                
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}