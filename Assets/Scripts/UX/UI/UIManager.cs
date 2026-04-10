using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UX.UI.Common;

namespace UX.UI
{
    public struct Empty
    {
    }

    public class UIManager : Singleton<UIManager>
    {
        [NonSerialized] private UIDocument _mainUIDocument;

        private List<ICloseableUI> _activeMenus;

        private void AssignMainDocument()
        {
            _mainUIDocument = GameObject.Find("MainUI").GetComponent<UIDocument>();
            if (!_mainUIDocument)
            {
                Debug.LogWarning($"[UIManager] No UIDocument found in scene {SceneManager.GetActiveScene().name}!");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _activeMenus = new List<ICloseableUI>();
            AssignMainDocument();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Single) return;
            ClearAll();
            AssignMainDocument();
        }

        public async UniTask<TResult> OpenMenu<TPayload, TResult>(InGameMenuType menuType, TPayload payload)
        {
            UIHolder.Ins.Get(menuType, out var uiAsset);

            var uiInstance = uiAsset.Instantiate();

            uiInstance.style.position = Position.Absolute;
            uiInstance.style.top = 0;
            uiInstance.style.bottom = 0;
            uiInstance.style.left = 0;
            uiInstance.style.right = 0;

            _mainUIDocument.rootVisualElement.Add(uiInstance);
            
            var awaitableUI =
                uiInstance.Children().FirstOrDefault(e => e is IAwaitableUI<TPayload, TResult>) as
                    IAwaitableUI<TPayload, TResult>;

            if (awaitableUI == null)
            {
                uiInstance.RemoveFromHierarchy();
                throw new Exception($"Root element of {menuType} does not implement IAwaitableUI!");
            }

            Push(awaitableUI);

            try
            {
                return await awaitableUI.WaitForSelection(payload);
            }
            finally
            {
                Remove(awaitableUI);
                uiInstance.RemoveFromHierarchy();
            }
        }

        public async UniTask OpenMenu<TPayload>(InGameMenuType menuType, TPayload payload)
        {
            await OpenMenu<TPayload, Empty>(menuType, payload);
        }

        public UniTask<TResult> OpenMenu<TResult>(InGameMenuType menuType)
        {
            return OpenMenu<Empty, TResult>(menuType, new Empty());
        }

        public async UniTask OpenMenu(InGameMenuType menuType)
        {
            await OpenMenu<Empty, Empty>(menuType, new Empty());
        }

        private void Push(ICloseableUI ui)
        {
            if (!_activeMenus.Contains(ui)) _activeMenus.Add(ui);
        }

        public void Pop()
        {
            if (_activeMenus.Count == 0) return;

            var lastIndex = _activeMenus.Count - 1;
            var topMenu = _activeMenus[lastIndex];

            _activeMenus.RemoveAt(lastIndex);

            if (IsAlive(topMenu))
            {
                topMenu.ForceClose();
            }
        }

        private void Remove(ICloseableUI ui) => _activeMenus.Remove(ui);
        private void ClearAll() => _activeMenus.Clear();
        public bool HasActiveMenus => _activeMenus.Count > 0;

        private static bool IsAlive(ICloseableUI ui)
        {
            return ui switch
            {
                null => false,
                VisualElement ve => ve.panel != null,
                _ => true
            };
        }
    }
}