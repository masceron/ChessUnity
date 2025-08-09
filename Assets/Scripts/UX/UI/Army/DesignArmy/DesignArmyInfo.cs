using TMPro;
using UnityEngine;

namespace UX.UI.Army.DesignArmy
{
    public class DesignArmyInfo: MonoBehaviour
    {
        [SerializeField] private TMP_Text boardSize;

        public void Load(int size)
        {
            boardSize.text = $"{size} x {size}";
        }
    }
}