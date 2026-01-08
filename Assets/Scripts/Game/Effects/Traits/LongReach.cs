using System.Linq;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits{
    public class LongReach : Effect, IAttackRangeModifier, IOnApply
    {
        private const int covetLensLevel = 3;
        public LongReach(PieceLogic piece, sbyte strength, sbyte duration = 1) : base(duration, strength, piece, "effect_long_reach")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + Strength * 15 + Duration * 5;
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (Piece.Augmentations.Any(aug => aug.Name == AugmentationName.CovetLens))
            {
                Strength = covetLensLevel;
            }
            return baseRange + Strength;
        }
        
        public void OnApply()
        {
            if (Piece.Augmentations.Any(aug => aug.Name == AugmentationName.CovetLens))
            {
                Strength = covetLensLevel;
                Duration = covetLensLevel;
            }
        }
    }
}

