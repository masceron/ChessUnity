using System.Collections.Generic;
using Game.Action.Relics;
using Game.Relics.Commons;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructor : RelicLogic
    {
        public ChaoticConstructor(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
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