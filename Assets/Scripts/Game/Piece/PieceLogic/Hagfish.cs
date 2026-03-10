using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class Hagfish : Commons.PieceLogic
    {
        public Hagfish(PieceConfig cfg) : base(cfg, WormMoves.Quiets, WormMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlimeCoat(this)));
        }
    }
}