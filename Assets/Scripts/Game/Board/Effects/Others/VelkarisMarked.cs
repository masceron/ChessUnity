using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMarked: Effect
    {
        public VelkarisMarked(PieceLogic piece) : base(-1, 1, piece, EffectType.VelkarisMarked)
        {}
    }
}