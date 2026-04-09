using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinningMoves : BaseMovePattern
    {
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

        public static int Quiets(List<int> list, int pos)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new SpinningMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false);
            return 20 + 5 * moveRange;
        }

        public static int Captures(List<int> list, int pos)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new SpinningMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true);
            return 20 + 5 * attackRange;
        }
    }
}