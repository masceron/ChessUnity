using Game.Board.General;

namespace Game.Board.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Trigger: Observer
    {
        protected readonly PieceLogic.PieceLogic Piece;

        protected Trigger(PieceLogic.PieceLogic p, ObserverType type, ObserverPriority priority): base(type, priority)
        {
            Piece = p;
            EventObserver.AddObserver(this);
        }
    }
}