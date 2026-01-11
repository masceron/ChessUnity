using System.Linq;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    public class Haste: Effect, IMoveRangeModifier, IOnApply
    {
        private const int covetLensLevel = 3;
        public Haste(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_haste")
        {}
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + Strength * 15 + Duration * 5;
        }
        
        public int ModifyMoveRange(int baseRange)
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