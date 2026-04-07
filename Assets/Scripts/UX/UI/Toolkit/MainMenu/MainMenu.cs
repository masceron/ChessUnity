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

            var play = root.Q<Button>("Play");

            play.clicked += LoadMatch;
        }

        private static void LoadMatch()
        {
            SceneLoader.Ins.ChangeScene("Game");
        }
    }
}
