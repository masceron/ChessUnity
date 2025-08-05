using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece, sbyte duration = -1) : base(duration, -1, piece, EffectName.SnappingStrike)
        {}
    }
}