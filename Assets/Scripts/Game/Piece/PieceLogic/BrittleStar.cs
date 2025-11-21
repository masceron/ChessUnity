using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class BrittleStar : Commons.PieceLogic
    {
        public BrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 15, this)));
        }
    }
}