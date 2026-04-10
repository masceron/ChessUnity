using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActivePending : PendingAction, IDisposable, ISkills
    {
        private static int _firstTargetId = -1;
        private static int _secondTargetId = -1;

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
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -40;
            return 0;
        }

        protected override void CompleteAction()
        {

            if (_firstTargetId == -1)
            {
                _firstTargetId = Target;

                var firstTarget = GetEntityByID(_firstTargetId) as PieceLogic;
                if (firstTarget == null)
                {
                    ResetTargets();
                    return;
                }

                _secondTargetId = -1;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(GetFrom()), FileOf(GetFrom()),
                             GetMakerAsPiece().AttackRange()))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color == firstTarget.Color) continue;
                    var newAction = new MegalodonActivePending(GetMakerAsPiece(), piece);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }

                return;
            }

            _secondTargetId = Target;
            if (_firstTargetId == -1 || _secondTargetId == -1) return;
            if (_firstTargetId == _secondTargetId) return;
            CommitResult(new MegalodonActive(GetMakerAsPiece(), _firstTargetId, _secondTargetId));
        }

        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }

        private static void ResetTargets()
        {
            _firstTargetId = -1;
            _secondTargetId = -1;
        }
    }
}