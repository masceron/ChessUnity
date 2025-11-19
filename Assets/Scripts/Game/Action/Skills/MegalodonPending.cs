using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using UX.UI.Ingame;

namespace Game.Action.Skills
{
    public class MegalodonPending : Action, IPendingAble, System.IDisposable
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private List<PieceLogic> availablePieces;
        private MegalodonActive _megalodonActive;
        
        public MegalodonPending(MegalodonActive m, int maker, int target) : base(maker)
        {
            _megalodonActive = m;
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.Select(FirstTarget.Pos);
                foreach (var piece in availablePieces)
                {
                    if (piece.Color == FirstTarget.Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                }

                return;
            }

            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();
            
            ActionManager.ExecuteImmediately(new KillPiece(FirstTarget.Pos));
            ActionManager.ExecuteImmediately(new KillPiece(SecondTarget.Pos));
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            
            _megalodonActive.SetCoolDown();
            
            
        }
        
        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();
            _megalodonActive = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
           
        }
    }
}