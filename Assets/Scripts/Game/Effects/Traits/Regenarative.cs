using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Regenarative : Effect, IEndTurnTrigger
    {
        private int CurrentTurn;
        private const int TurnToRemoveEffect = 2;
        public Regenarative(PieceLogic piece) : base(-1, -1, piece, "effect_regenarative")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
            CurrentTurn = 0;
        }
        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != action.Target) return;
            var piece = PieceOn(Piece.Pos);

            var bleeding = (Effect)null;
            if (piece.Effects.Any(Effects => Effects.EffectName == "effect_bleeding"))
            {
                bleeding = piece.Effects.First(e => e.EffectName == "effect_bleeding");
            }
            if (bleeding == null) return;

            CurrentTurn++;
            if (CurrentTurn >= TurnToRemoveEffect)
            {
                ActionManager.EnqueueAction(new RemoveEffect(bleeding));
                CurrentTurn = 0;
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 40;
        }
    }
}