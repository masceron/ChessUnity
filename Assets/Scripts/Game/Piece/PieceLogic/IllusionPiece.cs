using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class IllusionPiece : Commons.PieceLogic
    {
        public IllusionPiece(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Illusion(this)));
        }
    }
}