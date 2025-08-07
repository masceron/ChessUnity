using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece, sbyte duration = -1) : base(duration, -1, piece, EffectName.SnappingStrike)
        {}
    }
}