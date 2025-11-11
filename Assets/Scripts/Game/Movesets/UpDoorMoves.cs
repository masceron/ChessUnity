using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    /// <summary>
    /// *****x*****
    /// ****xxx****
    /// ****xOx****
    /// *****x*****
    /// O = position of the piece
    /// x = possible move and capture
    /// </summary>
    public static class UpDoorMoves
    {
        /*        public static void AddUpDoorPatternMoves(List<Action.Action> list, int pos, int range, bool forCapture)
                {
                    var caller = PieceOn(pos);
                    var color = caller.Color;
                    var (rank, file) = RankFileOf(pos);
                    int push = color ? +1 : -1;

                    for (int dr = -range; dr <= range + 1; dr++)
                    {
                        for (int df = -range; df <= range; df++)
                        {
                            if (dr == range + 1 && df != 0)
                                continue;

                            int newRank = rank + dr * push;
                            int newFile = file + df * (color ? 1 : -1);

                            if (!VerifyBounds(newRank) || !VerifyBounds(newFile))
                                continue;

                            if (dr == 0 && df == 0)
                                continue;

                            if (dr == -range && System.Math.Abs(df) != 0)
                                continue;

                            if (System.Math.Abs(df) > range || System.Math.Abs(dr) > range + 1)
                                continue;

                            int idx = IndexOf(newRank, newFile);
                            if (!IsActive(idx)) continue;

                            var blocker = Pathfinder.LineBlocker(rank, file, newRank, newFile);
                            bool isClear = blocker.Item1 == -1 && blocker.Item2 == -1;

                            var target = PieceOn(idx);

                            if (target == null)
                            {
                                if (!forCapture && isClear)
                                    list.Add(new NormalMove(pos, idx));
                            }
                            else if (target.Color != color)
                            {
                                if (forCapture && isClear)
                                    list.Add(new NormalCapture(pos, idx));
                            }
                        }
                    }
                }*/

        public static void AddUpDoorPatternMoves(List<Action.Action> list, int pos, int range, bool forCapture)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var (rank, file) = RankFileOf(pos);
            int push = color ? +1 : -1;

            var basePositions = new HashSet<int>(GenerateUpDoorPattern(pos));

            if (range > 1)
            {
                var toAdd = new List<int>();

                foreach (var targetPos in basePositions)
                {
                    var (tRank, tFile) = RankFileOf(targetPos);

                    int dr = tRank - rank;
                    int df = tFile - file;

                    for (int step = 1; step <= range - 1; step++)
                    {
                        int newRank = rank + dr + step * (dr / System.Math.Max(1, System.Math.Abs(dr)));
                        int newFile = file + df + step * (df / System.Math.Max(1, System.Math.Abs(df)));

                        if (dr == 0) newRank = rank;
                        if (df == 0) newFile = file;

                        if (!VerifyBounds(newRank) || !VerifyBounds(newFile))
                            continue;

                        int newIdx = IndexOf(newRank, newFile);
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
                if (!VerifyBounds(tRank) || !VerifyBounds(tFile))
                    continue;

                var blocker = Pathfinder.LineBlocker(rank, file, tRank, tFile);
                bool isClear = blocker.Item1 == -1 && blocker.Item2 == -1;
                var target = PieceOn(targetPos);

                if (target == null)
                {
                    if (!forCapture && isClear)
                        list.Add(new NormalMove(pos, targetPos));
                }
                else if (target.Color != color)
                {
                    if (forCapture && isClear)
                        list.Add(new NormalCapture(pos, targetPos));
                }
            }
        }


        private static List<int> GenerateUpDoorPattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            int push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 0, file - 1));
            positions.Add(IndexOf(rank + push * 0, file + 1));
            positions.Add(IndexOf(rank + push * -1, file));

            return positions;
        }


        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            AddUpDoorPatternMoves(list, pos, moveRange, forCapture: false);
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            AddUpDoorPatternMoves(list, pos, attackRange, forCapture: true);
        }


    }
}
