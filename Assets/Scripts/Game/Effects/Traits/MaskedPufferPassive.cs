using System;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    public class MaskedPufferPassive : Effect, IAfterPieceActionTrigger
    {
        public MaskedPufferPassive(PieceLogic piece) : base(-1, -1, piece, "effect_masked_pufferfish_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            if (action.GetMakerAsPiece() == Piece) return;
            if (action.GetTargetAsPiece() != Piece) return;

            if (action.Result != ResultFlag.Success) ActionManager.EnqueueAction(new Purify(Piece, Piece));
        }

        public override int GetValueForAI()
        {
            throw new NotImplementedException();
        }
    }
}