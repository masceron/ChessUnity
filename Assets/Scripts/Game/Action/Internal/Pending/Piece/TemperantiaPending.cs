using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaPending : PendingAction, IDisposable
    {
        private static int _ally = -1;
        private static int _enemy = -1; // -1 nếu chưa chọn enemy
        private readonly PieceLogic _temperantia;

        public TemperantiaPending(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
            _temperantia = PieceOn(Maker);
        }

        public void Dispose()
        {
            _ally = -1;
            _enemy = -1;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            if (_ally == -1 || _enemy == -1)
            {
                if (PieceOn(Target).Color == _temperantia.Color)
                {
                    _ally = Target;
                    foreach (var pending in BoardViewer.ListOf.Where(pending =>
                                 PieceOn(pending.Target).Color == _temperantia.Color))
                        TileManager.Ins.UnMark(pending.Target);
                }
                else
                {
                    _enemy = Target;
                    foreach (var pending in BoardViewer.ListOf.Where(pending =>
                                 PieceOn(pending.Target).Color != _temperantia.Color))
                        TileManager.Ins.UnMark(pending.Target);
                }
            }
            else
            {
                CommitResult(new TemperantiaSwap(Maker, _ally, _enemy));
                _ally = -1;
                _enemy = -1;
            }
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}