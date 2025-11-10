using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Movesets;
using Game.Piece;
using Game.Piece.PieceLogic;

namespace Game.Common
{
    public class FeatherStar  : PieceLogic
    {
        public FeatherStar(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
        }
    }
}