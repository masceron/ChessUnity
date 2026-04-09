using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingFishMoves : BaseMovePattern // Jumping Move
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateFlyingFishMovePattern(makerPos);
        }

        private List<int> GenerateFlyingFishMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file + 2));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file - 2));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file - 2));

            positions.Add(IndexOf(rank - push * 2, file + 2));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file - 2));

            return positions;
        }

        public static int Quiets(List<int> list, int pos)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new FlyingFishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false, true);

            return 15 + 5 * moveRange;
        }

        public static int Captures(List<int> list, int pos)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new FlyingFishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true, true);
            return 15 + 5 * attackRange;
        }
    }
}