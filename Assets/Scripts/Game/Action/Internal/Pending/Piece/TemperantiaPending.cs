using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Skills;

// <-- thêm để dùng LINQ

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaPending : PendingAction, System.IDisposable
    {
        public static int ally = -1;
        public static int enemy = -1; // -1 nếu chưa chọn enemy
        private PieceLogic temperantia;
        public TemperantiaPending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
            temperantia = PieceOn(Maker);
        }

        public override void CompleteAction()
        {
            if (ally == -1 || enemy == -1)
            {
                if (PieceOn(Target).Color == temperantia.Color)
                {
                    ally = Target;
                    foreach(var pending in BoardViewer.ListOf)
                    {
                        if (PieceOn(pending.Target).Color == temperantia.Color)
                        {
                            TileManager.Ins.UnMark(pending.Target);
                        }
                    }
                }
                else
                {
                    enemy = Target;
                    foreach(var pending in BoardViewer.ListOf)
                    {
                        if (PieceOn(pending.Target).Color != temperantia.Color)
                        {
                            TileManager.Ins.UnMark(pending.Target);
                        }
                    }
                }
            }
            else
            {
                BoardViewer.Ins.ExecuteAction(new TemperantiaSwap(Maker, ally, enemy));
                ally = -1;
                enemy = -1;
            }
        }
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public void Dispose()
        {
            ally = -1;
            enemy = -1;
            BoardViewer.SelectingFunction = 0;
        }
    }
}