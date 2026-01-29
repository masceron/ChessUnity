using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Quiets;

namespace Game.Action.Skills
{
    public class Diurnal : Action, ISkills, IPendingAble
    {
        private static PieceLogic SelectedPiece;
        private static int MovePosTo;

        public int AIPenaltyValue(PieceLogic pieceAI) => 0;

        public Diurnal(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        void IPendingAble.CompleteAction()
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
                        BoardViewer.ListOf.Add(new Diurnal(Maker, idx));
                    }
                }

                Debug.Log($"Diurnal select: {SelectedPiece.Type} at {SelectedPiece.Pos}");
                return;
            }

            MovePosTo = BoardViewer.HoveringPos;
            if (MovePosTo == -1 || PieceOn(MovePosTo) != null) return;

            var move = new NormalMove(SelectedPiece.Pos, MovePosTo);

            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            BoardViewer.Ins.ExecuteAction(move);
            ResetTargets();
        }

        private static void ResetTargets()
        {
            SelectedPiece = null;
            MovePosTo = -1;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}