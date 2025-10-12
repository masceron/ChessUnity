using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class Construct : Effect
    {
        public Construct(PieceLogic piece) : base(-1, 1, piece, EffectName.Construct)
        {
        }
    }
}