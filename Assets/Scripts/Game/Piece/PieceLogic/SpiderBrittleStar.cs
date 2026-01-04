using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class SpiderBrittleStar : Commons.PieceLogic
    {
        public SpiderBrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Consume(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 15, this)));
        }
    }
}