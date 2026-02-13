using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Shortreach : Effect, IAttackRangeModifier
    {
        public Shortreach(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_shortreach")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - (Strength * 15 + Duration * 5);
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange - Strength;
        }
    }
}