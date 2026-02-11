using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Skills;
using ZLinq;

// <-- thêm để dùng LINQ

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaPending : PendingAction, System.IDisposable
    {
        private static int _ally = -1;
        private static int _enemy = -1; // -1 nếu chưa chọn enemy
        private readonly PieceLogic _temperantia;
        public TemperantiaPending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
            _temperantia = PieceOn(Maker);
        }

        protected override void CompleteAction()
        {
            if (_ally == -1 || _enemy == -1)
            {
                if (PieceOn(Target).Color == _temperantia.Color)
                {
                    _ally = Target;
                    foreach (var pending in BoardViewer.ListOf.Where(pending => PieceOn(pending.Target).Color == _temperantia.Color))
                    {
                        TileManager.Ins.UnMark(pending.Target);
                    }
                }
                else
                {
                    _enemy = Target;
                    foreach (var pending in BoardViewer.ListOf.Where(pending => PieceOn(pending.Target).Color != _temperantia.Color))
                    {
                        TileManager.Ins.UnMark(pending.Target);
                    }
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
        public void Dispose()
        {
            _ally = -1;
            _enemy = -1;
            BoardViewer.SelectingFunction = 0;
        }
    }
}