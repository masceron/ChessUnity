using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Piece
{
    public class DiurnalPending : PendingAction
    {
        private static PieceLogic SelectedPiece;
        private static int MovePosTo;

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public DiurnalPending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (SelectedPiece == null)
            {
                if (hovering == null) return;

                SelectedPiece = hovering;

                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(SelectedPiece.Pos);
                BoardViewer.ListOf.Clear();

                for (var dr = -1; dr <= 1; dr++)
                {
                    var r = RankOf(SelectedPiece.Pos) + dr;
                    if (!VerifyBounds(r)) continue;

                    for (var df = -1; df <= 1; df++)
                    {
                        var f = FileOf(SelectedPiece.Pos) + df;
                        if (dr == 0 && df == 0) continue;
                        if (!VerifyBounds(f)) continue;

                        var idx = IndexOf(r, f);
                        if (PieceOn(idx) != null) continue;

                        TileManager.Ins.MarkAsMoveable(idx);
                        BoardViewer.ListOf.Add(new DiurnalPending(Maker, idx));
                    }
                }

                Debug.Log($"Diurnal select: {SelectedPiece.Type} at {SelectedPiece.Pos}");
                return;
            }

            MovePosTo = BoardViewer.HoveringPos;
            if (MovePosTo == -1 || PieceOn(MovePosTo) != null) return;

            var execute = new DiurnalActive(SelectedPiece.Pos, MovePosTo);

            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            BoardViewer.Ins.ExecuteAction(execute);
            ResetTargets();
        }

        private static void ResetTargets()
        {
            SelectedPiece = null;
            MovePosTo = -1;
        }
    }
}