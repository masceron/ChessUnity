using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Shield: Effect
    {
        public Shield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, "effect_shield")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ResultFlag.Success || (action.Flag & ActionFlag.Unblockable) != 0) return;
            
            action.Result = ResultFlag.Blocked;

            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}