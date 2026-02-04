using Game.Effects;
using Game.Relics;

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