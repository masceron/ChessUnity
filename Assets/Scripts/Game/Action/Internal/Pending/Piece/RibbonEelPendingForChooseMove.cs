using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    public class RibbonEelPendingForChooseMove : PendingAction, ISkills
    {
        private readonly int sourcePiecePos;
        private readonly int targetPiecePos;

        public RibbonEelPendingForChooseMove(int maker, int sourcePiece, int targetPiece) : base(maker, targetPiece)
        {
            sourcePiecePos = sourcePiece;
            targetPiecePos = targetPiece;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void CompleteAction()
        {
            TileManager.Ins.UnmarkAll();
            BoardViewer.ListOf.Clear();
            var newAction = new RibbonEelActive(GetFrom(), sourcePiecePos, targetPiecePos);
            CommitResult(newAction);
        }
    }
}