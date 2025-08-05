using UnityEngine;

namespace UX.UI.Loader
{
    public class StartLoader: MonoBehaviour
    {
        private void Awake()
        {
            SceneLoader.Start();
        }
    }
}