using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Common;
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

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                var commander = GetCommanderOf(Color);
                if (commander != null)
                {
                    var pending = new OvergrownSlugPending(this, commander.Pos);
                    TileManager.Ins.MarkAsMoveable(commander.Pos);
                    BoardViewer.ListOf.Add(pending);
                    BoardViewer.Selecting = -2;
                    BoardViewer.SelectingFunction = 4;
                }
            }
        }
    }
}