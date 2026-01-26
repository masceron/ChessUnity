using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class FireGobyFish : Commons.PieceLogic
    {
        public FireGobyFish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FireGobyFishPassive(this)));
        }
    }
}