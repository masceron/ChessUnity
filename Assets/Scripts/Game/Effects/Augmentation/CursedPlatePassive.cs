using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CursedPlatePassive : Effect, IMoveRangeModifierTrigger, IAttackRangeModifier, IOnApplyTrigger
    {
        private const int moveRangeModifier = 2;
        private const int attackRangeModifier = 2;

        public CursedPlatePassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_plate_passive")
        {
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (baseRange >= attackRangeModifier)
                return baseRange - attackRangeModifier;
            return 0;
        }

        public int ModifyMoveRange(int baseRange)
        {
            if (baseRange >= moveRangeModifier)
                return baseRange - moveRangeModifier;
            return 0;
        }

        public void OnApply()
        {
        }
    }
}