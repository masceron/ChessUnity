using UnityEngine;
using UX.UI.Loader;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MenuPanel : MonoBehaviour
    {
        public void OnClickPlay()
        {
            SceneLoader.LoadSceneWithLoadingScreen(3);
        }

        public void OnClickSettings()
        {
            UIManager.Ins.Load(CanvasID.Settings);
        }

        public void OnClickFreePlayTest()
        {
            SceneLoader.LoadSceneWithLoadingScreen(2);
        }

        public void OnClickMapMode()
        {
            SceneLoader.LoadSceneWithLoadingScreen(4);
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
    }
}
