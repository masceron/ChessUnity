using UX.UI.Army.DesignArmy;
using UnityEngine;
using TMPro;
using Game.Save.Relics;
using Game.Managers;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPRelicSearcher: ArmyRelicSearcher
    {
        private bool chosenSide = false;
        [SerializeField] private TMP_Text enemyRelicText;
        public void LoadEnemyRelic(Relic? relic)
        {
            enemyRelicText.text = !relic.HasValue ? Localizer.GetText("game", "relic", null)
                : Localizer.GetText("relic_name", AssetManager.Ins.RelicData[relic.Value.Type].key, null);
        }
        public void ToggleWithSide(bool side)
        {
            Toggle();
            chosenSide = side;
        }
        public override void SelectRelic()
        {
            FPArmyDesign.Ins.SelectRelic(selecting, chosenSide);
            if (chosenSide)
            {
                enemyRelicText.text = description.nameText.text;
            }
            else
            {
                relicText.text = description.nameText.text;
            }
            Toggle();
        }
    }
}