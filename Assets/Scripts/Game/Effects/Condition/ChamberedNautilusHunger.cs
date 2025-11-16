using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Effect
    {
        public ChamberedNautilusHunger(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, "effect_chambered_nautilus_hunger")
        {
            
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action is not { Result: ActionResult.Succeed }) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece)));
        }
    }
}