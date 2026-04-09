using System.Collections.Generic;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Relics
{
    public class OvergrownSlug : RelicLogic, IRelicAction
    {
        public OvergrownSlug(RelicConfig cfg) : base(cfg)
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