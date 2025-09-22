using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow: Effect
    {
        public Slow(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.Slow)
        {}
    }
}