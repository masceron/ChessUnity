using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.AI;
using Game.Action.Skills;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasPending : PendingAction, System.IDisposable, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -20;
            return 0;
        }
        public static int FirstTarget;
        public static int SecondTarget;

        private bool isExecuting;
        public HumilitasPending(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            isExecuting = false;
        }

        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            UnityEngine.Debug.Log("Executing HumilitasPending" + FirstTarget);
            if (FirstTarget == 0)
            {

                FirstTarget = BoardViewer.HoveringPos;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 5);
                foreach (var piece in listPieces)
                {
                    if (piece == FirstTarget) continue;
                    var newAction = new HumilitasPending(Maker, piece);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(piece);
                }
                return;
            }
            SecondTarget = BoardViewer.HoveringPos;
            BoardViewer.Ins.ExecuteAction(new HumilitasActive(Maker, FirstTarget, SecondTarget));
            isExecuting = true;
        }
        public void Dispose()
        {
            if (!isExecuting) return;
            FirstTarget = -1;
            SecondTarget = -1;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
