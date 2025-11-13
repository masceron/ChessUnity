using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    /// <summary>
    /// *****x*****
    /// ****xxx****
    /// ****xOx****
    /// *****x*****
    /// O = position of the piece
    /// x = possible move and capture
    /// </summary>
    public class UpDoorMoves: BaseMovePattern
    {
        private static List<int> GenerateUpDoorPattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            int push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 0, file - 1));
            positions.Add(IndexOf(rank + push * 0, file + 1));
            positions.Add(IndexOf(rank + push * -1, file));

            return positions;
        }


        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(GenerateUpDoorPattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false);
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(GenerateUpDoorPattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true);
        }

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateUpDoorPattern(makerPos);
        }
    }
}
