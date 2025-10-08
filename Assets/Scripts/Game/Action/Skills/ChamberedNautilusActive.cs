using System.Collections.Generic;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilusActive : Action, ISkills
    {
        public ChamberedNautilusActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }
        
        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            // Danh sách lưu các quân địch trong phạm vi 2 ô
            List<PieceLogic> enemiesInRange = new List<PieceLogic>();

            // Quét vùng 5x5 quanh caller
            for (var rankOff = rank - 2; rankOff <= rank + 2; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 2; fileOff <= file + 2; fileOff++)
                {
                    if (!VerifyBounds(fileOff)) continue;
                    if (rankOff == rank && fileOff == file) continue;

                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color == caller.Color) continue;

                    enemiesInRange.Add(p);
                }
            }

            // Nếu không có quân địch nào trong phạm vi, thoát ra (không kích hoạt skill)
            if (enemiesInRange.Count == 0)
                return;

            // Ngược lại: chọn ngẫu nhiên 1 quân địch
            var randomIndex = UnityEngine.Random.Range(0, enemiesInRange.Count);
            var target = enemiesInRange[randomIndex];

            // Thêm hành động áp dụng hiệu ứng Bound (1 lượt)
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, target)));

            // Đặt hồi chiêu kỹ năng (chỉ khi skill thực sự kích hoạt)
            SetCooldown(Maker, ((IPieceWithSkill)caller).TimeToCooldown);
        }
    }
}