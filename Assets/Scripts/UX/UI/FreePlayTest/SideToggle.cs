using TMPro;
using UX.UI.Army.DesignArmy;
using UnityEngine;

namespace UX.UI.FreePlayTest
{
    public class SideToggle : MonoBehaviour
    {
        public TMP_Text tmp;
        public void OnToggle()
        {
            ArmyDesign.Ins.ToggleChosenSide();
            tmp.text = (ArmyDesign.Ins.choosenSide == false) ? "Ally" : "Enemy";
        }
    }
}