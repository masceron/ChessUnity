using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece, sbyte duration = -1) : base(duration, -1, piece, EffectName.SnappingStrike)
        {}
    }
}