using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Attached</b> (gắn lên <b>Piece</b>)<br/>
    ///     Quân cờ này đang chứa 1 quân có State <see cref="StateType.Adhesive"/>.<br/>
    ///     - Nhận hiệu ứng từ quân Adhesive gây ra (theo mô tả Skill).<br/>
    ///     - Khi bị ăn/phá hủy (<see cref="IDeadTrigger"/>): quân Adhesive được spawn ra
    ///       vị trí ngẫu nhiên còn trống xung quanh vị trí đó.<br/><br/>
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Attached : StateEffect, IDeadTrigger
    {
        /// <summary>Tham chiếu đến quân Adhesive đang bám.</summary>
        public PieceLogic AdhesivePiece;

        public override StateType StateType => StateType.Attached;

        /// <summary>Constructor khi Adhesive bám vào một Piece.</summary>
        /// <param name="piece">Host piece (nhận state Attached).</param>
        /// <param name="adhesivePiece">Quân Adhesive đang bám.</param>
        public Attached(PieceLogic piece, PieceLogic adhesivePiece)
            : base(-1, 0, piece, "effect_attached")
        {
            AdhesivePiece = adhesivePiece;
        }

        /// <summary>
        ///     Khi Piece host bị ăn/phá hủy: spawn quân Adhesive ra vị trí ngẫu nhiên
        ///     còn trống xung quanh vị trí vừa chết.
        /// </summary>
        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie != Piece) return;

            SpawnAdhesiveAround(pieceToDie.Pos, Piece);
        }

        internal static void SpawnAdhesiveAround(int hostPos, PieceLogic hostLogic, PieceLogic adhesivePiece)
        {
            var availablePos = new List<int>();

            foreach (var (rank, file) in MoveEnumerators.AroundUntil(
                BoardUtils.RankOf(hostPos),
                BoardUtils.FileOf(hostPos), 1))
            {
                var idx = BoardUtils.IndexOf(rank, file);
                if (!BoardUtils.VerifyIndex(idx)) continue;
                if (BoardUtils.PieceOn(idx) != null) continue;
                if (!BoardUtils.IsActive(idx)) continue;
                availablePos.Add(idx);
            }

            if (availablePos.Count == 0)
            {
                // hostLogic == null khi host là Formation: truyền hostPos làm formationPos
                ActionManager.EnqueueAction(new DestroyAdhesivePiece(
                    adhesivePiece, hostLogic, formationPos: hostLogic == null ? hostPos : -1));
            }
            else
            {
                var randomPos = availablePos[UnityEngine.Random.Range(0, availablePos.Count)];
                ActionManager.EnqueueAction(new MoveToDetachAdhesive(hostPos, randomPos, adhesivePiece, hostLogic));
            }
        }

        private void SpawnAdhesiveAround(int hostPos, PieceLogic hostLogic)
            => SpawnAdhesiveAround(hostPos, hostLogic, AdhesivePiece);
    }
}
