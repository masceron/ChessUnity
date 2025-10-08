using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class EpauletteSharkActive : Action, ISkills
    {
        public EpauletteSharkActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);
            var swarmTargets = new List<int>(); // lưu vị trí các mục tiêu hợp lệ

            for (var rankOff = rank - 3; rankOff <= rank + 3; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 3; fileOff <= file + 3; fileOff++)
                {
                    if (!VerifyBounds(fileOff)) continue;
                    if (rankOff == rank && fileOff == file) continue;

                    var piecePos = IndexOf(rankOff, fileOff);
                    var p = PieceOn(piecePos);
                    if (p == null || p.Color == caller.Color || p.PieceRank != PieceRank.Swarm) 
                        continue;

                    swarmTargets.Add(piecePos);
                }
            }

            if (swarmTargets.Count == 0) return;
            // Ngược lại: chọn ngẫu nhiên 1 quân địch
            var randomIndex = UnityEngine.Random.Range(0, swarmTargets.Count);
            var target = swarmTargets[randomIndex];

            // Thêm hành động áp dụng hiệu ứng Bound (1 lượt)
            ActionManager.EnqueueAction(new KillPiece(target));

            // Đặt thời gian hồi chiêu
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}