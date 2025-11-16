using UnityEngine;
using UX.UI.Army.DesignArmy;
using UX.UI.FreePlayTest.RegionalRealmScene;

namespace UX.UI.FreePlayTest.DesignArmyScene
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