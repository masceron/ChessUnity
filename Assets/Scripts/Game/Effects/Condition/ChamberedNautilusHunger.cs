using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;

namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Effect
    {
        public ChamberedNautilusHunger(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, EffectName.ChamberedNautilusHunger)
        {
            
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Result != ActionResult.Succeed) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece)));
        }
    }
}