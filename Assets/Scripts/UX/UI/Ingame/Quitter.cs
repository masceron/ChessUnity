using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.Loader;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Quitter: MonoBehaviour
    {
        [SerializeField] private RectTransform menuRect;

        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            Cancel();
        }

        public void Cancel()
        {
            UIManager.Ins.Load(CanvasID.Ingame);
        }

        public void OnEnable()
        {
            menuRect.rotation = new Quaternion
            {
                eulerAngles = new Vector3(90, 0, 0)
            };
            menuRect.DORotate(new Vector3(0, 0, 0), 0.15f);
        }

        public void QuitToMainMenu()
        {
            SceneLoader.LoadSceneWithLoadingScreen(0);
        }

        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}