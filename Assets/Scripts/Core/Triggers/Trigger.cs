using Core.General;
using Core.Piece;

namespace Core.Triggers
{
    public abstract class Trigger
    {
        protected readonly GameState GameState;
        public readonly PieceLogic Piece;

        protected Trigger(GameState gameState, PieceLogic p)
        {
            GameState = gameState;
            Piece = p;
        }

        public abstract bool CallTrigger();
    }
}