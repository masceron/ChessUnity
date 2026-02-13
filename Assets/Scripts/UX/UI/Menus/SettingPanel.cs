using UnityEngine;
using UnityEngine.InputSystem;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SettingPanel : MonoBehaviour
    {
        public static float AIvsAIplayspeed = 1f;
        public SpeedSwitcher speedSwitcher;

        private void Awake()
        {
            speedSwitcher.SetValue(PlayerPrefs.GetInt("AIvsAIplayspeed"));
        }

        public void OnClickBack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.MainMenu);
        }

        public void ReturnToMainMenu()
        {
            UIManager.Ins.Load(CanvasID.MainMenu);
        }
    }
}