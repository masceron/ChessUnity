using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MethaneCasing : RelicLogic
    {
        public MethaneCasing(RelicConfig cfg) : base(cfg)
        {
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