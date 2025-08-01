using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Surpass: Effect
    {
        public Surpass(PieceLogic piece) : base(-1, 1, piece, EffectName.Surpass)
        {}
    }
}