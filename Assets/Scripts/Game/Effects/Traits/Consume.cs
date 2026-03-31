using Game.Action;
using Game.Action.Captures;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Consume : Effect, IBeforePieceActionTrigger
    {
        public Consume(PieceLogic piece) : base(-1, 1, piece, "effect_consume")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action.Maker != Piece.Pos) return;
            if (action is not ICaptures) return;
            if (action.Result != ResultFlag.Success) return;

            var captured = BoardUtils.PieceOn(action.Target);
            if (captured == null) return;
            if (captured.Effects == null) return;

            if (captured.Effects.Any(e => e.EffectName is "effect_surpass" or "effect_vigorous"))
                return;

            Piece.Quiets = captured.Quiets;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}