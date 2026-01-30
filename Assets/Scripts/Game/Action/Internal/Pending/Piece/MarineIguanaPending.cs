using Game.Action.Internal.Pending;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    public class MarineIguanaPending : PendingAction, System.IDisposable, ISkills
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        public MarineIguanaPending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return pieceAI.Color != BoardUtils.PieceOn(Maker).Color ? -50 : 0;
        }

        public override void CompleteAction()
        {
            if (FirstTarget == null)
            {
                FirstTarget = BoardUtils.PieceOn(Target);
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(FirstTarget.Pos), BoardUtils.FileOf(FirstTarget.Pos), 2))
                {
                    var index = BoardUtils.IndexOf(rankOff, fileOff);
                    var piece = BoardUtils.PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color || piece == FirstTarget) continue;
                    var newAction = new MarineIguanaPending(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            } else
            {
                SecondTarget = BoardUtils.PieceOn(Target);
                
                var kill = new MarinelKill(Maker, FirstTarget.Pos, SecondTarget.Pos);
                BoardViewer.Ins.ExecuteAction(kill);

                BoardViewer.Ins.Unmark();
            }
        }

        public void Dispose()
        {
            FirstTarget = null;
            SecondTarget = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
