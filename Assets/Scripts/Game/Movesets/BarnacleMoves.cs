using System.Collections.Generic;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BarnacleMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
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
            
        }
    }
}