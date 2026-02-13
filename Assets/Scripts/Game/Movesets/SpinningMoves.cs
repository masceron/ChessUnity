using System.Collections.Generic;
using static Game.Common.BoardUtils;
namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinningMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos)
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
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateSpinningMovePattern(makerPos);
        }

        private List<int> GenerateSpinningMovePattern(int makerPos)
        {
            PieceOn(makerPos);
            var positions = new List<int>();
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + 2, file + 2));
            positions.Add(IndexOf(rank + 2, file - 2));

            positions.Add(IndexOf(rank + 1, file + 1));
            positions.Add(IndexOf(rank + 1, file));
            positions.Add(IndexOf(rank + 1, file - 1));

            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));

            positions.Add(IndexOf(rank - 1, file + 1));
            positions.Add(IndexOf(rank - 1, file));
            positions.Add(IndexOf(rank - 1, file - 1));

            positions.Add(IndexOf(rank - 2, file + 2));
            positions.Add(IndexOf(rank - 2, file - 2));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new SpinningMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, excludeEmptyTile: excludeEmptyTile);
            return 20 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new SpinningMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, excludeEmptyTile: excludeEmptyTile);
            return 20 + 5 * attackRange;
        }
    }
}