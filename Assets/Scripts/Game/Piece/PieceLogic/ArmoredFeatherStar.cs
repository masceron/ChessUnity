using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class ArmoredFeatherStar : Commons.PieceLogic
    {
        public ArmoredFeatherStar(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
        }
    }
}