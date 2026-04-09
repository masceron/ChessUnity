using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLeveler : RelicLogic
    {
        private readonly Charge charge;

        public SeabedLeveler(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddEffectObserver(charge);
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }
    }
}