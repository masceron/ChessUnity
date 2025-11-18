using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearl: RelicLogic
    {
        public CommonPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Color != Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                        var pending = new CommonPearlPending(this, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                } 
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}