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

                    if (PieceOn(idx) != null) continue;

                    if (offset == 2)
                    {
                        int mid = IndexOf(startRank + 1 * push, startFile);
                        if (PieceOn(mid) != null) continue;
                    }

                    list.Add(new NormalMove(pos, idx));
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
