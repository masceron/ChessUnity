using Game.Effects;
using Game.Managers;
using Game.Piece;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class SpawnPieceWithEffect : Action, IInternal
    {
        private readonly Effect _effectToApply;
        private readonly PieceConfig _pieceToSpawn;

        public SpawnPieceWithEffect(PieceConfig p, Effect effect) : base(-1)
        {
            _pieceToSpawn = p;
            _effectToApply = effect;
        }

        protected override void Animate()
        {
            PieceManager.Ins.SpawnPiece(_pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            var spawnedPiece = SpawnPiece(_pieceToSpawn);

            if (spawnedPiece == null || _effectToApply == null) return;
            
            _effectToApply.Piece = spawnedPiece;
            ActionManager.EnqueueAction(new ApplyEffect(_effectToApply));
        }
    }
}