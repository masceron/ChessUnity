using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class MegalodonMoves
    {
        public static int Captures(List<int> list, int pos)
        {
            var caller = PieceOn(pos);
            var range = caller.GetAttackRange();
            var (startRank, startFile) = RankFileOf(pos);
            var push = !caller.Color ? -1 : 1;

            for (var rank = RankOf(pos) - (range - 1) * push; rank != RankOf(pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(pos) - range * push; file != FileOf(pos) + range * push; file += push)
                {
                    if (!VerifyBounds(file)) continue;
                    var idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;
                    if (Pathfinder.LineBlocker(startRank, startFile, rank, file).Item1 != -1) continue;

                    list.Add(idx); // trả tất cả ô reachable (kể cả trống)
                }
            }

            return 10 + 10 * range;
        }
    }
}