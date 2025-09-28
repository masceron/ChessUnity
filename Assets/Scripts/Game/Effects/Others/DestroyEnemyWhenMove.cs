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
        private int range;
        public DestroyEnemyWhenMove(PieceLogic piece, int range) : base(-1, 1, piece, EffectName.DestroyEnemyWhenMove)
        {
            this.range = range;
        }

        override public void OnCall(Game.Action.Action action)
        {
            List<Action.Action> actionsToAdd = new List<Action.Action>();

            if (action is NormalMove move && BoardUtils.PieceOn(move.Maker) == Piece)
            {
                var targetPos = move.Target;
                var (rank, file) = BoardUtils.RankFileOf(targetPos);

                var enemyPieces = BoardUtils.GetPiecesInRange(
                    rank,
                    file,
                    range,
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
