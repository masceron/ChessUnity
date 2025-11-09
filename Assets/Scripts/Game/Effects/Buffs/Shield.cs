using Game.Action;
using Game.Action.Internal;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Shield: Effect
    {
        public Shield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, EffectName.Shield)
        {}

        public override void OnCallPieceAction(Action.Action action)
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