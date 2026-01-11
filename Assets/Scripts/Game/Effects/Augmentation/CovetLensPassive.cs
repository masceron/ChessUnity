using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CovetLensPassive : Effect, IAttackRangeModifier, IMoveRangeModifier
    {
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
    }
}