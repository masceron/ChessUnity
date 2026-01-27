using Game.Effects;
using Game.Action.Skills;
using Game.Relics;
using Game.Action;

namespace Assets.Scripts.Game.Effects.Traits
{
    public class SeabedLevelerPassive : Effect
    {
        private SeabedLeveler seabedLeveler;
        public SeabedLevelerPassive(SeabedLeveler sl) : base(-1, -1, null, "effect_seabed_leveler_passive")
        {
            seabedLeveler = sl;
        }

        
    }
}