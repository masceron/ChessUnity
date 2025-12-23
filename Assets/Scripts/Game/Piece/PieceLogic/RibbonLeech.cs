using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class RibbonLeech : Commons.PieceLogic
    {
        public RibbonLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KnightMoves.Captures)
        {
            //ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));   
        }
    }
}