using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ShellfishMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange(ref index);
            var color = piece.Color;

            int[] dr = { -1, 0, 1, 0 };
            int[] dc = { 0, 1, 0, -1 };
            
            foreach ( var (rankOff, fileOff)  in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                MakeMove(rankOff, fileOff);
            }

            for (int dir = 0; dir < 4; ++dir)
            {
                var (rankOff, fileOff) = (rank + dr[dir] * moveRange, file + dc[dir] * moveRange);
                MakeMove(rankOff, fileOff);
            }

            return; 
            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalMove(pos, index));
            }
            
        }*/
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateShellfishMovePattern(makerPos);
        }

        private List<int> GenerateShellfishMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + 2, file));

            positions.Add(IndexOf(rank + 1, file + 1));
            positions.Add(IndexOf(rank + 1, file));
            positions.Add(IndexOf(rank + 1, file - 1));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));
            positions.Add(IndexOf(rank, file - 2));

            positions.Add(IndexOf(rank - 1, file + 1));
            positions.Add(IndexOf(rank - 1, file));
            positions.Add(IndexOf(rank - 1, file - 1));

            positions.Add(IndexOf(rank - 2, file));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new ShellfishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, isPlayer: isPlayer);

            return 30 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new ShellfishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, isPlayer: isPlayer);
            return 30 + 5 * attackRange;
        }
    }
}
