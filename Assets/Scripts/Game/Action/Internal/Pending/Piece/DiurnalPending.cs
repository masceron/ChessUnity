using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Skills;

namespace Game.Action.Internal.Pending.Piece
{
    public class DiurnalPending : PendingAction
    {
        private static PieceLogic _selectedPiece;
        private static int _movePosTo;

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public DiurnalPending(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (_selectedPiece == null)
            {
                if (hovering == null) return;

                _selectedPiece = hovering;

                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(_selectedPiece.Pos);
                BoardViewer.ListOf.Clear();

                for (var dr = -1; dr <= 1; dr++)
                {
                    var r = RankOf(_selectedPiece.Pos) + dr;
                    if (!VerifyBounds(r)) continue;

                    for (var df = -1; df <= 1; df++)
                    {
                        var f = FileOf(_selectedPiece.Pos) + df;
                        if (dr == 0 && df == 0) continue;
                        if (!VerifyBounds(f)) continue;

                        var idx = IndexOf(r, f);
                        if (PieceOn(idx) != null) continue;

                        TileManager.Ins.MarkAsMoveable(idx);
                        BoardViewer.ListOf.Add(new DiurnalPending(Maker, idx));
                    }
                }

                Debug.Log($"Diurnal select: {_selectedPiece.Type} at {_selectedPiece.Pos}");
                return;
            }

            _movePosTo = BoardViewer.HoveringPos;
            if (_movePosTo == -1 || PieceOn(_movePosTo) != null) return;

            var execute = new DiurnalActive(_selectedPiece.Pos, _movePosTo);

            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            CommitResult(execute);
            ResetTargets();
        }

        private static void ResetTargets()
        {
            _selectedPiece = null;
            _movePosTo = -1;
        }
    }
}