using System.Linq;
using Game.Action.Internal.Pending.Relic;
using Game.Effects;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;
using Game.Piece.PieceLogic.Commons;
using Game.Piece;
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
                // foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                // {
                //     if (piece == null || piece.Color != Color) continue;
                //     if (piece.PieceRank == PieceRank.Commander || piece.PieceRank == PieceRank.Construct) continue;
                //     TileManager.Ins.MarkAsMoveable(piece.Pos);
                //     var pending = new PeaceTreatyPending(this, piece.Pos);
                //     BoardViewer.ListOf.Add(pending);
                // }
                // thiếu marker
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}