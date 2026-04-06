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
            int totalForwardRows = 2;
    
            for (int i = 1; i <= totalForwardRows; i++)
            {
                int forwardRank = rank + push * i;
                
                positions.Add(IndexOf(forwardRank, file));
                
                for (int j = 1; j <= i; j++)
                {
                    positions.Add(IndexOf(forwardRank, file - j));
                    positions.Add(IndexOf(forwardRank, file + j));
                }
            }

            positions.Add(IndexOf(rank - push * 1, file));

            return positions;
        }
        

        public static int Quiets(List<int> list, int pos)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new WormMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false);

            return 10 + 5 * moveRange;
        }

        public static int Captures(List<int> list, int pos) 
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new WormMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true);
            return 10 + 5 * attackRange;
        }

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateWormMovePattern(makerPos);
        }
    }
}