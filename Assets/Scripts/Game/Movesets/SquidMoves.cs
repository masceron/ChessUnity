using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class SquidMoves : BaseMovePattern
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateSquidMovePattern(makerPos);
        }

        private List<int> GenerateSquidMovePattern(int makerPos)
        {
            var positions = new List<int>();
            var (rank, file) = RankFileOf(makerPos);
            var push = PieceOn(makerPos).Color ? +1 : -1;

            positions.Add(IndexOf(rank + 1 * push, file));
            positions.Add(IndexOf(rank, file + 1 * push));
            positions.Add(IndexOf(rank, file - 1 * push));

            positions.Add(IndexOf(rank - 1 * push, file - 2));
            positions.Add(IndexOf(rank - 1 * push, file - 1));
            positions.Add(IndexOf(rank - 1 * push, file));
            positions.Add(IndexOf(rank - 1 * push, file + 1));
            positions.Add(IndexOf(rank - 1 * push, file + 2));
            
            positions.Add(IndexOf(rank - 2 * push, file));

            return positions;
        }

        public static int Quiets(List<int> list, int pos)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new SquidMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false);
            return 10 + 5 * moveRange;
        }

        public static int Captures(List<int> list, int pos)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new SquidMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true);
            return 10 + 5 * attackRange;
        }
    }
}