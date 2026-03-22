using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class TigerPrawnPassive : Condition.Nocturnal, IMoveRangeModifierTrigger, IAttackRangeModifier
    {
        public TigerPrawnPassive(PieceLogic piece) : base(-1, 1, piece, "effect_tiger_prawn_passive")
        {
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (IsActive) baseRange += Strength;
            return baseRange;
        }

        public int ModifyMoveRange(int baseRange)
        {
            if (IsActive) baseRange += Strength;
            return baseRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}