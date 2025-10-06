using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
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
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var caller = PieceOn(pos);
            var color = caller.Color; //trắng = false, đen = true
            var (startRank, startFile) = RankFileOf(pos);
            var effectiveMoveRange = caller.GetMoveRange(ref index);
            int delta = effectiveMoveRange - 2;
            int push = color ? +1 : -1; // hướng chính (lên/xuống tùy màu)

            var basePattern = new (int height, int width)[]
            {
                (2, 1), // forward
                (1, 1), // backward
                (2, 1), // right
                (2, 1)  // left
            };

            basePattern[0].height = System.Math.Max(1, basePattern[0].height + delta);
            basePattern[1].height = System.Math.Max(0, basePattern[1].height + delta);
            basePattern[2].height = System.Math.Max(1, basePattern[2].height + delta);
            basePattern[2].width = System.Math.Max(0, basePattern[2].width + delta);
            basePattern[3].height = System.Math.Max(1, basePattern[3].height + delta);
            basePattern[3].width = System.Math.Max(0, basePattern[3].width + delta);

            void TryAdd(int rank, int file)
            {
                if (!VerifyBounds(rank) || !VerifyBounds(file)) return;
                int idx = IndexOf(rank, file);
                if (!IsActive(idx)) return;
                if (PieceOn(idx) != null) return;
                list.Add(new NormalMove(pos, idx));
            }

            //forward
            for (int i = 1; i <= basePattern[0].height; i++)
            {
                var newRank = startRank + i * push;
                TryAdd(newRank, startFile);
            }

            //backward
            for (int i = 1; i <= basePattern[1].height; i++)
            {
                var newRank = startRank - i * push;
                TryAdd(newRank, startFile);
            }

            //right
            for (int i = 0; i < basePattern[2].height; i++)
            {
                for (int j = 1; j <= basePattern[2].width; j++)
                {
                    var newRank = startRank + i * push;
                    var newFile = startFile + j * (color ? 1 : -1);
                    TryAdd(newRank, newFile);
                }
            }

            //left
            for (int i = 0; i < basePattern[3].height; i++)
            {
                for (int j = 1; j <= basePattern[3].width; j++)
                {
                    var newRank = startRank + i * push;
                    var newFile = startFile - j * (color ? 1 : -1);
                    TryAdd(newRank, newFile);
                }
            }
        }


        public static void Captures(List<Action.Action> list, int pos)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var (startRank, startFile) = RankFileOf(pos);

            int push = !color ? -1 : +1;

            for (int offset = -1; offset <= 2; offset++)
            {
                int rank = startRank + offset * push;
                if (!VerifyBounds(rank)) continue;

                int fileStart = (offset == 2) ? startFile : startFile - 1;
                int fileEnd = (offset == 2) ? startFile : startFile + 1;

                for (int file = fileStart; file <= fileEnd; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    if (rank == startRank && file == startFile) continue;

                    if (offset < 0 && file != startFile) continue;

                    int idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;

                    var target = PieceOn(idx);
                    if (target == null || target.Color == color) continue;

                    if (offset == 2)
                    {
                        int mid = IndexOf(startRank + 1 * push, startFile);
                        if (PieceOn(mid) != null) continue;
                    }

                    list.Add(new NormalCapture(pos, idx));
                }
            }
        }

    }
}
