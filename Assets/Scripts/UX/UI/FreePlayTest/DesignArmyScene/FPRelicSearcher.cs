using Game.Managers;
using Game.Save.Relics;
using TMPro;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPRelicSearcher : ArmyRelicSearcher
    {
        [SerializeField] private TMP_Text enemyRelicText;
        private bool chosenSide;

        public void LoadEnemyRelic(Relic? relic)
        {
            enemyRelicText.text = !relic.HasValue
                ? Localizer.GetText("game", "relic", null)
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
                enemyRelicText.text = description.nameText.text;
            else
                relicText.text = description.nameText.text;
            Toggle();
        }
    }
}