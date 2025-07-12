using Game.Board.General;

namespace Game.Board.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Trigger: Observer
    {
        protected readonly GameState GameState;
        public readonly PieceLogic.PieceLogic Piece;

        protected Trigger(GameState gameState, PieceLogic.PieceLogic p, ObserverType type, byte priority): base(type, priority)
        {
            GameState = gameState;
            Piece = p;
            EventObserver.AddObserver(this);
        }
    }
}