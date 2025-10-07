using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public static class PhantomJellyMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
        
        }
    
        public static void Captures(List<Action.Action> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);

            var color = caller.Color;
            //var attackRange = caller.AttackRange;

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                MakeCapture(rankOff, fileOff);
            }
            
            int[] dr = { -1, -2, -2, -1, 1, 2, 2, 1 };
            int[] dc = { -2, -1, 1, 2, 2, 1, -1, -2 };

            foreach (var rankOffset in dr)
            {
                foreach (var fileOffset in dc)
                {
                    var rankOff = rank + rankOffset;
                    var fileOff = file + fileOffset;
                    
                    MakeCapture(rankOff, fileOff);
                }
            }

            return;
            
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                
                if (piece == null || 
                    piece.Color == color || 
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalCapture(pos, index));
            }
        }
    }
    
    
}