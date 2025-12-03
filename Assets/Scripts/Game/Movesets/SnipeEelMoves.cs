using System.Collections.Generic;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RangerMove : BaseMovePattern
    {
        public static int Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new RangerMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false);
            return 30 + 5 * moveRange;
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new RangerMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true);
        }

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateRangerMovePattern(makerPos);
        }
        
        private List<int> GenerateRangerMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file + 1));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file - 1));

            positions.Add(IndexOf(rank + push * 1, file + 2));
            positions.Add(IndexOf(rank + push * 1, file - 2));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file - 2));

            positions.Add(IndexOf(rank - push * 1, file + 2));
            positions.Add(IndexOf(rank - push * 1, file - 2));

            positions.Add(IndexOf(rank - push * 2, file + 1));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file - 1));

            return positions;

        }
    }
}