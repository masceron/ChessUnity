using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece) : base(-1, -1, piece, EffectName.SnappingStrike)
        {}
    }
}