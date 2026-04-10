using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Common;
using SceneLoader = UX.UI.Loader.SceneLoader;

namespace UX.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<Button>("Play").clicked += LoadMatch;
            root.Q<Button>("Quit").clicked += () => Application.Quit(0);
            root.Q<Button>("FreePlayTest").clicked +=
                () => UIManager.Ins.OpenMenu(InGameMenuType.FreePlayTest).Forget();
        }

        private static void LoadMatch()
        {
            SceneLoader.Ins.ChangeScene("Game");
        }
    }
}