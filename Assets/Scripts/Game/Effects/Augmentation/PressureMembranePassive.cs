using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class PressureMembranePassive : Effect, IMoveRangeModifierTrigger, IBeforeApplyEffectTrigger
    {
        private const int moveRangeModifier = 2;

        public PressureMembranePassive(PieceLogic piece) : base(-1, 1, piece, "effect_pressure_membrane_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var pieceApplied = applyEffect.Effect.Piece;

            if (pieceApplied != Piece) return;

            if (applyEffect.Effect.EffectName == "effect_shortreach") applyEffect.Result = ResultFlag.EffectResistance;
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + moveRangeModifier;
        }
    }
}