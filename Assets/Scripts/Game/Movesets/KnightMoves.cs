using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class KnightMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var maxRange = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, maxRange))
                MakeMove(rankOff, fileOff);

            return 15 + 3 * maxRange;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null) return; // ô có quân → skip
                list.Add(index);
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var maxRange = caller.GetAttackRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, maxRange))
                MakeCapture(rankOff, fileOff);

            return 15 + 3 * maxRange;

            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                list.Add(index); // trả tất cả ô active trong range (kể cả trống)
            }
        }
    }
}