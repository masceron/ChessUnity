using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    public class BlackPearl : RelicLogic, IRelicAction
    {
        public BlackPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown != 0) return;
            
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                TileManager.Ins.MarkAsMoveable(piece.Pos);
                var pending = new BlackPearlPending(this, piece.Pos);
                BoardViewer.ListOf.Add(pending);
            }
            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }

        public override void ActiveForAI()
        {}
    }
}