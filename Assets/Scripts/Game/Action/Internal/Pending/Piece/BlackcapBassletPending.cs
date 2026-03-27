using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    public class BlackcapBassletPending : PendingAction, ISkills
    {
        
        public BlackcapBassletPending(int maker, int target) : base(maker, target)
        {
        }

        protected override void CompleteAction()
        {
            TileManager.Ins.UnmarkAll();
            BoardViewer.ListOf.Clear();
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(GetTargetPos()),
                         FileOf(GetTargetPos()), 1))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn != null) continue;
                var newAction = new BlackcapBassletActive(GetMakerPos(), index);
                BoardViewer.ListOf.Add(newAction);
                TileManager.Ins.MarkAsMoveable(index);
            }
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
        
    }
}