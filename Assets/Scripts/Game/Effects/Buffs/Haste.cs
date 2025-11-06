using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    public class Haste: Effect, IMoveRangeModifier
    {
        public Haste(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.Haste)
        {}

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }
    }
}