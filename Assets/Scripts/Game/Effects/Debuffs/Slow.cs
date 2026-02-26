using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow : Effect, IMoveRangeModifierTrigger
    {
        public Slow(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_slow")
        {
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange - Strength;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - Strength * 15 - Duration * 5;
        }
    }
}