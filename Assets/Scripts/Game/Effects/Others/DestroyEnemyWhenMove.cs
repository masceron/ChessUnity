using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    /// <summary>
    ///     When this piece moves, destroy an enemy piece within a certain range.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyEnemyWhenMove : Effect, IAfterPieceActionTrigger
    {
        private readonly int radius;

        public DestroyEnemyWhenMove(PieceLogic piece, int radius) : base(-1, 1, piece, "effect_destroy_enemy_when_move")
        {
            this.radius = radius;
        }

        public AfterActionPriority Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not NormalMove move || move.GetMakerAsPiece() != Piece) return;
            var targetPos = move.GetTargetPos();
            var (rank, file) = BoardUtils.RankFileOf(targetPos);

            var enemyPieces = BoardUtils.GetPiecesInRadius(
                rank,
                file,
                radius,
                p => p != null && p.Color != Piece.Color && p != Piece
            );

            foreach (var target in enemyPieces) ActionManager.EnqueueAction(new KillPiece(Piece, target));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 130;
        }
    }
}