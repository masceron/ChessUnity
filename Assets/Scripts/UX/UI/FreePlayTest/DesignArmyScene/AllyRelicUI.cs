using Game.Relics;
using UnityEngine.EventSystems;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class AllyRelicUI : ArmyDesignRelic
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Config.relicWhiteConfig = new RelicConfig(Relic.type, false, 5);
        }
    }
}