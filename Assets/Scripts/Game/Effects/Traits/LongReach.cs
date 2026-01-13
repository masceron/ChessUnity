using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits{
    public class LongReach : Effect, IAttackRangeModifier
    {
        public LongReach(PieceLogic piece, sbyte strength, sbyte duration = 1) : base(duration, strength, piece, "effect_long_reach")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + Strength * 15 + Duration * 5;
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + Strength;
        }
    }
}

