using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Shield : Effect, IBeforePieceActionTrigger
    {
        public Shield(PieceLogic piece, int stack = 1) : base(-1, stack, piece, "effect_shield")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece || action.Result != ResultFlag.Success ||
                (action.Flag & ActionFlag.Unblockable) != 0) return;

            action.Result = ResultFlag.Blocked;

            if (Strength > 1) Strength--;
            else
                ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}