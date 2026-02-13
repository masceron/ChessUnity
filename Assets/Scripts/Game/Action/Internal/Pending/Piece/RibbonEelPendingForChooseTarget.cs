using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    public class RibbonEelPendingForChooseTarget : PendingAction, ISkills
    {
        private readonly int _sourcePiecePos;
        public RibbonEelPendingForChooseTarget(int maker, int sourcePiece) : base(maker)
        {
            Maker = maker;
            Target = maker;
            _sourcePiecePos = sourcePiece;
        }

        protected override void CompleteAction()
        {
            TileManager.Ins.UnmarkAll();
            BoardViewer.ListOf.Clear();
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(Maker), BoardUtils.FileOf(Maker), 1))
            {
                var index = BoardUtils.IndexOf(rankOff, fileOff);
                var pOn = BoardUtils.PieceOn(index);
                if (pOn != null) continue;
                var newAction = new RibbonEelPendingForChooseMove(index, _sourcePiecePos, Maker);
                BoardViewer.ListOf.Add(newAction);
                TileManager.Ins.MarkAsMoveable(index);
            }
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        // public void Dispose()
        // {
        //     BoardViewer.SelectingFunction = 0;
        // }
    }
}