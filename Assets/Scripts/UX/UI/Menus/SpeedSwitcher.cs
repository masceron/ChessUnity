using UnityEngine;
using TMPro;

namespace UX.UI.Menus
{
    public class SpeedSwitcher : MonoBehaviour
    {
        float[] values = { 0.25f, 0.5f, 1f };
        private int index = 0;
        [SerializeField] private TMP_Text delayTMP;
        public void NextValue()
        {
            index = (index + 1) % values.Length;
            SettingPanel.AIvsAIplayspeed = values[index];
            PlayerPrefs.SetFloat("AIvsAIplayspeed", SettingPanel.AIvsAIplayspeed);
            
            delayTMP.text = values[index].ToString();
        }
    }

}