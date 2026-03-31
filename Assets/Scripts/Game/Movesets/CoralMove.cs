using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CoralMove : BaseMovePattern // Jumping Move
    {

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateCoralMovePattern(makerPos);
        }

        private List<int> GenerateCoralMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));

            
            positions.Add(IndexOf(rank + push * 1, file + 2));
            positions.Add(IndexOf(rank - push * 1, file + 2));
            positions.Add(IndexOf(rank + push * 1, file - 2));
            positions.Add(IndexOf(rank - push * 1, file - 2));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new CoralMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false, excludeEmptyTile, true);

            return 15 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new CoralMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true, excludeEmptyTile, true);
            return 15 + 5 * attackRange;
        }
    }
}