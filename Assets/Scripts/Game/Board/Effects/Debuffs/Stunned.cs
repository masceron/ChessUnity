using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Stunned: Effect
    {
        public Stunned(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectType.Stunned)
        {}
    }
}