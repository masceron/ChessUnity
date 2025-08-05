using Game.Data.Pieces;
using Game.Managers;

namespace Game.Action.Internal
{
    public class SpawnPiece: Action, IInternal
    {
        private readonly PieceConfig pieceToSpawn;
        public SpawnPiece(PieceConfig p) : base(-1)
        {
            pieceToSpawn = p;
        }

        protected override void Animate()
        {
            PieceManager.Ins.SpawnPiece(pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.SpawnPiece(pieceToSpawn);
        }
    }
}