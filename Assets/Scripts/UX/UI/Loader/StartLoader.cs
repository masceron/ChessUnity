using PrimeTween;
using UnityEngine;

namespace UX.UI.Loader
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StartLoader: MonoBehaviour
    {
        private void Awake()
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
            SceneLoader.Start();
        }
    }
}