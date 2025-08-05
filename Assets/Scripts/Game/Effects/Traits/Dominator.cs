using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, EffectName.Dominator)
        {}
    }
}