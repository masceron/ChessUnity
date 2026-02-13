using System;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class MaskedPufferPassive : Effect, IAfterPieceActionTrigger
    {
        public MaskedPufferPassive(PieceLogic piece) : base(-1, -1, piece, "effect_masked_puffer_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            if (action.Maker == Piece.Pos) return;
            if (action.Target != Piece.Pos) return;

            if (action.Result != ResultFlag.Success) ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
        }

        public override int GetValueForAI()
        {
            throw new NotImplementedException();
        }
    }
}