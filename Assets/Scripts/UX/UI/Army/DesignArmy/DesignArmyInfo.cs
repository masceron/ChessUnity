using TMPro;
using UnityEngine;

namespace UX.UI.Army.DesignArmy
{
    public class DesignArmyInfo: MonoBehaviour
    {
        [SerializeField] private TMP_Text boardSize;
        [SerializeField] public TMP_InputField armyName;

        public void Load(int size)
        {
            boardSize.text = $"{size} x {size}";
            armyName.text = "";
        }

        public void LoadSave(string arn)
        {
            armyName.text = arn;
        }
    }
}