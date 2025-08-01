using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Shield: Effect
    {
        public Shield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, EffectName.Shield)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.Pos || action.Result != ActionResult.Succeed) return;
            
            action.Result = ActionResult.Failed;
            
            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }
    }
}