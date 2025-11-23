using TMPro;
using UX.UI.Army.DesignArmy;
using UnityEngine;
using UX.UI.FreePlayTest.DesignArmyScene;

namespace UX.UI.FreePlayTest
{
    public class SideToggle : MonoBehaviour
    {
        public TMP_Text tmp;
        public void OnToggle()
        {
            FreePlayArmyDesign.Ins.ToggleChosenSide();
            tmp.text = (FreePlayArmyDesign.Ins.choosenSide == false) ? "Ally" : "Enemy";
        }
    }
}