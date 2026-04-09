using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class QueenMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange();

            // Rook directions
            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;

            // Bishop directions
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
                if (!MakeMove(rankOff, fileOff)) break;

            return 15 + 5 * moveRange;

            bool MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p != null) return false;
                list.Add(index);
                return true;
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
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
                if (!MakeCapture(rankOff, fileOff)) break;

            return 15 + 5 * moveRange;

            bool MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p == null)
                {
                    list.Add(index);
                    return true;
                }
                list.Add(index);
                return false;
            }
        }
    }
}