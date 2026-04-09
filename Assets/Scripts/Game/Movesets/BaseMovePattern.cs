using System.Collections.Generic;
using Game.Common;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public abstract class BaseMovePattern
    {
        public abstract List<int> GenerateBaseMovePattern(int makerPos);

        /// <summary>
        /// Từ basePositions, lọc ra các vị trí hợp lệ (bounds, active, không bị chặn)
        /// và thêm vào list positions.
        /// forCapture = false → chỉ trả ô trống (Quiets)
        /// forCapture = true  → trả tất cả ô reachable (cả trống lẫn có quân)
        /// </summary>
        public static void AddToPatternMoves(List<int> list, HashSet<int> basePositions, int pos, int range,
            bool forCapture, bool isSurpass = false)
        {
            if (range <= 0) return;

            var caller = PieceOn(pos);
            var (rank, file) = RankFileOf(pos);

            if (range > 1)
            {
                var toAdd = new List<int>();

                foreach (var targetPos in basePositions)
                {
                    var (tRank, tFile) = RankFileOf(targetPos);

                    var dr = tRank - rank;
                    var df = tFile - file;

                    for (var step = 1; step <= range - 1; step++)
                    {
                        var stepRank = dr == 0 ? 0 : step * (dr / Mathf.Abs(dr));
                        var stepFile = df == 0 ? 0 : step * (df / Mathf.Abs(df));

                        var newRank = rank + dr + stepRank;
                        var newFile = file + df + stepFile;

                        if (!VerifyBounds(newRank) || !VerifyBounds(newFile))
                            continue;

                        var newIdx = IndexOf(newRank, newFile);
                        if (!basePositions.Contains(newIdx) && !toAdd.Contains(newIdx))
                            toAdd.Add(newIdx);
                    }
                }

                foreach (var add in toAdd)
                    basePositions.Add(add);
            }

            foreach (var targetPos in basePositions)
            {
                var (tRank, tFile) = RankFileOf(targetPos);
                if (!VerifyBounds(tRank) || !VerifyBounds(tFile) || !IsActive(targetPos))
                    continue;

                var isClear = true;
                if (!isSurpass)
                {
                    var blocker = Pathfinder.LineBlocker(rank, file, tRank, tFile);
                    isClear = blocker.Item1 == -1 && blocker.Item2 == -1;
                }

                if (!isClear) continue;

                var target = PieceOn(targetPos);

                if (!forCapture)
                {
                    // Quiets: chỉ trả ô trống
                    if (target == null)
                        list.Add(targetPos);
                }
                else
                {
                    // Captures: trả tất cả ô reachable (cả trống lẫn có quân)
                    list.Add(targetPos);
                }
            }
        }
    }
}