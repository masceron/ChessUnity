using System.Collections.Generic;
using Game.Action.Relics;
using Game.Relics.Commons;

namespace Game.Relics
{
    public class KelpBanner : RelicLogic
    {
        public KelpBanner(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }
    }
}