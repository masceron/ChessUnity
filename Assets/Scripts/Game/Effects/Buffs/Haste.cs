using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    public class Haste: Effect, IMoveRangeModifierTrigger
    {
        public Haste(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_haste")
        {}

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + Strength * 15 + Duration * 5;
        }
    }
}