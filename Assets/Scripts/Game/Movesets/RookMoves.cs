using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class RookMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;

            return 15 + 5 * moveRange;

            bool MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p != null) return false; // ô có quân → stop, không thêm
                list.Add(index);
                return true; // ô trống → thêm, tiếp tục slide
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetAttackRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;

            return 15 + 5 * moveRange;

            bool MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p == null)
                {
                    list.Add(index); // ô trống → thêm, tiếp tục slide
                    return true;
                }
                list.Add(index); // ô có quân (bất kể màu) → thêm, stop
                return false;
            }
        }
    }
}