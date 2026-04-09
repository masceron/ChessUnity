using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class KingMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var effectiveMoveRange = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, effectiveMoveRange))
                MakeMove(rankOff, fileOff);

            return 10 + 10 * effectiveMoveRange;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(index);
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var attackRange = caller.GetAttackRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
                MakeCapture(rankOff, fileOff);

            return 10 + 10 * attackRange;

            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                if (Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1) return;
                list.Add(index); // trả tất cả ô reachable (kể cả trống)
            }
        }
    }
}