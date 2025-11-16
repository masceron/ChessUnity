using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    public class Haste: Effect, IMoveRangeModifier
    {
        public Haste(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_haste")
        {}

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }
    }
}