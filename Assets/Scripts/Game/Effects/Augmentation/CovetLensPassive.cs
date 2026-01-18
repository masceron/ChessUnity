using System.Linq;
using Game.Action.Internal;
using Game.Augmentation;
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
            PieceLogic pieceApplied = applyEffect.Effect.Piece;
            if (pieceApplied.Augmentations.Any(aug => aug.Name == AugmentationName.CovetLens))
            {
                Effect effect = applyEffect.Effect;
                if (effect.EffectName == "effect_haste" || effect.EffectName == "effect_long_reach")
                {
                    effect.Strength = covetLensLevel;
                    effect.Duration = covetLensLevel;
                }
            }
        }
    }
}