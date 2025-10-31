using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Summon
{
    public class RibbonLeech : PieceLogic
    {
        public RibbonLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, HorseLeechMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));   
        }
    }
}