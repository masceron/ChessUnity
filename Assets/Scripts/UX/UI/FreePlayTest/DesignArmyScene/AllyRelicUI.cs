using Game.Relics;
using Game.Relics.Commons;
using UnityEngine.EventSystems;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class AllyRelicUI : ArmyDesignRelic
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Config.relicWhiteConfig = new RelicConfig(Relic.key, false, 5);
        }
    }
}