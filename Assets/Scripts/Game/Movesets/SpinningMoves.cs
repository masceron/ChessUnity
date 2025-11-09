using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;
namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinningMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange(ref index);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, moveRange))
            {
                TryAdd(rankOff, fileOff);
            }
            
            TryAdd(rank + moveRange + 1, file + moveRange + 1);
            TryAdd(rank + moveRange + 1, file - moveRange - 1);
            TryAdd(rank - moveRange - 1, file + moveRange + 1);
            TryAdd(rank - moveRange - 1, file - moveRange - 1);

            // --- helper function ---
            void TryAdd(int r, int f)
            {
                if (!VerifyBounds(r) || !VerifyBounds(f)) return;
                int idx = IndexOf(r, f);
                if (!IsActive(idx) || PieceOn(idx) != null) return;

                // Không đi xuyên tường
                if (Pathfinder.LineBlocker(rank, file, r, f).Item1 != -1) return;

                list.Add(new NormalMove(pos, idx));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var attackRange = piece.AttackRange;
            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
            {
                TryCapture(rankOff, fileOff);
            }

            TryCapture(rank + attackRange + 1, file + attackRange + 1);
            TryCapture(rank + attackRange + 1, file - attackRange - 1);
            TryCapture(rank - attackRange - 1, file + attackRange + 1);
            TryCapture(rank - attackRange - 1, file - attackRange - 1);

            // --- helper function ---
            void TryCapture(int r, int f)
            {
                if (!VerifyBounds(r) || !VerifyBounds(f)) return;
                int idx = IndexOf(r, f);
                if (!IsActive(idx)) return;

                var target = PieceOn(idx);
                if (target == null) return;

                // Không đánh xuyên tường
                if (Pathfinder.LineBlocker(rank, file, r, f).Item1 != -1) return;

                if (target.Color != color)
                {
                    list.Add(new NormalCapture(pos, idx));
                }
            }
        }
    }
}