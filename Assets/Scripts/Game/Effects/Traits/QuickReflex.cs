using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class QuickReflex: Effect
    {
        public QuickReflex(PieceLogic piece, EffectName name) : base(-1, 1, piece, EffectName.QuickReflex)
        {}
    }
}