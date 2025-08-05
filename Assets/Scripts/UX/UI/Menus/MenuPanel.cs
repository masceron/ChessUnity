using UnityEngine;

namespace UX.UI.Menus
{
    public class MenuPanel : MonoBehaviour
    {
        public void OnClickPlay()
        {
            UIManager.Ins.Load(CanvasID.PlayMenu);
        }

        public void OnClickSettings()
        {
            //UIManager.UIManager.Ins.Load(CanvasID.Settings);
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
    }
}
