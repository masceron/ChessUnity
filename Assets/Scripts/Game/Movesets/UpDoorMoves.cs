using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class UpDoorMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var (startRank, startFile) = RankFileOf(pos);

            // Trắng đi lên (rank giảm), Đen đi xuống (rank tăng)
            int push = !color ? -1 : +1;

            // offset -1 = lùi, 0 = đứng yên, 1 = tiến 1, 2 = tiến 2
            for (int offset = -1; offset <= 2; offset++)
            {
                int rank = startRank + offset * push;
                if (!VerifyBounds(rank)) continue;

                // Đi 2 ô chỉ thẳng file, không duyệt sang ngang
                int fileStart = (offset == 2) ? startFile : startFile - 1;
                int fileEnd = (offset == 2) ? startFile : startFile + 1;

                for (int file = fileStart; file <= fileEnd; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    if (rank == startRank && file == startFile) continue; // bỏ chính mình

                    // Không cho lùi chéo
                    if (offset < 0 && file != startFile) continue;

                    int idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;

                    // Nếu có quân thì không thể đi
                    if (PieceOn(idx) != null) continue;

                    // Nếu đi 2 ô thẳng, kiểm tra ô trung gian
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

            // Ăn chỉ xét 1 ô lùi hoặc 1 ô tiến
            for (int offset = -1; offset <= 1; offset++)
            {
                int rank = startRank + offset * push;
                if (!VerifyBounds(rank)) continue;

                for (int file = startFile - 1; file <= startFile + 1; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    if (rank == startRank && file == startFile) continue;

                    // Không cho lùi chéo
                    if (offset < 0 && file != startFile) continue;

                    int idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;

                    var target = PieceOn(idx);
                    if (target == null || target.Color == color) continue;

                    list.Add(new NormalCapture(pos, idx));
                }
            }
        }
    }
}
