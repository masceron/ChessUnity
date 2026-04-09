using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class MorayEelMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var effectiveMoveRange = piece.GetMoveRange();

            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= effectiveMoveRange; rankOff--)
                if (!MakeStraightQuiets(IndexOf(rankOff, file)))
                    break;

            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= effectiveMoveRange; rankOff++)
                if (!MakeStraightQuiets(IndexOf(rankOff, file)))
                    break;

            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= effectiveMoveRange; fileOff--)
                if (!MakeStraightQuiets(IndexOf(rank, fileOff)))
                    break;

            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= effectiveMoveRange; fileOff++)
                if (!MakeStraightQuiets(IndexOf(rank, fileOff)))
                    break;

            if (effectiveMoveRange > 1)
            {
                var rankTo = rank - effectiveMoveRange;
                if (rankTo >= 0)
                    for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                    {
                        if (!VerifyBounds(tFileTo)) continue;
                        var idx = IndexOf(rankTo, tFileTo);

                        if (!IsActive(idx) || PieceOn(idx) != null) continue;
                        if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;

                        list.Add(idx);
                    }

                rankTo = rank + effectiveMoveRange;
                if (VerifyUpperBound(rankTo))
                    for (var tFileTo = file - effectiveMoveRange + 1; tFileTo <= file + effectiveMoveRange - 1; tFileTo++)
                    {
                        if (!VerifyBounds(tFileTo)) continue;
                        var idx = IndexOf(rankTo, tFileTo);

                        if (!IsActive(idx) || PieceOn(idx) != null) continue;
                        if (Pathfinder.LineBlocker(rank, file, rankTo, tFileTo).Item1 != -1) continue;

                        list.Add(idx);
                    }

                var fileTo = file - effectiveMoveRange;
                if (fileTo >= 0)
                    for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                    {
                        if (!VerifyBounds(tRankTo)) continue;
                        var idx = IndexOf(tRankTo, fileTo);

                        if (!IsActive(idx) || PieceOn(idx) != null) continue;
                        if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;

                        list.Add(idx);
                    }

                fileTo = file + effectiveMoveRange;
                if (VerifyUpperBound(fileTo))
                    for (var tRankTo = rank - effectiveMoveRange + 1; tRankTo <= rank + effectiveMoveRange - 1; tRankTo++)
                    {
                        if (!VerifyBounds(tRankTo)) continue;
                        var idx = IndexOf(tRankTo, fileTo);

                        if (!IsActive(idx) || PieceOn(idx) != null) continue;
                        if (Pathfinder.LineBlocker(rank, file, tRankTo, fileTo).Item1 != -1) continue;

                        list.Add(idx);
                    }
            }

            return 20 + 5 * effectiveMoveRange;

            bool MakeStraightQuiets(int index)
            {
                if (!IsActive(index) || PieceOn(index) != null) return false;
                list.Add(index);
                return true;
            }
        }
    }
}