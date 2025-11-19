using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class QuickReflex: Effect
    {
        public QuickReflex(PieceLogic piece) : base(-1, 1, piece, "effect_quick_reflex")
        {}
    }
}