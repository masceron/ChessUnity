using Game.ScriptableObjects;
using UX.UI.Army.DesignArmy;
using UnityEngine;
using UX.UI.Tooltip;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPArmyRelic : ArmyDesignRelic
    {
        [SerializeField] private TooltipTrigger tooltipTrigger;
        public override void Load(RelicInfo relicInfo)
        {
            base.Load(relicInfo);
            var relicName = Localizer.GetText("relic_", relicInfo.key, null);
            var description = Localizer.GetText("relic_", relicInfo.key + "_description", null);
            tooltipTrigger.SetText(relicName, "", description);
        }
    }
}