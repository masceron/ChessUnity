using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class WormMoves : BaseMovePattern
    {
        private List<int> GenerateWormMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();

            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            int range = caller.GetMoveRange();

            // đã có sẵn 2 tầng khi range = 1
            int totalForwardRows = range + 1;
    
            for (int i = 1; i <= totalForwardRows; i++)
            {
                int forwardRank = rank + push * i;

                // ô thẳng
                positions.Add(IndexOf(forwardRank, file));

                // mở ngang theo đúng tầng
                for (int j = 1; j <= i; j++)
                {
                    positions.Add(IndexOf(forwardRank, file - j));
                    positions.Add(IndexOf(forwardRank, file + j));
                }
            }

            // lùi thẳng scale theo range
            for (int i = 1; i <= range; i++)
            {
                positions.Add(IndexOf(rank - push * i, file));
            }

            return positions;
        }
        

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new WormMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, 1, false, excludeEmptyTile);

            return 10 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new WormMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, 1, true, excludeEmptyTile);
            return 10 + 5 * attackRange;
        }

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateWormMovePattern(makerPos);
        }
    }
}