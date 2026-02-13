using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CovetLensPassive : Effect, IAttackRangeModifier, IMoveRangeModifierTrigger, IBeforeApplyEffectTrigger
    {
        private const int covetLensLevel = 3;
        public CovetLensPassive(PieceLogic piece) : base(-1, 1, piece, "effect_covet_lens_passive")
        {
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange - 1;
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange - 1;
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var pieceApplied = applyEffect.Effect.Piece;

            if (pieceApplied != Piece) return;
            
            var effect = applyEffect.Effect;
            if (effect.EffectName == "effect_haste" || effect.EffectName == "effect_long_reach")
            {
                effect.Strength = covetLensLevel;
            }
        }
    }
}