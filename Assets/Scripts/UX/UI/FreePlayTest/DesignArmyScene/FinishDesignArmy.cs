using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest
{
    public class FinishDesignArmy : MonoBehaviour
    {
        public void OnClick()
        {
            if (ArmyDesign.Ins.TrySave())
            {
                UIManager.Ins.Load(CanvasID.RegionalEffect);
                RegionalManagerUI.Ins.Load();
            }
        }
    }
}