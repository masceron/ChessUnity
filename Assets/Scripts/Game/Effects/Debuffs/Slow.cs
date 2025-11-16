using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow: Effect, IMoveRangeModifier
    {
        public int ModifyMoveRange(int baseRange)
        {
            return baseRange - Strength;
        }
    
        public Slow(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_slow")
        {}
    }
}