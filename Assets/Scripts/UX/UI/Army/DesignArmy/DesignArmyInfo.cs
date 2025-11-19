using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    public class DesignArmyInfo: MonoBehaviour
    {
        [SerializeField] private TMP_Text boardSize;
        [SerializeField] private Button boardSizeButton;
        [SerializeField] public TMP_InputField armyName;

        public void Load(int size)
        {
            boardSize.text = $"{size} x {size}";
            armyName.text = "";
            boardSizeButton.interactable = false;
        }

        public void LoadSave(string arn)
        {
            armyName.text = arn;
        }
    }
}