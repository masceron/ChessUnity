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
        private readonly PieceConfig _pieceToSpawn;
        private readonly System.Action<PieceLogic>[] _onSpawned;

        public SpawnPiece(PieceConfig p) : base(null, p.Index)
        {
            _pieceToSpawn = p;
        }
        
        public SpawnPiece(PieceConfig p, params System.Action<PieceLogic>[] onSpawned)
            : base(null, p.Index)
        {
            _pieceToSpawn = p;
            _onSpawned = onSpawned;
        }

        protected override void Animate()
        {
            PieceManager.Ins.SpawnPiece(_pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            var spawnedPiece = SpawnPiece(_pieceToSpawn);
            
            if (_onSpawned == null) return;
            foreach (var callback in _onSpawned)
            {
                callback?.Invoke(spawnedPiece);
            }
        }
    }
}