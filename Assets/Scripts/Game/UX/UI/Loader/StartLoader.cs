using System;
using UnityEngine;

namespace Game.UX.UI.Loader
{
    public class StartLoader: MonoBehaviour
    {
        private void Awake()
        {
            SceneLoader.Start();
        }
    }
}