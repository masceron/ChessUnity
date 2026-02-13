using TMPro;
using UnityEngine;

namespace UX.UI.FreePlayTest
{
    public class SideToggle : MonoBehaviour
    {
        public TMP_Text tmp;

        public void OnToggle()
        {
            // FreePlayArmyDesign.Ins.ToggleChosenSide();
            // tmp.text = !FreePlayArmyDesign.Ins.choosenSide ? "Ally" : "Enemy";
        }
    }
}