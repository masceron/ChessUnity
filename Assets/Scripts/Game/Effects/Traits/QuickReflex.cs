using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class QuickReflex: Effect
    {
        public QuickReflex(PieceLogic piece) : base(-1, 1, piece, EffectName.QuickReflex)
        {}
    }
}