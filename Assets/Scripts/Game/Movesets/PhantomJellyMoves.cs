using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PhantomJellyMoves //Tentacle moves
    {
        private static readonly int[] Dr = { -1, -2, -2, -1, 1, 2, 2, 1 };
        private static readonly int[] Dc = { -2, -1, 1, 2, 2, 1, -1, -2 };

        public static int Quiets(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var range = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                MakeMove(rankOff, fileOff);

            foreach (var rankOffset in Dr)
            foreach (var fileOffset in Dc)
            {
                var rankOff = rank + rankOffset;
                var fileOff = file + fileOffset;
                MakeMove(rankOff, fileOff);
            }

            return 30 + 5 * range;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                if (piece == null && IsActive(index))
                    list.Add(index);
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var range = caller.GetAttackRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                MakeCapture(rankOff, fileOff);

            foreach (var rankOffset in Dr)
            foreach (var fileOffset in Dc)
            {
                var rankOff = rank + rankOffset;
                var fileOff = file + fileOffset;
                MakeCapture(rankOff, fileOff);
            }

            return 30 + 5 * range;

            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                if (Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1) return;
                list.Add(index); // trả tất cả ô reachable
            }
        }
    }
}