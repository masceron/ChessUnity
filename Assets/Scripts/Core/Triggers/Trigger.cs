namespace Core.Triggers
{
    public abstract class Trigger
    {
        protected readonly GameState GameState;
        public readonly PieceData Piece;

        protected Trigger(GameState gameState, PieceData p)
        {
            GameState = gameState;
            Piece = p;
        }

        public abstract bool CallTrigger(PieceData movedPiece, Move lastMove);
    }
}