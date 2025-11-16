using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class Construct : Effect
    {
        public Construct(PieceLogic piece) : base(-1, 1, piece, "effect_construct")
        {
        }
    }
}