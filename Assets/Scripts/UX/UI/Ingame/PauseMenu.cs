using System;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Common;

namespace UX.UI.Ingame
{
    public class PauseMenu : MonoBehaviour
    {
        [NonSerialized] private InputManager _inputManager;
        [NonSerialized] private VisualElement _inGameHUD;
        [NonSerialized] private UIDocument _mainDoc;
        private const int FadeTime = 180;

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
            await _inGameHUD.AnimateIn("hud-container--hidden", FadeTime);
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
                await _inGameHUD.AnimateOut("hud-container--hidden", FadeTime);
            }
        }
    }
}