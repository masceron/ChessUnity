using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActivePending : PendingAction, IDisposable, ISkills
    {
        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;

        public MegalodonActivePending(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker() as PieceLogic;
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -40;
            return 0;
        }

        protected override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (_firstTarget == null)
            {
                _firstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(GetFrom()), FileOf(GetFrom()),
                             ((PieceLogic)GetMaker()).AttackRange()))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color == _firstTarget.Color) continue;
                    var newAction = new MegalodonActivePending(GetMaker() as PieceLogic, piece);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }

                return;
            }

            _secondTarget = hovering;
            if (_firstTarget == null || _secondTarget == null) return;
            if (_firstTarget == _secondTarget) return;
            CommitResult(new MegalodonActive(GetFrom(), _firstTarget.Pos, _secondTarget.Pos));
        }

        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }

        private static void ResetTargets()
        {
            _firstTarget = null;
            _secondTarget = null;
        }
    }
}