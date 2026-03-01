using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpawnPiece : Action, IInternal
    {
        public readonly PieceConfig pieceToSpawn;
        public readonly System.Action<PieceLogic> onSpawned;

        public SpawnPiece(PieceConfig p) : base(-1)
        {
            pieceToSpawn = p;
        }
        
        public SpawnPiece(PieceConfig p, System.Action<PieceLogic> onSpawned = null) 
            : base(-1)
        {
            pieceToSpawn = p;
            this.onSpawned = onSpawned;
        }

        protected override void Animate()
        {
            PieceManager.Ins.SpawnPiece(pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.SpawnPiece(pieceToSpawn);
            var spawnedPiece = PieceOn(pieceToSpawn.Index);
            onSpawned?.Invoke(spawnedPiece);
        }
    }
}