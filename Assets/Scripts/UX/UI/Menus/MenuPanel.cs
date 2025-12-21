using UnityEngine;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MenuPanel : MonoBehaviour
    {
        public void OnClickPlay()
        {
            UIManager.Ins.Load(CanvasID.PlayMenu);
        }

        public void OnClickSettings()
        {
            UIManager.Ins.Load(CanvasID.Settings);
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
    }
}
