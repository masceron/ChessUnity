using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class EmptyDiamondMove : BaseMovePattern
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateEmptyDiamondMovePattern(makerPos);
        }

        private List<int> GenerateEmptyDiamondMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file));

            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank - push * 1, file + 1));
            positions.Add(IndexOf(rank - push * 1, file - 1));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file - 2));

            return positions;
        }

        public static int Quiets(List<int> list, int pos)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new EmptyDiamondMove().GenerateEmptyDiamondMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false);

            return 30 + 5 * moveRange;
        }

        public static int Captures(List<int> list, int pos)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new EmptyDiamondMove().GenerateEmptyDiamondMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true);
            return 30 + 5 * attackRange;
        }
    }
}