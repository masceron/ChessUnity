using UnityEngine;
using UX.UI.Loader;


namespace UX.UI.Ingame
{
    public class MainMenuButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneLoader.LoadSceneWithLoadingScreen(0);
        }
    }

}
