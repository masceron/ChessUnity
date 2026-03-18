using System.Collections.Generic;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.SpecialAbility
{
    public class PaintedGreenlingPassive : Effect, IAfterPieceActionTrigger
    {
        public PaintedGreenlingPassive(PieceLogic piece) : base(-1, 1, piece, "effect_painted_greenling_passive")
        {
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            if (action.Maker != Piece.Pos) return;
        }
    }
}