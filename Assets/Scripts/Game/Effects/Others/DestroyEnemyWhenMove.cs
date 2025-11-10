using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic;
using Game.Action;

namespace Game.Effects.Others
{

    /// <summary>
    /// When this piece moves, destroy an enemy piece within a certain range.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyEnemyWhenMove : Effect
    {
        private int radius;
        public DestroyEnemyWhenMove(PieceLogic piece, int radius) : base(-1, 1, piece, EffectName.DestroyEnemyWhenMove)
        {
            this.radius = radius;
        }

        override public void OnCallPieceAction(Game.Action.Action action)
        {
            List<Action.Action> actionsToAdd = new List<Action.Action>();

            if (action is NormalMove move && BoardUtils.PieceOn(move.Maker) == Piece)
            {
                var targetPos = move.Target;
                var (rank, file) = BoardUtils.RankFileOf(targetPos);

                var enemyPieces = BoardUtils.GetPiecesInRadius(
                    rank,
                    file,
                    radius,
                    p => p != null && p.Color != Piece.Color && p != Piece
                );

                foreach (var target in enemyPieces)
                {
                    ActionManager.EnqueueAction(new KillPiece(target.Pos));
                }
            }
        }
    }
}
