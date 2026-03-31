using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public abstract class BaseMovePattern
    {
        public abstract List<int> GenerateBaseMovePattern(int makerPos);

        public static void AddToPatternMoves(List<Action.Action> list, HashSet<int> basePositions, int pos, int range,
            bool forCapture, bool excludeEmptyTile, bool isSurpass = false)
        {
            if (range <= 0) return;

            var caller = PieceOn(pos);
            var color = caller.Color;
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

                var target = PieceOn(targetPos);

                if (target == null)
                {
                    if (!forCapture && isClear)
                        list.Add(new NormalMove(caller, targetPos));
                    if (forCapture && isClear && !excludeEmptyTile) // add capture để nếu đi vào đây thì sẽ bị phạt
                        list.Add(new NormalCapture(caller, target));
                }
                else if (target.Color != color)
                {
                    if (forCapture && isClear)
                        list.Add(new NormalCapture(caller, target));
                }
            }
        }
    }
}