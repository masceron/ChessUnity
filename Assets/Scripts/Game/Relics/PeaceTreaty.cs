using Game.Relics.Commons;
using UX.UI.Ingame;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Common;
using static Game.Common.BoardUtils;
namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PeaceTreaty : RelicLogic
    {
        public PeaceTreaty(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                var targets = SkillRangeHelper.GetActiveAllyPieceGlobal(-1);
                foreach (var target in targets)
                {
                    var piece = PieceOn(target);
                    if (piece == null) continue;
                    TileManager.Ins.MarkAsMoveable(target);
                    var pending = new PeaceTreatyPending(this, target);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}