using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeshadeSculpinPending : PendingAction, ISkills, IDisposable
    {
        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;
        
        public EyeshadeSculpinPending(int maker, int target) : base(maker)
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
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(_firstTarget.Pos), FileOf(_firstTarget.Pos), 4))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != _firstTarget.Color) continue;
                    var newAction = new EyeshadeSculpinPending(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            _secondTarget = hovering;
            CommitResult(new EyeshadeSculpinActive(Maker, _firstTarget.Pos, _secondTarget.Pos));
        }
        
        public void Dispose()
        {
            _firstTarget = null;
            _secondTarget = null;
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
    }
}