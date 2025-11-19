using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Marked : Effect
    {
        public Marked(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_marked")
        {

        }
    }
}

