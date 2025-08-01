using Game.Board.Piece;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Internal
{
    public class SpawnPiece: Action, IInternal
    {
        private readonly PieceConfig pieceToSpawn;
        public SpawnPiece(PieceConfig p) : base(-1, false)
        {
            pieceToSpawn = p;
        }

        protected override void Animate()
        {
            pieceManager.SpawnPiece(pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            gameState.SpawnPiece(pieceToSpawn);
        }
    }
}