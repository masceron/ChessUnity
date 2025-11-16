using System.Collections.Generic;
using static Game.Common.BoardUtils;
namespace Game.Movesets
{
    public class SmallPredatorMoves : BaseMovePattern
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateSmallPredatorMovesPattern(makerPos);
        }

        private List<int> GenerateSmallPredatorMovesPattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            int push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file));

            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file - 1));

            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));

            return positions;
        }

        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new SmallPredatorMoves().GenerateSmallPredatorMovesPattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false);
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new SmallPredatorMoves().GenerateSmallPredatorMovesPattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true);
        }
    }
}