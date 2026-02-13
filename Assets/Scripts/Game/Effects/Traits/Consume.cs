using Game.Action;
using Game.Action.Captures;
using Game.Common;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Consume : Effect, IAfterPieceActionTrigger
    {
        public Consume(PieceLogic piece) : base(-1, 1, piece, "effect_consume")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.Maker != Piece.Pos || action is not ICaptures || action.Result != ResultFlag.Success) return;
            var captured = BoardUtils.PieceOn(action.Target);

            if (captured.Effects.Any(e => e.EffectName is "effect_surpass" or "effect_vigorous")) return;

            Piece.Quiets = captured.Quiets;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}