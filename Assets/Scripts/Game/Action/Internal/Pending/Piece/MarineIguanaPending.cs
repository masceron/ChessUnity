using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    public class MarineIguanaPending : PendingAction, IDisposable, ISkills
    {
        private static PieceLogic _firstTarget;
        public static PieceLogic SecondTarget;

        public MarineIguanaPending(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        public void Dispose()
        {
            _firstTarget = null;
            SecondTarget = null;
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return pieceAI.Color != BoardUtils.PieceOn(Maker).Color ? -50 : 0;
        }

        protected override void CompleteAction()
        {
            if (_firstTarget == null)
            {
                _firstTarget = BoardUtils.PieceOn(Target);
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(_firstTarget.Pos),
                             BoardUtils.FileOf(_firstTarget.Pos), 2))
                {
                    var index = BoardUtils.IndexOf(rankOff, fileOff);
                    var piece = BoardUtils.PieceOn(index);
                    if (piece == null || piece.Color != _firstTarget.Color || piece == _firstTarget) continue;
                    var newAction = new MarineIguanaPending(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
            }
            else
            {
                SecondTarget = BoardUtils.PieceOn(Target);

                var kill = new MarinelKill(Maker, _firstTarget.Pos, SecondTarget.Pos);
                CommitResult(kill);

                BoardViewer.Ins.Unmark();
            }
        }
    }
}