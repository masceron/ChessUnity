using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class FrogFish : Commons.PieceLogic
    {
        public FrogFish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Adaptation(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrogFishPassive(this)));
        }
    }
}