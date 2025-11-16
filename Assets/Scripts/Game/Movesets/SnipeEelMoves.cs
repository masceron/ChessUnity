using System.Collections.Generic;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class SnipeEelMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var piece = PieceOn(pos);
            var (rank, file) = RankFileOf(pos);

            for (var nFile = file - 1; nFile <= file + 1; ++nFile)
            {
                var nRank = rank - 2;
                if (!MakeMove(IndexOf(nRank, nFile))) break;
            }
            
            for (var nFile = file - 1; nFile <= file + 1; ++nFile)
            {
                var nRank = rank + 2;
                if (!MakeMove(IndexOf(nRank, nFile))) break;
            }
            
            for (var nRank = rank - 1; nRank <= rank + 1; ++nRank)
            {
                var nFile = file + 2;
                if (!MakeMove(IndexOf(nRank, nFile))) break;
            }
            
            for (var nRank = rank - 1; nRank <= rank + 1; ++nRank)
            {
                var nFile = file - 2;
                if (!MakeMove(IndexOf(nRank, nFile))) break;
            }

            return;
            
            bool MakeMove(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p != null)
                {
                    return false;
                }

                list.Add(new NormalMove(pos, index));
                return true;
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            
        }
        
    }
}