using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CovetLensPassive : Effect, IAttackRangeModifier, IMoveRangeModifier, IApplyEffect
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