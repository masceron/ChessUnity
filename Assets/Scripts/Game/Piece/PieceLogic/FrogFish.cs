using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class FrogFish : Commons.PieceLogic
    {
        public FrogFish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(this)));   
            ActionManager.EnqueueAction(new ApplyEffect(new FrogFishPassive(this)));   
        }
        
    }
}