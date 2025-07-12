using Game.Board.General;

namespace Game.Board.Effects
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMarked: Effect
    {
        public VelkarisMarked(PieceLogic.PieceLogic piece) : base(-1, 1, piece, ObserverType.None, 0)
        {
        }
    }
}