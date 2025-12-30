using Game.Managers;
using Game.Piece;
using Game.Effects;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class SpawnPieceWithEffect : Action, IInternal
    {
        private readonly PieceConfig pieceToSpawn;
        private readonly Effect effectToApply;
        
        public SpawnPieceWithEffect(PieceConfig p, Effect effect) : base(-1)
        {
            pieceToSpawn = p;
            effectToApply = effect;
        }

        protected override void Animate()
        {
            PieceManager.Ins.SpawnPiece(pieceToSpawn);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.SpawnPiece(pieceToSpawn);

            var spawnedPiece = PieceOn(pieceToSpawn.Index);
            
            if (spawnedPiece != null && effectToApply != null)
            {
                effectToApply.Piece = spawnedPiece;
                ActionManager.EnqueueAction(new ApplyEffect(effectToApply));
            }
        }
    }
}