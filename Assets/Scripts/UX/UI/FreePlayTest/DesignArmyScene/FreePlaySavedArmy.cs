using UnityEngine.UI;
using UnityEngine;
using Game.Save.FreePlay;
using TMPro;
using Game.Save.Army;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlaySavedArmy : MonoBehaviour
    {
        [SerializeField] private Image image;
        private FPPreset preset;
        [SerializeField] protected TMP_Text armyName;
        [SerializeField] protected TMP_Text boardSize;
        public void Load(FPPreset preset)
        {
            this.preset = preset;
            armyName.text = preset.Name;
            boardSize.text = $"{preset.BoardSize} x {preset.BoardSize}";
        }
        public void Click()
        {
            FPArmyDesign.Ins.Load(preset.BoardSize, preset);
            foreach(var savedArmy in FPSavedArmies.Ins.savedArmies)
            {
                if(savedArmy != this)
                {
                    savedArmy.RemoveHighLight();
                }
            }
            image.color = Color.white;
        }
        public void RemoveHighLight()
        {
            image.color = new Color(229 / 255.0f, 232 / 255.0f, 69 / 255.0f);
        }
        public void Delete()
        {
            FreePlaySaveLoader.Remove(preset.Name);
            Destroy(gameObject);
        }
    }
}

