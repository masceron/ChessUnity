using UnityEngine;
using UnityEngine.UIElements;
using SceneLoader = UX.UI.Toolkit.Loader.SceneLoader;

namespace UX.UI.Toolkit.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<Button>("Play").clicked += LoadMatch;
            root.Q<Button>("Quit").clicked += () => Application.Quit(0);
        }

        private static void LoadMatch()
        {
            SceneLoader.Ins.ChangeScene("Game");
        }
    }
}
