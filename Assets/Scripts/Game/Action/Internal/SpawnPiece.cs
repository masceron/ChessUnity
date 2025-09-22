using Game.Managers;
using Game.Piece;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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