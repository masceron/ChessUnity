using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.Augmentation
{
    public class DuelistReflexPassive : Effect, IAfterPieceActionTrigger, IBeforePieceActionTrigger
    {
        public DuelistReflexPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_duelist_reflex_passive")
        {
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetTarget() != Piece) return;
            if (action.Result != ResultFlag.Parry) return;

            ActionManager.EnqueueAction(new KillPiece(action.Maker));
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Reaction;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetTarget() != Piece) return;
            if (action.Result != ResultFlag.Success) return;
            if ((action.Flag & ActionFlag.Unblockable) != 0) return;

            var probability = Random.Range(0, 101);
            if (probability >= 50) action.Result = ResultFlag.Parry;
        }
    }
}