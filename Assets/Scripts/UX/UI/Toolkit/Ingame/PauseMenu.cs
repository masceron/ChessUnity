using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Toolkit.Common;

namespace UX.UI.Toolkit.Ingame
{
    public class PauseMenu : MonoBehaviour
    {
        [NonSerialized] private InputManager _inputManager;
        [NonSerialized] private VisualElement _inGameHUD;
        [NonSerialized] private UIDocument _mainDoc;
        private const int FadeTime = 150;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _mainDoc = GetComponent<UIDocument>();
            _inGameHUD = _mainDoc.rootVisualElement.Q<VisualElement>("InGameRoot");
        }

        private void OnEnable()
        {
            _inputManager.OnMenuToggle += MenuClick;
        }

        private void OnDisable()
        {
            _inputManager.OnMenuToggle -= MenuClick;
        }

        private async void MenuClick()
        {
            _inGameHUD.AddToClassList("hud-container--hidden");
            await UniTask.Delay(FadeTime);
            try
            {
                await UIManager.Ins.OpenMenu(InGameMenuType.PauseMenu, _mainDoc);
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _inGameHUD.RemoveFromClassList("hud-container--hidden");
                await UniTask.Delay(FadeTime);
            }
        }
    }
}