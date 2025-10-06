using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Effect
    {
        public ChamberedNautilusHunger(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, EffectName.ChamberedNautilusHunger)
        {
            
        }
        
        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed) return;
            
            action.Result = ActionResult.Failed;
            
            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }
    }
}