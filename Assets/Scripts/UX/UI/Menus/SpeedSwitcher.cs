using TMPro;
using UnityEngine;

namespace UX.UI.Menus
{
    public class SpeedSwitcher : MonoBehaviour
    {
        [SerializeField] private TMP_Text delayTMP;
        private readonly float[] values = { 0.25f, 0.5f, 1f };
        private int index;

        public void NextValue()
        {
            index = (index + 1) % values.Length;
            SetValue(index);
        }

        public void SetValue(int index)
        {
            SettingPanel.AIvsAIplayspeed = values[index];
            PlayerPrefs.SetInt("AIvsAIplayspeed", index);
            delayTMP.text = values[index].ToString();
        }
    }
}