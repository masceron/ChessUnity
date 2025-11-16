using UnityEngine;

namespace UX.UI.FreePlayTest
{
    public class LoadCanvasByFunc : MonoBehaviour
    {
        public void LoadFreePlayDesignArmy()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
            FreePlayTest.Ins.Load();
        }
    }

}
