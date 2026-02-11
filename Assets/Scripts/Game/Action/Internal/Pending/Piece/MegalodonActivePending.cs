using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Common;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActivePending: PendingAction, IDisposable, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -40;
            return 0;
        }

        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;

        public MegalodonActivePending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (_firstTarget == null)
            {
                _firstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), PieceOn(Maker).AttackRange()))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color == _firstTarget.Color) continue;
                    var newAction = new MegalodonActivePending(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            _secondTarget = hovering;
            if (_firstTarget == null || _secondTarget == null) return;
            if (_firstTarget == _secondTarget) return;
            CommitResult(new MegalodonActive(Maker, _firstTarget.Pos, _secondTarget.Pos));
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
        
        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }

        
    }
}
