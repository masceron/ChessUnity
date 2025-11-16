using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Bound: Effect
    {
        public Bound(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_bound")
        {}
    }
}