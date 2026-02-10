
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    public class RibbonEelPendingForChooseMove : PendingAction, ISkills
    {
        private int sourcePiecePos;
        private int targetPiecePos;
        
        public RibbonEelPendingForChooseMove(int maker, int sourcePiece, int targetPiece) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
            sourcePiecePos = sourcePiece;
            targetPiecePos = targetPiece;
        }
        
        public override void CompleteAction()
        {
            TileManager.Ins.UnmarkAll();
            BoardViewer.ListOf.Clear();
            var newAction = new RibbonEelActive(Maker, sourcePiecePos, targetPiecePos);
            BoardViewer.Ins.ExecuteAction(newAction);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}