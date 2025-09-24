using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    public class KelpBass : PieceLogic
    {
        public KelpBass(PieceConfig cfg, QuietsDelegate quiets = null, CapturesDelegate captures = null) : base(cfg, BishopMoves.Quiets , KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
        }
    }
}