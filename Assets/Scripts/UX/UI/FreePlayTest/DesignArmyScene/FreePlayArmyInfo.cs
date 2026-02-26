using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlayArmyInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text boardSize;
        [SerializeField] private Button boardSizeButton;

        public void Load(int size)
        {
            boardSize.text = $"{size} x {size}";
            boardSizeButton.interactable = false;
        }
    }
}