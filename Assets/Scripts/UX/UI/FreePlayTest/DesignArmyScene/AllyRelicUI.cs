using Game.Relics;
using UX.UI.Army.DesignArmy;
using UnityEngine.EventSystems;

namespace UI.UX.FreePlayTest
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