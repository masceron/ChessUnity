using System.Collections.Generic;
using Game.Relics.Commons;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStone : RelicLogic
    {
        public LedgerStone(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }
        
        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }

        public override void ActiveForAI()
        {
        }
    }
}