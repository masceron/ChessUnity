using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Carapace : Effect, IBeforePieceActionTrigger, IAfterPieceActionTrigger
    {
        public Carapace(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_carapace")
        {
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece || action.Result != ResultFlag.Blocked) return;

            ActionManager.EnqueueAction(new RemoveEffect(this));
            if (MatchManager.Ins.Roll(25)) ActionManager.EnqueueAction(new CarapaceKill(Piece, action.GetMakerAsPiece()));
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Mitigation;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece || action.Result != ResultFlag.Success
                || (action.Flag & ActionFlag.Unblockable) != 0) return;

            action.Result = ResultFlag.Parry;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 80;
        }
    }
}