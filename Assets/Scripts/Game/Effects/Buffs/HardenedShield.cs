using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HardenedShield : Effect, IBeforePieceActionTrigger, IAfterPieceActionTrigger
    {
        public HardenedShield(PieceLogic piece, int stack = 1, int duration = -1) : base(duration, stack, piece, "effect_hardened_shield")
        {
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures
                || action.GetTarget() != Piece
                || action.Result != ResultFlag.Blocked
                || action.Result != ResultFlag.HardenedBlock
                || action.Result != ResultFlag.Evade) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, BoardUtils.action.GetMaker()), Piece));
            if (Strength > 1) Strength--;
            else
                ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Mitigation;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTarget() != Piece || action.Result != ResultFlag.Success ||
                (action.Flag & ActionFlag.Unblockable) != 0) return;
            action.Result = ResultFlag.HardenedBlock;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 50;
        }
    }
}