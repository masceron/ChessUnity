using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;

// <-- thêm để dùng LINQ

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaPending : PendingAction, IDisposable, ISkills
    {
        private static int _ally = -1;
        private static int _enemy = -1; // -1 nếu chưa chọn enemy
        private readonly PieceLogic _temperantia;

        public TemperantiaPending(int maker, int target) : base(maker, target)
        {
            _temperantia = GetMaker() as PieceLogic;
        }

        public void Dispose()
        {
            _ally = -1;
            _enemy = -1;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            if (GetTarget().Color == _temperantia.Color)
            {
                _ally = GetTargetPos();
                foreach (var pending in BoardViewer.ListOf.Where(pending =>
                                pending.GetTarget().Color == _temperantia.Color))
                    TileManager.Ins.UnMark(pending.GetTargetPos());
            }
            else
            {
                _enemy = GetTargetPos();
                foreach (var pending in BoardViewer.ListOf.Where(pending =>
                                pending.GetTarget().Color != _temperantia.Color))
                    TileManager.Ins.UnMark(pending.GetTargetPos());
            }

            if (_ally == -1 || _enemy == -1) return;
            CommitResult(new TemperantiaSwap(GetFrom(), _ally, _enemy));
            _ally = -1;
            _enemy = -1;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}