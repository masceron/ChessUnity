using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Linq;
using UnityEngine;
using Game.Effects.Buffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using Game.Piece.PieceLogic;
using NUnit.Framework;
using System.Collections.Generic;
using Game.Action.Quiets;

namespace Game.Action.Skills
{
    public class Diurnal : Action, ISkills, IPendingAble
    {
        private PieceLogic SelectedPiece;
        private int MovePosTo;
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public Diurnal(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }
        void IPendingAble.CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            var actions = new List<Action>();

            if (SelectedPiece == null || SelectedPiece.Color == hovering.Color)
            {
                SelectedPiece = hovering;
                TileManager.Ins.Select(SelectedPiece.Pos);
                
                for (int i = -1; i <= 1; i++)
                {
                    if (!VerifyBounds(RankOf(SelectedPiece.Pos) + i)) continue;
                    for (int j = -1; j <= 1; j++)
                    {
                        if (!VerifyBounds(FileOf(SelectedPiece.Pos) + j))
                            continue;

                        int newRank = RankOf(SelectedPiece.Pos) + i;
                        int newFile = FileOf(SelectedPiece.Pos) + j;
                        if (PieceOn(IndexOf(newRank, newFile)) == null)
                        {
                            TileManager.Ins.MarkAsMoveable(IndexOf(newRank, newFile));
                        }
                    }
                }

                if (hovering != null) return;

                var move = new NormalMove(SelectedPiece.Pos, BoardViewer.HoveringPos);

                BoardViewer.Selecting = -1;
                BoardViewer.SelectingFunction = 0;

                BoardViewer.Ins.ExecuteAction(move);

                
            }
        }

        protected override void ModifyGameState()
        {
            var Pieces = FindPiece<PieceLogic>(PieceOn(Maker).Color);
            var picked = Pieces
                .OrderBy(_ => Random.value)
                .Take(Mathf.Min(3, Pieces.Count))
                .ToList();
            foreach (var piece in picked)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Rally(1, piece)));
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}