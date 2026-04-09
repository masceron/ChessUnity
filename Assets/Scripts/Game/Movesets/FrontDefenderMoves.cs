using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrontDefenderMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var color = caller.Color;
            var effectiveMoveRange = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, effectiveMoveRange))
                MakeMove(rankOff, fileOff);
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, effectiveMoveRange))
                MakeMove(rankOff, fileOff);

            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, effectiveMoveRange))
                    MakeMove(rankOff, fileOff);
            }

            return 20 + 5 * effectiveMoveRange;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(index);
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.GetAttackRange();
            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;

            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
                    if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            return 20 + 5 * moveRange;

            bool MakeCapture(int index)
            {
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